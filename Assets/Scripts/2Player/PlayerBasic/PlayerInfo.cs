using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECooltimeName                               // ��Ÿ�� ����
{
    JUMP,
    ROLL,
    GUARD,
    THROW,
    HEAL,
    SKILL1,

    LAST = SKILL1 + ValueDefine.MAX_SKILL_SLOT
}

public struct SPlayerWeaponInfo                         // ���� ����
{
    public FRange WeaponAttack;
    public FRange WeaponMagic;
    public float WeaponAttackSpeed;
    public SPlayerWeaponInfo(WeaponScript _weapon) { WeaponAttack = _weapon.WeaponAttack; WeaponMagic = _weapon.WeaponMagic; WeaponAttackSpeed = _weapon.WeaponAttackSpeed; }
}


[Serializable]
public class PlayerStatInfo                             // �÷��̾� ���� ����
{
    public float m_health;          // ü��
    public float m_endure;          // ������
    public float m_strength;        // �ٷ�
    public float m_intellect;       // ����
    public float m_rapid;           // ��ø
    public float m_mental;          // ����

    public void SetStat(EStatInfoName _name, float _num)      // ���� ����
    {
        switch (_name)
        {
            case EStatInfoName.HEALTH: m_health = _num; break;
            case EStatInfoName.ENDURE: m_endure = _num; break;
            case EStatInfoName.STRENGTH: m_strength = _num; break;
            case EStatInfoName.INTELLECT: m_intellect = _num; break;
            case EStatInfoName.RAPID: m_rapid = _num; break;
            case EStatInfoName.MENTAL: m_mental = _num; break;
        }
    }
    public void UpgradeStat(EStatInfoName _name, float _up)
    {
        switch (_name)
        {
            case EStatInfoName.HEALTH: m_health += _up; break;
            case EStatInfoName.ENDURE: m_endure += _up; break;
            case EStatInfoName.STRENGTH: m_strength += _up; break;
            case EStatInfoName.INTELLECT: m_intellect += _up; break;
            case EStatInfoName.RAPID: m_rapid += _up; break;
            case EStatInfoName.MENTAL: m_mental += _up; break;
        }
    }
    public float GetStat(EStatInfoName _name)
    {
        return _name switch
        {
            EStatInfoName.HEALTH => m_health,
            EStatInfoName.ENDURE => m_endure,
            EStatInfoName.STRENGTH => m_strength,
            EStatInfoName.INTELLECT => m_intellect,
            EStatInfoName.RAPID => m_rapid,
            EStatInfoName.MENTAL => m_mental,
            _ => -1,
        };
    }
    public PlayerStatInfo() { m_health = 10; m_endure = 10; m_strength = 10; m_intellect = 10; m_rapid = 10; m_mental = 10; }
    public PlayerStatInfo(PlayerStatInfo _info, int[] _point)
    {
        m_health = _info.m_health + _point[0];
        m_endure = _info.m_endure + _point[1];
        m_strength = _info.m_strength + _point[2];
        m_intellect = _info.m_intellect + _point[3];
        m_rapid = _info.m_rapid + _point[4];
        m_mental = _info.m_mental + _point[5];
    }
}

[Serializable]
public class PlayerCombatInfo : ObjectCombatInfo        // �÷��̾� ���� ����
{
    public float MaxStamina;        // �ִ� ���׹̳�
    public float Magic;             // ���� ���ݷ�
    public float Overdrive;         // ���� ������
    public float Tolerance;         // ����

