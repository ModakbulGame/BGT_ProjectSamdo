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
    MAX_HP,

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
    [SerializeField]
    private float m_timeCount;
    public float TimeCount { get { return m_timeCount; } private set { m_timeCount = value; } }
    public void SetTimeCount(float _time) { TimeCount = _time; }
    public void SetAmount(float _amount) { m_adjInfo.Amount = _amount; }
    public void SetReset(FPointer _reset) { m_resetFunction = _reset; }
    public bool ProcTime()
    {
        TimeCount -= Time.deltaTime;
        if (TimeCount <= 0)
        {
            if(m_resetFunction != null)
                m_resetFunction();
            return true;
        }
        return false;
    }
    private FPointer m_resetFunction;
    public BuffNDebuff(StatAdjust _adj, FPointer _reset)
    {
        m_adjInfo = _adj; m_resetFunction = _reset; SetTimeCount(_adj.Time);
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
    // 기본 컴포넌트
    protected Rigidbody m_rigid;            // Rigidbody
    protected Animator m_anim;              // Animator


    // 기본 정보
    [SerializeField]
    protected ObjectBaseInfo m_baseInfo = new();                // 기본 정보
    public string ObjectName { get { return m_baseInfo.ObjectName; } }                      // 이름
    public virtual float AttackSpeed { get { return m_baseInfo.AttackSpeed; } }             // 공격 속도
    public float MoveSpeed { get { return m_baseInfo.MoveSpeed * MoveSpeedMultiplier; } }   // 이동 속도
    public virtual float ObjectHeight { get { return 2; } }                                 // 오브젝트 높이

    public float MoveSpeedMultiplier { get; protected set; } = 1;                           // 이동 배율

    public virtual void SetMoveMultiplier(float _multiplier) { MoveSpeedMultiplier = _multiplier; }     // 이동 배율 설정


    // 전투 정보
    public virtual ObjectCombatInfo CombatInfo { get; }       // 전투 정보
    public float MaxHP { get { return CombatInfo.MaxHP * MaxHPMultiplier; } }       // 최대 HP
    public virtual float Attack { get { return CombatInfo.Attack; } }               // 물리 공격력
    public float Defense { get { return CombatInfo.Defense; } }                     // 방어력

    public float MaxHPMultiplier { get; protected set; } = 1;

    private float ExtraHP { get; set; } = 0;
    public virtual void SetMaxHPMultiplier(float _multiplier)
    {
        if(_multiplier == 1) { ResetMaxHP(); return; }

        MaxHPMultiplier = _multiplier;
        ExtraHP = MaxHP - CombatInfo.MaxHP;
        CurHP += ExtraHP;

        ApplyHPUI();
    }
    public virtual void ResetMaxHP()
    {
        MaxHPMultiplier = 1;
        if(ExtraHP > 0) { CurHP -= ExtraHP; }

        ExtraHP = 0;
        ApplyHPUI();
    }
    public virtual void ApplyHPUI() { }


    [SerializeField]
    private float m_dm = 1;     // 데미지 배율 (임시)

    public float DamageMultiplier { get { return m_dm; } protected set { m_dm = value; } }
    public float AttackMultiplier { get; protected set; } = 1;
    public float MagicMultiplier { get; protected set; } = 1;

    public virtual ObjectAttackScript AttackObject { get; set; }    // 부속 공격 판정


    // 상속 정보
    public virtual bool IsPlayer { get { return false; } }
    public virtual bool IsMonster { get { return false; } }


    // 버프 디버프 정보
    [SerializeField]
    private List<BuffNDebuff> m_buffNDebuff = new();                // 버프, 디버프 리스트

    public virtual void GetStatAdjust(StatAdjust _adjust)           // 능력치 조정
    {
        BuffNDebuff info = null;
        float replace;
        replace = GetBuffed(_adjust);
        if (replace != 0) info = new(_adjust, delegate { ResetMultiplier(_adjust.Type, _adjust.Amount > 1); });

        if (info != null) m_buffNDebuff.Add(info);
    }

    private float GetBuffed(StatAdjust _adj)            // 버프, 디버프 받기
    {
        EAdjType type = _adj.Type;
        float amount = _adj.Amount;
        bool isBuff = amount > 1;
        float time = _adj.Time;
        bool exist = false;
        for (int i = 0; i<m_buffNDebuff.Count; i++)
        {
            BuffNDebuff buff = m_buffNDebuff[i];
            if (buff.IsBuff != isBuff) { continue; }
            if (buff.AdjType != type) { continue; }
            exist = true;

            if (buff.Amount == amount)
            {
                if (buff.TimeCount < time) { buff.SetTimeCount(time); }
                return 0;
            }
            else if (buff.Amount < amount)
            {
                SetMultiplier(type, amount);
                if (buff.TimeCount > time) { return amount; }
                else if (buff.TimeCount < time) { buff.SetAmount(amount); buff.SetTimeCount(time); return 0; }
                else { buff.SetAmount(amount); return 0; }
            }
            else
            {
                if (buff.TimeCount >= time) { return 0; }
            }
        }
        if (!exist) { SetMultiplier(type, amount); }
        return 1;
    }

    private void ResetMultiplier(EAdjType _type, bool _isBuff)          // 버프, 디버프 종료
    {
        float max = 1;
        for (int i = 0; i<m_buffNDebuff.Count; i++)
        {
            if (m_buffNDebuff[i].AdjType != _type || m_buffNDebuff[i].IsBuff != _isBuff || m_buffNDebuff[i].TimeCount <= 0)
                continue;
            float amount = m_buffNDebuff[i].Amount;
            if ((_isBuff && max < amount) || (!_isBuff && max > amount)) { max = m_buffNDebuff[i].Amount; }
        }
        SetMultiplier(_type, max);
    }
    private void SetMultiplier(EAdjType _type, float _multiplier)       // 배율 설정
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
            case EAdjType.MAX_HP:
                SetMaxHPMultiplier(_multiplier);
                break;
        }
    }

    private void BuffNDebuffProc()                                      // 버프 디버프 쿨타임 적용
    {
        Queue<BuffNDebuff> removeQue = new();
        for (int i = 0; i < m_buffNDebuff.Count; i++)
        {
            bool done = m_buffNDebuff[i].ProcTime();
            if (done) { removeQue.Enqueue(m_buffNDebuff[i]); }
        }
        while (removeQue.Count > 0) { BuffNDebuff buff = removeQue.Dequeue(); m_buffNDebuff.Remove(buff); }
    }


    // 초기 설정
    public virtual void SetComps()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_anim = GetComponentInChildren<Animator>();
        SetAttackObject();
    }
    public virtual void SetInfo() { }
    public virtual void SetAttackObject()
    {
        AttackObject = GetComponentInChildren<ObjectAttackScript>();
    }

    public virtual void Awake() { SetComps(); }
    public virtual void Start() { SetHP(MaxHP); CurSpeed = MoveSpeed; }
}
