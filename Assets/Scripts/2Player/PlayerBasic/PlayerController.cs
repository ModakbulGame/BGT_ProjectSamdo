using Cinemachine;
using System;
using System.Collections;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController : ObjectScript
{
    // 입력 관련 관련
    private InputSystem.PlayerActions PlayerInput { get { return GameManager.PlayerInputs; } }                      // Input System Player 입력
    public Vector2 InputVector { get { return PlayerInput.Move.ReadValue<Vector2>(); } }                            // WASD 방향
    public bool JumpPressing { get; private set; }                                                                  // 스페이스바
    public bool RollPressing { get; private set; }                                                                  // 쉬프트
    public bool AttackTrigger { get { return PlayerInput.Attack.triggered; } }                                      // 좌클릭
    public bool[] SkillTriggers { get {
            return new bool[ValueDefine.MAX_SKILL_SLOT] { 
                PlayerInput.Skill1.triggered, PlayerInput.Skill2.triggered, PlayerInput.Skill3.triggered }; } }                                                                         // 숫자 123
    public bool GuardTrigger { get { return PlayerInput.Guard.triggered; } }                                        // 우클릭
    public bool GuardPressing { get; private set; }                                                                 // 우클릭 입력 중
    public bool LightTrigger { get { return PlayerInput.Light.triggered; } }                                        // T
    public bool InteractTrigger { get { return PlayerInput.Interact.triggered; } }                                  // E
    public bool ThrowItemTrigger { get { return PlayerInput.ThrowItem.triggered; } }                                // Q
    public bool HealItemTrigger { get { return PlayerInput.HealItem.triggered; } }                                  // R

    private void UpdateInputs()
    {
        JumpPressing = PlayerInput.Jump.IsPressed();
        RollPressing = PlayerInput.Roll.IsPressed();
        GuardPressing = PlayerInput.Guard.IsPressed();
    }


    // 상태 매니저 관련
    private PlayerStateManager m_stateManager;                                                                      // 상태 관리자
    private IPlayerState CurState { get { return m_stateManager.CurState; } }                                       // 현재 상태
    private readonly IPlayerState[] m_playerStates = new IPlayerState[(int)EPlayerState.LAST];                      // 상태 클래스들

    public bool IsIdle { get { return CurState.StateEnum == EPlayerState.IDLE; } }                                  // IDLE
    public bool IsMoving { get { return CurState.StateEnum == EPlayerState.MOVE; } }                                // 이동 중
    public bool IsAttacking { get { return CurState.StateEnum == EPlayerState.ATTACK; } }                           // 공격 중
    public bool IsSkilling { get { return CurState.StateEnum == EPlayerState.SKILL; } }                             // 스킬 중
    public bool IsJumping { get { return CurState.StateEnum == EPlayerState.JUMP; } }                               // 점프 중
    public bool IsRolling { get { return CurState.StateEnum == EPlayerState.ROLL; } }                               // 구르기 중
    public bool IsThrowing { get { return CurState.StateEnum == EPlayerState.THROW; } }                             // 구르기 중

    public void ChangeState(EPlayerState _state) { m_stateManager.ChangeState(m_playerStates[(int)_state]); }       // 상태 변환


    // 조준 관련
    public Vector2 PlayerAimDirection { get { return FunctionDefine.DegToVec(PlayManager.CameraRotation); } }       // 플레이어 에임 좌우
    public float PlayerAimAngle { get { return PlayManager.CameraAngle; } }                                         // 플레이어 에임 상하
    public Vector3 PlayerAimVector { get {                                                                          // 플레이어 에임 3D
            float angleRad = (PlayerAimAngle+45) * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);
            Vector2 dir = PlayerAimDirection * cos;
            return new(dir.x, sin, dir.y); } }

    public void ShowSkillAim(float _radius, float _range)                                                           // 조준 보이기
    {
        PlayManager.ShowSkillAim(Position, _radius, _range);
    }
    public void TraceSkillAim()                                                                                     // 조준 마크 위치 설정
    {
        PlayManager.TraceSkillAim(Position, SkillInfoInHand.SkillCastRange);
    }
    public void HideSkillAim()                                                                                      // 조준 숨기기
    {
        PlayManager.HideSkillAim();
    }

    
    // 스테미나
    public float CurStamina { get; protected set; }                                                                 // 현재 스테미나
    private float StaminaRestoreRate = 1.5f;                                                                        // 초당 스테미나 회복

    private void InitStamina() { CurStamina = MaxStamina; SetStaminaRate(); }                                       // 스테미나 초기 설정
    public void UseStamina(float _use)                                                                              // 스테미나 사용
    {
        CurStamina -= _use;
        if (CurStamina < 0) { CurStamina = 0; }
        SetStaminaRate();
    }
    private void StaminaUpdate()                                                                                    // 스테미나 업데이트
    {
        if (CurStamina < MaxStamina)
        {
            CurStamina += StaminaRestoreRate * Time.deltaTime;
            if (CurStamina > MaxStamina) { CurStamina = MaxStamina; }
        }
        SetStaminaRate();
    }
    private void SetStaminaRate()                                                                                   // 스테미나 UI 설정
    {
        float rate = CurStamina / MaxStamina;
        PlayManager.SetStaminaRate(rate);
    }


    // 쿨타임
    private float[] m_cooltimes = new float[(int)ECooltimeName.LAST];
    public float JumpCooltime
    {
        get { return m_cooltimes[(int)ECooltimeName.JUMP]; }    // 점프 쿨타임
        private set { m_cooltimes[(int)ECooltimeName.JUMP] = value; }
    }
    public float RollCooltime
    {
        get { return m_cooltimes[(int)ECooltimeName.ROLL]; }
        private set { m_cooltimes[(int)ECooltimeName.ROLL] = value; }
    }
    public float HealCooltime
    {
        get { return m_cooltimes[(int)ECooltimeName.HEAL]; }
        private set { m_cooltimes[(int)ECooltimeName.HEAL] = value; }
    }
    public float[] SkillCooltime
    {
        get { return m_cooltimes[(int)ECooltimeName.SKILL1..(int)ECooltimeName.LAST]; }
        private set { for (int i = 0; i<ValueDefine.MAX_SKILL_SLOT; i++) { m_cooltimes[(int)ECooltimeName.SKILL1 + i] = value[i]; } }
    }
    public override void ProcCooltime()
    {
        for (int i = 0; i<(int)ECooltimeName.LAST; i++)
        {
            if (m_cooltimes[i] > 0) { m_cooltimes[i] -= Time.deltaTime; if (m_cooltimes[i] < 0) { m_cooltimes[i] = 0; } }
        }
        base.ProcCooltime();
    }


    // 버프 디버프
    private bool[] m_fieldDebuff = new bool[(int)EProperty.LAST];

    public void SetFieldDebuff(EProperty _property) { m_fieldDebuff[(int)_property] = true; }


    // 능력 관련
    [SerializeField]
    private PlayerLightScript m_light;                                              // 불빛 프리펍

    private const float MAX_LIGHT_POWER = 5;        // 최대 능력 스테미나
    private float LightUseRate = 0/*.5f*/;              // 초당 능력 사용
    private float LightRestoreRate = 1;             // 초당 능력 회복
    private float LightRestoreDelay = 2;            // 능력 다 썼을 때 딜레이

    private float CurLightRate { get; set; }        // 현재 능력 스테미나(?)
    private bool CanRestoreLight { get; set; } = true;      // 회복 가능
    public bool IsLightOn { get; private set; }                                 // 불빛이 켜져 있는지

    public void LightOn()               // 능력 사용
    {
        m_light.LightOn();
        IsLightOn = true;
    }
    public void LightOff()              // 능력 중단
    {
        m_light.LightOff();
        IsLightOn = false;
    }
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
    private IEnumerator LightRestoreCoolTime()      // 능력 탕진 후딜레이
    {
        yield return new WaitForSeconds(LightRestoreDelay);
        CanRestoreLight = true;
    }


    // 업데이트
    public override void Update()
    {
        base.Update();
        UpdateInputs();                     // InputSystem 상태 반영

        CurState.Proceed();                 // 현재 상태 Update

        StaminaUpdate();                    // 스테미나

        HealUpdate();                       // 회복 상태
        GuardUpdate();                      // 가드 상태
        LightUpdate();                      // 능력 상태

        PlayerDetactUpdate();               // 상호작용 물체들 관리
    }

    public override void FixedUpdate()
    {
        PrePhysicsUpdate();                 // 선처리 물리

        CurState.FixedProceed();            // 현재 상태 FixedUpdate

        LatePhysicsUpdate();                // 후처리 물리
    }
}