    public void SetCombatInfo(PlayerStatInfo _stat)
    {
        MaxHP = FunctionDefine.RoundF1(28.85f + 15 * Mathf.Sqrt(1.6f*_stat.m_health + 0.65f * _stat.m_strength));
        MaxStamina = FunctionDefine.RoundF1(64.6f + (1.5f * _stat.m_endure + _stat.m_health + 0.8f * _stat.m_intellect + 0.24f * _stat.m_rapid));
        Defense = FunctionDefine.RoundF3((0.5f * (_stat.m_endure + 0.3f * _stat.m_mental) - 6.5f)* 0.01f);
        Attack = FunctionDefine.RoundF1(6 * Mathf.Sqrt(_stat.m_strength) - 7);
        Magic = FunctionDefine.RoundF1(6 * Mathf.Sqrt(_stat.m_intellect) - 7);
        Overdrive = FunctionDefine.RoundF3(0.73f + (15 * Mathf.Sqrt(_stat.m_rapid) * 0.01f));
        Tolerance = FunctionDefine.RoundF3(1.2f * _stat.m_mental * 0.01f - 0.12f);
    }
    public override float GetStat(ECombatInfoName _name)
    {
        return _name switch
        {
            ECombatInfoName.MAX_HP => MaxHP,
            ECombatInfoName.MAX_STAMINA => MaxStamina,
            ECombatInfoName.DEFENSE => Defense,
            ECombatInfoName.ATTACK => Attack,
            ECombatInfoName.MAGIC => Magic,
            ECombatInfoName.OVERDRIVE => Overdrive,
            ECombatInfoName.TOLERANCE => Tolerance,
            _ => -1,
        };
    }
    public PlayerCombatInfo(PlayerStatInfo _stat)
    {
        SetCombatInfo(_stat);
    }
}

