using Cinemachine;
using System;
using System.Collections;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerStatInfo             // 오브젝트 스탯 정보
{
    public float m_health;          // 체력
    public float m_endure;          // 지구력
    public float m_strength;        // 근력
    public float m_intellect;       // 지력
    public float m_rapid;           // 민첩
    public float m_mental;          // 정신

    public void SetStat(EStatInfoName _name, float _num)      // 스탯 설정
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

public struct SPlayerWeaponInfo
{
    public FRange WeaponAttack;
    public FRange WeaponMagic;
    public float WeaponAttackSpeed;
    public SPlayerWeaponInfo(WeaponScript _weapon) { WeaponAttack = _weapon.WeaponAttack; WeaponMagic = _weapon.WeaponMagic; WeaponAttackSpeed = _weapon.WeaponAttackSpeed; }
}

public partial class PlayerController : ObjectScript
{
    // 플레이어 정보
    private CapsuleCollider m_collider;
    public override float ObjectHeight { get { return m_collider.height; } }

    [SerializeField]
    protected PlayerStatInfo m_statInfo = new();    // 스탯 정보
    public float Health { get { return m_statInfo.m_health; } }                     // 체력
    public float Endure { get { return m_statInfo.m_endure; } }                     // 지구력
    public float Strength { get { return m_statInfo.m_strength; } }                 // 근력
    public float Intellect { get { return m_statInfo.m_intellect; } }               // 지력
    public float Rapid { get { return m_statInfo.m_rapid; } }                       // 민첩
    public float Mental { get { return m_statInfo.m_mental; } }                     // 정신
    public PlayerStatInfo GetStatInfo() { return m_statInfo; }
    public void SetStat(EStatInfoName _name, float _num) { m_statInfo.SetStat(_name, _num); ApplyStat(); }      // 스탯 설정

    public override bool IsPlayer { get { return true; } }


    // 플레이어 전투 정보
    [SerializeField]
    private PlayerCombatInfo m_combatInfo;
    public override ObjectCombatInfo CombatInfo { get { return m_combatInfo; } }
    public override float Attack => base.Attack + WeaponAttack.Num;                 // 공격력
    public float MaxStamina { get { return m_combatInfo.MaxStamina; } }             // 최대 스테미나
    public override float AttackSpeed => base.AttackSpeed * WeaponAttackSpeed;      // 공격 속도
    public float Magic { get { return m_combatInfo.Magic + WeaponMagic.Num; } }     // 주술 공격력
    public float Tolerance { get { return m_combatInfo.Tolerance; } }               // 내성
    public float Defence { get { return m_combatInfo.Defense; } }                   // 방어력
    public float Overdrive { get { return m_combatInfo.Overdrive; } }               // 증폭
    private void ApplyStat()    // 스탯 -> 전투 적용
    {
        m_combatInfo.SetCombatInfo(m_statInfo);
    }

    public FRange WeaponAttack { get { return CurWeaponInfo.WeaponAttack; } }
    public FRange WeaponMagic { get { return CurWeaponInfo.WeaponMagic; } }
    public float WeaponAttackSpeed { get { return CurWeaponInfo.WeaponAttackSpeed; } }


    // 플레이어 상태
    public Vector2 PlayerAimDirection { get { return FunctionDefine.DegToVec(PlayManager.CameraRotation); } }       // 플레이어 에임 (좌우 각도)
    public float PlayerAimAngle { get { return PlayManager.CameraAngle; } }                                         // 플레이어 에임 (위아래 각도)
    public Vector3 PlayerAimVector
    {
        get
        {
            float angleRad = (PlayerAimAngle+45) * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);
            Vector2 dir = PlayerAimDirection * cos;
            return new(dir.x, sin, dir.y);
        }
    }                                                                              // 플레이어 에임 (3D 벡터)

    public float CurStamina { get; protected set; }                             // 현재 스테미나

    public bool IsIdle { get { return CurState.StateEnum == EPlayerState.IDLE; } }                                  // IDLE
    public bool IsMoving { get { return CurState.StateEnum == EPlayerState.MOVE; } }                                // 이동 중
    public bool IsAttacking { get { return CurState.StateEnum == EPlayerState.ATTACK; } }                           // 공격 중
    public bool IsSkilling { get { return CurState.StateEnum == EPlayerState.SKILL; } }                             // 스킬 중
    public bool IsJumping { get { return CurState.StateEnum == EPlayerState.JUMP; } }                               // 점프 중
    public bool IsRolling { get { return CurState.StateEnum == EPlayerState.ROLL; } }                               // 구르기 중
    public bool IsThrowing { get { return CurState.StateEnum == EPlayerState.THROW; } }                             // 구르기 중
    public bool IsGuarding { get; private set; }                                                                    // 가드 중
    public float JumpCoolTime { get; private set; }                                                                 // 점프 쿨타임
    public float RollCoolTime { get; private set; }                                                                 // 점프 쿨타임

    private bool[] m_fieldDebuff = new bool[(int)EProperty.LAST];
    public void SetFieldDebuff(EProperty _property) { m_fieldDebuff[(int)_property] = true; }



    // 카메라 관련
    private CameraChange m_cameraChange;

    // 상태 관리
    private PlayerStateManager m_stateManager;                                                                      // 상태 관리자
    private IPlayerState CurState { get { return m_stateManager.CurState; } }                                       // 현재 상태
    private readonly IPlayerState[] m_playerStates = new IPlayerState[(int)EPlayerState.LAST];                      // 상태 클래스들
    public void ChangeState(EPlayerState _state) { m_stateManager.ChangeState(m_playerStates[(int)_state]); }       // 상태 변환


    // InputSystem 관련
    private InputSystem.PlayerActions PlayerInput { get { return GameManager.PlayerInputs; } }                      // Input System Player 입력
    public Vector2 InputVector { get { return PlayerInput.Move.ReadValue<Vector2>(); } }                            // WASD 방향
    public bool JumpPressing { get; private set; }                                                                  // 스페이스바 입력 중
    public bool RollPressing { get; private set; }
    public bool AttackTrigger { get { return PlayerInput.Attack.triggered; } }                                      // 좌클릭
    public bool[] SkillTriggers
    { get { return new bool[ValueDefine.MAX_SKILL_SLOT]                                                             // 키보드 123
    { PlayerInput.Skill1.triggered, PlayerInput.Skill2.triggered, PlayerInput.Skill3.triggered }; } }
    public bool GuardTrigger { get { return PlayerInput.Guard.triggered; } }                                        // 우클릭
    public bool GuardPressing { get; private set; }                                                                 // 우클릭 입력 중
    public bool LightTrigger { get { return PlayerInput.Light.triggered; } }                                        // T 입력
    public bool InteractTrigger { get { return PlayerInput.Interact.triggered; } }                                  // E 입력
    public bool ThrowItemTrigger { get { return PlayerInput.ThrowItem.triggered; } }                                // 숫자 입력


    // 장비 관련
    public override ObjectAttackScript AttackObject { get { return CurWeapon; } }
    private WeaponScript CurWeapon { get; set; }
    public SPlayerWeaponInfo CurWeaponInfo { get { return new(CurWeapon); } }
    [SerializeField]
    private WeaponScript[] m_weapons;
    [SerializeField]
    private Transform m_throwItemTransform;
    private void InitWeapon()
    {
        foreach(WeaponScript weapon in m_weapons) { if (weapon.gameObject.activeSelf) { CurWeapon = weapon; } }
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
            if(weapon.WeaponEnum == _weapon) { CurWeapon = weapon; weapon.gameObject.SetActive(true); }
            else if(weapon.gameObject.activeSelf) { weapon.gameObject.SetActive(false); }
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

    // 스테미나 관련
    private float StaminaRestoreRate = 1.5f;        // 초당 스테미나 회복
    private void InitStamina() { CurStamina = MaxStamina; SetStaminaRate(); }       // 스테미나 초기 설정
    public void UseStamina(float _use)              // 스테미나 사용
    {
        CurStamina -= _use;
        if (CurStamina < 0) { CurStamina = 0; }
        SetStaminaRate();
    }
    private void StaminaUpdate()                    // 스테미나 업데이트
    {
        if (CurStamina < MaxStamina)
        {
            CurStamina += StaminaRestoreRate * Time.deltaTime;
            if (CurStamina > MaxStamina) { CurStamina = MaxStamina; }
        }
        SetStaminaRate();
    }
    private void SetStaminaRate()                   // 스테미나 UI 설정
    {
        float rate = CurStamina / MaxStamina;
        PlayManager.SetStaminaRate(rate);
    }


    // 에임 관련
    public void ShowSkillAim(float _radius, float _range)       // 스킬 에임 보이기
    {
        PlayManager.ShowSkillAim(Position, _radius, _range);
    }
    public void TraceSkillAim()
    {
        PlayManager.TraceSkillAim(Position, SkillInfoInHand.SkillCastRange);
    }
    public void HideSkillAim()                                  // 스킬 에임 숨기기
    {
        PlayManager.HideSkillAim();
    }


    // 능력 관련
    [SerializeField]
    private PlayerLightScript m_light;                                              // 불빛 프리펍

    private const float MAX_LIGHT_POWER = 5;        // 최대 능력 스테미나
    private float LightUseRate = 0.5f;              // 초당 능력 사용
    private float LightRestoreRate = 1;             // 초당 능력 회복
    private float LightRestoreDelay = 2;            // 능력 다 썼을 때 딜레이

    private float CurLightRate { get; set; }        // 현재 능력 스테미나(?)
    private bool CanRestoreLight { get; set; } = true;      // 회복 가능
    public bool IsLightOn { get; private set; }                                 // 불빛이 켜져 있는지

    public void LightOn()
    {
        m_light.LightOn();
        IsLightOn = true;
    }
    public void LightOff() 
    {
        m_light.LightOff();
        IsLightOn = false;
    }                       // 능력 중단
    private void InitLight() { CurLightRate = MAX_LIGHT_POWER; SetLightRate(); }    // 초기 설정
    public void LightChange()                                   // T 눌렀을 시 실행
    {
        if (PlayerLightScript.CurState == ELightState.CHANGE) { return; }

        if (IsLightOn) { LightOff(); }
        else if (CurLightRate > 0) { LightOn(); }
    }
    private void LightUpdate()                      // 능력 업데이트
    {
        if (IsLightOn)
        {
            if (Interacting) { return; }
            CurLightRate -= LightUseRate * Time.deltaTime;
            if (CurLightRate <= 0) { CurLightRate = 0; LightOff(); CanRestoreLight = false; StartCoroutine(LightRestoreCoolTime()); }
        }
        else if (CurLightRate < MAX_LIGHT_POWER)
        {
            if (!CanRestoreLight) { return; }
            CurLightRate += LightRestoreRate * Time.deltaTime;
            if (CurLightRate >= MAX_LIGHT_POWER) { CurLightRate = MAX_LIGHT_POWER; }
        }
        SetLightRate();
    }
    private void SetLightRate()                     // 능력 UI 설정
    {
        float rate = CurLightRate / MAX_LIGHT_POWER;
        PlayManager.SetLightRate(rate);
    }
    private IEnumerator LightRestoreCoolTime()
    {
        yield return new WaitForSeconds(LightRestoreDelay);
        CanRestoreLight = true;
    }


    // 기타
    private void UpdateCooltime()
    {
        if (JumpCoolTime > 0) { JumpCoolTime -= Time.deltaTime; }   // 점프 쿨타임
        if (RollCoolTime > 0) { RollCoolTime -= Time.deltaTime; }
        for (int i = 0; i<ValueDefine.MAX_SKILL_SLOT; i++)              // 스킬 쿨타임
        {
            if (SkillCooltime[i] > 0) { SkillCooltime[i] -= Time.deltaTime; }
        }
    }



    // 기본 설정
    public override void SetComps()
    {
        base.SetComps();
        m_collider = GetComponentInChildren<CapsuleCollider>();
        FunctionDefine.SetFriction(m_collider, FLOOR_FRICTION, true);
    }
    private void SetStates()                // 상태들 추가
    {
        m_stateManager = new(this);
        m_playerStates[(int)EPlayerState.IDLE] = gameObject.AddComponent<PlayerIdleState>();
        m_playerStates[(int)EPlayerState.MOVE] = gameObject.AddComponent<PlayerMoveState>();
        m_playerStates[(int)EPlayerState.JUMP] = gameObject.AddComponent<PlayerJumpState>();
        m_playerStates[(int)EPlayerState.FALL] = gameObject.AddComponent<PlayerFallState>();
        m_playerStates[(int)EPlayerState.ATTACK] = gameObject.AddComponent<PlayerAttackState>();
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
        PlayManager.SetCurPlayer(this);     // PlayerManger에 플레이어 등록
        ChangeState(EPlayerState.IDLE);     // Idle로 상태 전이
        PlayManager.SetPlayerMaxHP(MaxHP);  // 체력바 설정
        InitStamina();                      // 스테미나 설정
        InitLight();                        // 능력 초기 설정
        InitWeapon();                       // 무기 설정
        HideSkillAim();                     // 스킬 에임 끄기
    }

    private void UpdateInputs()
    {
        JumpPressing = PlayerInput.Jump.IsPressed();
        RollPressing = PlayerInput.Roll.IsPressed();
        GuardPressing = PlayerInput.Guard.IsPressed();
    }

    public override void Update()
    {
        base.Update();
        UpdateInputs();                     // InputSystem 상태 반영

        CurState.Proceed();                 // 현재 상태 Update

        UpdateCooltime();                   // 쿨타임
        StaminaUpdate();                    // 스테미나

        GuardUpdate();                      // 가드 상태
        LightUpdate();                      // 능력 상태

        PlayerDetactUpdate();               // 상호작용 물체들 관리
    }

    private void FixedUpdate()
    {
        PrePhysicsUpdate();

        CurState.FixedProceed();

        LatePhysicsUpdate();
    }
}
