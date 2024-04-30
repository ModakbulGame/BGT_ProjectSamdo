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
public class ObjectBaseInfo             // 기본 정보
{
    public string ObjectName;           // 이름
    public float AttackSpeed = 1;       // 공격 속도
    public float MoveSpeed = 5;         // 이동 속도
    public void SetInfo(MonsterScriptable _monster)
    {
        ObjectName = _monster.MonsterName;
        AttackSpeed = _monster.AttackSpeed;
        MoveSpeed = _monster.MoveSpeed;
    }
}

[Serializable]
public class ObjectCombatInfo     // 전투 정보
{
    public float MaxHP;             // 최대 HP
    public float Defense;           // 방어력
    public float Attack;            // 물리 공격력
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
    // 정보
    [SerializeField]
    protected ObjectBaseInfo m_baseInfo = new();                // 기본 정보
    public string ObjectName { get { return m_baseInfo.ObjectName; } }              // 이름
    public virtual float AttackSpeed { get { return m_baseInfo.AttackSpeed; } }     // 공격 속도
    public float MoveSpeed { get { return m_baseInfo.MoveSpeed; } }                 // 현재 속도
    public virtual float ObjectHeight { get { return 2; } }

    public virtual ObjectCombatInfo CombatInfo { get; }       // 전투 정보
    public float MaxHP { get { return CombatInfo.MaxHP; } }                         // 최대 HP
    public virtual float Attack { get { return CombatInfo.Attack; } }               // 물리 공격력
    public float Defense { get { return CombatInfo.Defense; } }                     // 방어력


    public virtual bool IsPlayer { get { return false; } }
    public virtual bool IsMonster { get { return false; } }


        // 버프 디버프 정보
    private List<BuffNDebuff> m_buffNDebuff = new();
    public virtual void GetStatAdjust(StatAdjust _adjust)
    {
        BuffNDebuff buff = new(_adjust, delegate {/* Attack -= _adjust.Amonut;*/ });
        m_buffNDebuff.Add(buff);
        Debug.Log("버프!");
    }

    private void BuffNDebuffProc()
    {
        for (int i = 0; i < m_buffNDebuff.Count; i++)
        {
           bool done = m_buffNDebuff[i].ProcTime();
            if (!done) { /*없애기*/}
        }
    }
}
