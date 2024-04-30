using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAdjType
{
    DAMAGE,
    ATTACK,
    MAGIC,

    LAST
}

[Serializable]
public struct StatAdjust
{
    public EAdjType Type;
    public float Amonut;
    public float Time;
    public bool IsNull { get { return Type == EAdjType.LAST; } }
    public static StatAdjust Null { get { return new(EAdjType.LAST, 0, 0); } }
    public StatAdjust(EAdjType _type, float _amount, float _time)
    {
        Type = _type; Amonut = _amount; Time = _time;
    }
}

public class BuffNDebuff
{
    private StatAdjust m_adjInfo;
    private float TimeCount { get; set; }
    public bool ProcTime()
    {
        TimeCount -= Time.deltaTime;
        if (TimeCount <= 0)
        {
            m_resetFunction();
            return false;
        }
        return true;
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
    // ����
    [SerializeField]
    protected ObjectBaseInfo m_baseInfo = new();                // �⺻ ����
    public string ObjectName { get { return m_baseInfo.ObjectName; } }              // �̸�
    public virtual float AttackSpeed { get { return m_baseInfo.AttackSpeed; } }     // ���� �ӵ�
    public float MoveSpeed { get { return m_baseInfo.MoveSpeed; } }                 // ���� �ӵ�
    public virtual float ObjectHeight { get { return 2; } }

    public virtual ObjectCombatInfo CombatInfo { get; }       // ���� ����
    public float MaxHP { get { return CombatInfo.MaxHP; } }                         // �ִ� HP
    public virtual float Attack { get { return CombatInfo.Attack; } }               // ���� ���ݷ�
    public float Defense { get { return CombatInfo.Defense; } }                     // ����


    public virtual bool IsPlayer { get { return false; } }
    public virtual bool IsMonster { get { return false; } }


        // ���� ����� ����
    private List<BuffNDebuff> m_buffNDebuff = new();
    public virtual void GetStatAdjust(StatAdjust _adjust)
    {
        BuffNDebuff buff = new(_adjust, delegate {/* Attack -= _adjust.Amonut;*/ });
        m_buffNDebuff.Add(buff);
        Debug.Log("����!");
    }

    private void BuffNDebuffProc()
    {
        for (int i = 0; i < m_buffNDebuff.Count; i++)
        {
           bool done = m_buffNDebuff[i].ProcTime();
            if (!done) { /*���ֱ�*/}
        }
    }
}
