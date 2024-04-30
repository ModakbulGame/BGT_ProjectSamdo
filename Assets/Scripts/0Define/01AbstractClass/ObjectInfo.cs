using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAdjType
{
    DAMAGE,
    ATTACK,
    MAGIC,
    MOVE_SPEED,

    LAST
}

[Serializable]
public struct StatAdjust
{
    public EAdjType Type;
    public float Amount;
    public float Time;
    public bool IsNull { get { return Type == EAdjType.LAST; } }
    public static StatAdjust Null { get { return new(EAdjType.LAST, 0, 0); } }
    public StatAdjust(EAdjType _type, float _amount, float _time)
    {
        Type = _type; Amount = _amount; Time = _time;
    }
}

[Serializable]
public class BuffNDebuff
{
    [SerializeField]
    private StatAdjust m_adjInfo;
    public EAdjType AdjType { get { return m_adjInfo.Type; } }
    public float Amount { get { return m_adjInfo.Amount; } }
    public bool IsBuff { get { return m_adjInfo.Amount > 0; } }
    private float m_timeCount;
    public float TimeCount { get { return m_timeCount; } private set { m_timeCount = value; } }
    public void SetTimeCount(float _time) { TimeCount = _time; }
    public void SetReset(FPointer _reset) { m_resetFunction = _reset; }
    public bool ProcTime()
    {
        TimeCount -= Time.deltaTime;
        if (TimeCount <= 0)
        {
            m_resetFunction();
            return true;
        }
        return false;
    }
    private FPointer m_resetFunction;
    public BuffNDebuff(StatAdjust _adj, FPointer _reset)
    {
        m_adjInfo = _adj; m_resetFunction = _reset;
    }
}


[Serializable]
public class ObjectBaseInfo             // �⺻ ����
{
    public string ObjectName;           // �̸�
    public float AttackSpeed = 1;       // ���� �ӵ�
    public float MoveSpeed = 5;         // �̵� �ӵ�
    public void SetInfo(MonsterScriptable _monster)
    {
        ObjectName = _monster.MonsterName;
        AttackSpeed = _monster.AttackSpeed;
        MoveSpeed = _monster.MoveSpeed;
    }
}

[Serializable]
public class ObjectCombatInfo     // ���� ����
{
    public float MaxHP;             // �ִ� HP
    public float Defense;           // ����
    public float Attack;            // ���� ���ݷ�
    public virtual void SetInfo(MonsterScriptable _monster)
    {
        MaxHP = _monster.MaxHP;
        Defense = 5;
        Attack = _monster.Attack;
    }
    public virtual float GetStat(ECombatInfoName _name)
    {
        return _name switch
        {
            ECombatInfoName.MAX_HP => MaxHP,
            ECombatInfoName.MAX_STAMINA => -1,
            ECombatInfoName.DEFENSE => Defense,
            ECombatInfoName.ATTACK => Attack,
            ECombatInfoName.MAGIC => -1,
            ECombatInfoName.OVERDRIVE => -1,
            ECombatInfoName.TOLERANCE => -1,
            _ => -1,
        };
    }
}

public abstract partial class ObjectScript
{
    // �⺻ ����
    [SerializeField]
    protected ObjectBaseInfo m_baseInfo = new();                // �⺻ ����
    public string ObjectName { get { return m_baseInfo.ObjectName; } }                      // �̸�
    public virtual float AttackSpeed { get { return m_baseInfo.AttackSpeed; } }             // ���� �ӵ�
    public float MoveSpeed { get { return m_baseInfo.MoveSpeed * MoveSpeedMultiplier; } }   // �̵� �ӵ�
    public virtual float ObjectHeight { get { return 2; } }

    public float MoveSpeedMultiplier { get; protected set; } = 1;

    public virtual void SetMoveMultiplier(float _multiplier) { MoveSpeedMultiplier = _multiplier; }


    // ���� ����
    public virtual ObjectCombatInfo CombatInfo { get; }       // ���� ����
    public float MaxHP { get { return CombatInfo.MaxHP; } }                         // �ִ� HP
    public virtual float Attack { get { return CombatInfo.Attack; } }               // ���� ���ݷ�
    public float Defense { get { return CombatInfo.Defense; } }                     // ����


    public float DamageMultiplier { get; protected set; } = 1;
    public float AttackMultiplier { get; protected set; } = 1;
    public float MagicMultiplier { get; protected set; } = 1;


    public virtual bool IsPlayer { get { return false; } }
    public virtual bool IsMonster { get { return false; } }


    // ���� ����� ����
    [SerializeField]
    private readonly List<BuffNDebuff> m_buffNDebuff = new();
    public virtual void GetStatAdjust(StatAdjust _adjust)
    {
        BuffNDebuff info = null;
        float replace;
        replace = GetBuffed(_adjust);
        if (replace != 0) info = new(_adjust, delegate { SetMultiplier(_adjust.Type, replace); });

        if (info != null) m_buffNDebuff.Add(info);
    }

    private float GetBuffed(StatAdjust _adj)
    {
        EAdjType type = _adj.Type;
        float amount = _adj.Amount;
        bool isBuff = amount > 0;
        float time = _adj.Time;
        for (int i = 0; i<m_buffNDebuff.Count; i++)
        {
            BuffNDebuff buff = m_buffNDebuff[i];
            if (buff.IsBuff != isBuff) { continue; }
            if (buff.AdjType != type) { continue; }

            if (buff.Amount == amount)
            {
                if (buff.TimeCount < time) { buff.SetTimeCount(time); }
                return 0;
            }
            else if (buff.Amount < amount)
            {
                SetMultiplier(type, amount);
                if (buff.TimeCount > time) { return amount; }
                else if (buff.TimeCount < time) { buff.SetTimeCount(time); }
            }
            else
            {
                if (buff.TimeCount >= time) { return 0; }
            }
        }
        return 1;
    }

    private void SetMultiplier(EAdjType _type, float _multiplier)
    {
        switch (_type)
        {
            case EAdjType.DAMAGE:
                DamageMultiplier = _multiplier;
                break;
            case EAdjType.ATTACK:
                AttackMultiplier = _multiplier;
                break;
            case EAdjType.MAGIC:
                MagicMultiplier = _multiplier;
                break;
            case EAdjType.MOVE_SPEED:
                SetMoveMultiplier(_multiplier);
                break;
        }
    }


    private void BuffNDebuffProc()
    {
        Queue<BuffNDebuff> removeQue = new();
        for (int i = 0; i < m_buffNDebuff.Count; i++)
        {
            bool done = m_buffNDebuff[i].ProcTime();
            if (done) { removeQue.Enqueue(m_buffNDebuff[i]); }
        }
        while (removeQue.Count > 0) { BuffNDebuff buff = removeQue.Dequeue(); m_buffNDebuff.Remove(buff); }
    }
}