public partial class PlayerController
{
    // ������
    public void LoadData()
    {
        SaveData data = PlayManager.CurSaveData;

        transform.position = data.PlayerPos;
        transform.localEulerAngles = data.PlayerRot;
    }
    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        data.PlayerPos = transform.position;
        data.PlayerRot = transform.localEulerAngles;
    }


    // �⺻ ������Ʈ
    private CapsuleCollider m_collider;


    // ���� ����
    public override float ObjectHeight { get { return m_collider.height; } }        // ������Ʈ ����
    private float HalfHeight { get { return ObjectHeight / 2; } }

    public override void ApplyHPUI()
    {
        PlayManager.SetPlayerMaxHP(MaxHP);
        PlayManager.SetPlayerCurHP(CurHP);
    }


    // ���� ����
    [SerializeField]
    protected PlayerStatInfo m_statInfo = new();    // ���� ����
    public float Health { get { return m_statInfo.m_health; } }                     // ü��
    public float Endure { get { return m_statInfo.m_endure; } }                     // ������
    public float Strength { get { return m_statInfo.m_strength; } }                 // �ٷ�
    public float Intellect { get { return m_statInfo.m_intellect; } }               // ����
    public float Rapid { get { return m_statInfo.m_rapid; } }                       // ��ø
    public float Mental { get { return m_statInfo.m_mental; } }                     // ����
    public PlayerStatInfo GetStatInfo() { return m_statInfo; }
    public void SetStat(EStatInfoName _name, float _num) { m_statInfo.SetStat(_name, _num); ApplyStat(); }      // ���� ����


    // ���� ����
    [SerializeField]
    private PlayerCombatInfo m_combatInfo;
    public override ObjectCombatInfo CombatInfo { get { return m_combatInfo; } }
    public override float Attack => base.Attack + WeaponAttack.Num;                 // ���ݷ�
    public float MaxStamina { get { return m_combatInfo.MaxStamina; } }             // �ִ� ���׹̳�
    public override float AttackSpeed => base.AttackSpeed * WeaponAttackSpeed;      // ���� �ӵ�
    public float Magic { get { return m_combatInfo.Magic + WeaponMagic.Num; } }     // �ּ� ���ݷ�
    public float Tolerance { get { return m_combatInfo.Tolerance; } }               // ����
    public float Defence { get { return m_combatInfo.Defense; } }                   // ����
    public float Overdrive { get { return m_combatInfo.Overdrive; } }               // ����
    public void ApplyStat() { m_combatInfo.SetCombatInfo(m_statInfo); }            // ���� ����

    public override float DamageMultiplier => base.DamageMultiplier * Overdrive;


    // ��� ����
    [SerializeField]
    private WeaponScript[] m_weapons;
    [SerializeField]
    private Transform m_throwItemTransform;

    public override ObjectAttackScript AttackObject { get { return CurWeapon; } }
    private WeaponScript CurWeapon { get; set; }
    public SPlayerWeaponInfo CurWeaponInfo { get { return new(CurWeapon); } }
    public FRange WeaponAttack { get { return CurWeaponInfo.WeaponAttack; } }
    public FRange WeaponMagic { get { return CurWeaponInfo.WeaponMagic; } }
    public float WeaponAttackSpeed { get { return CurWeaponInfo.WeaponAttackSpeed; } }

    private void InitWeapon()
    {
        foreach (WeaponScript weapon in m_weapons) { if (weapon.gameObject.activeSelf) { CurWeapon = weapon; } }
        if (CurWeapon != null)
        {
            PlayManager.SetCurWeapon(CurWeapon.WeaponEnum);
        }
        EWeaponName name = PlayManager.CurWeapon;
        SetCurWeapon(name);
    }
    public void SetCurWeapon(EWeaponName _weapon)
    {
        foreach (WeaponScript weapon in m_weapons)
        {
            if (weapon.WeaponEnum == _weapon) { CurWeapon = weapon; weapon.gameObject.SetActive(true); }
            else if (weapon.gameObject.activeSelf) { weapon.gameObject.SetActive(false); }
        }
        SetWeaponAnimationLayer(CurWeapon.WeaponType);
        SetWeaponName();
    }
    private void SetWeaponName()
    {
        string name;
        switch (CurWeapon.WeaponType)
        {
            case EWeaponType.BLADE: name = "Blade"; break;
            case EWeaponType.SWORD: name = "Sword"; break;
            case EWeaponType.SCEPTER: name = "Scepter"; break;
            default: name = "Weapon"; break;
        }
        CurWeapon.gameObject.name = name;
    }
    private void SetWeaponCCType(ECCType _cc) { CurWeapon.SetCCType(_cc); }
    private void ResetWeaponCCType() { CurWeapon.ResetCCType(); }



    private SkillCastingEffect m_castingEffect;
    private void CastingEffectOn() { m_castingEffect.ShowEffect(); }
    private void CastingEffectOff() { m_castingEffect.HideEffect(); }




    // ��� ����
    public override bool IsPlayer { get { return true; } }


    // �ʱ� ����
    public override void SetComps()
    {
        base.SetComps();
        m_collider = GetComponentInChildren<CapsuleCollider>();
        FunctionDefine.SetFriction(m_collider, FloorFriction, true);
        m_castingEffect = GetComponentInChildren<SkillCastingEffect>();
        m_castingEffect.SetComps();
    }
    private void SetStates()                // ���µ� �߰�
    {
        m_stateManager = new(this);
        m_playerStates[(int)EPlayerState.IDLE] = gameObject.AddComponent<PlayerIdleState>();
        m_playerStates[(int)EPlayerState.MOVE] = gameObject.AddComponent<PlayerMoveState>();
        m_playerStates[(int)EPlayerState.JUMP] = gameObject.AddComponent<PlayerJumpState>();
        m_playerStates[(int)EPlayerState.FALL] = gameObject.AddComponent<PlayerFallState>();
        m_playerStates[(int)EPlayerState.ATTACK] = gameObject.AddComponent<PlayerAttackState>();
        m_playerStates[(int)EPlayerState.GUARD] = gameObject.AddComponent<PlayerGaurdState>();
        m_playerStates[(int)EPlayerState.SKILL] = gameObject.AddComponent<PlayerSkillState>();
        m_playerStates[(int)EPlayerState.ROLL] = gameObject.AddComponent<PlayerRollState>();
        m_playerStates[(int)EPlayerState.THROW] = gameObject.AddComponent<PlayerThrowState>();
        m_playerStates[(int)EPlayerState.HIT] = gameObject.AddComponent<PlayerHitState>();
        m_playerStates[(int)EPlayerState.DIE] = gameObject.AddComponent<PlayerDieState>();
    }
    public override void SetInfo()
    {
        m_combatInfo = new PlayerCombatInfo(m_statInfo);
        ApplyStat();
    }

    public override void Awake()
    {
        base.Awake();
        SetStates();
        SetAnimator();
        SetInfo();
    }

    public override void Start()
    {
        base.Start();
        PlayManager.SetCurPlayer(this);     // PlayerManger�� �÷��̾� ���
        ChangeState(EPlayerState.IDLE);     // Idle�� ���� ����
        PlayManager.SetPlayerMaxHP(MaxHP);  // ü�¹� ����
        InitStamina();                      // ���׹̳� ����
        InitLight();                        // �ɷ� �ʱ� ����
        InitWeapon();                       // ���� ����
        HideSkillAim();                     // ��ų ���� ����
    }
}