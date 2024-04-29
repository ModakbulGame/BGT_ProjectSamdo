using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    // 움직임 관련 수치
    private const float JUMP_POWER = 11;                // 점프 파워
    private const float JUMP_DELAY = 0.15f;             // 점프 간격
    private const float ROLL_DELAY = 0.25f;             // 구르기 간격
    public const float ROLL_MULTIPLIER = 10/7f;         // 구르기 / 걷기 속도 배율
    public const float ROLLING_TIME = 0.85f;            // 구르기 진행 시간


    // 움직임 함수
    private Vector2 GetMoveVector(Vector2 _dir) { return FunctionDefine.RotateVector2(_dir, PlayManager.CameraRotation); }      // 에임에 맞춰 벡터 회전
    public void MoveDirection(Vector2 _dir)                     // 방향으로 이동
    {
        MoveDirection(_dir, 1);
    }
    public void GroundMove(Vector2 _dir, float _mul)
    {
        if (_dir != Vector2.zero)
        {
            Vector2 adjDir = GetMoveVector(_dir);
            TargetAngle = FunctionDefine.VecToDeg(adjDir);
            Vector3 move = new(adjDir.x, PlayerForward.y, adjDir.y);
            MoveDirection(move, _mul);
        }
    }
    public void MoveDirection(Vector2 _dir, float _mul)         // 방향으로 이동 + 속도 배율
    {
        Vector2 move = GetMoveVector(_dir) * _mul;
        Vector3 move3 = new(move.x, 0, move.y);
        MoveTo(move3);
    }
    public void MoveDirection(Vector3 _dir, float _mul)
    {
        Vector3 move3 = _mul * _dir;
        if(!IsGrounded) { move3.y = 0; }
        MoveTo(move3);
    }
    public override void StopMove()                             // 이동 중지
    {
        m_rigid.velocity = Vector3.zero;
    }
    public void RotateDirection(Vector2 _dir)                   // 방향으로 회전
    {
        Vector2 move = GetMoveVector(_dir);
        RotateTo(move);
    }
    public override void MoveTo(Vector3 _dir)
    {
        Vector3 vel = m_rigid.velocity;
        base.MoveTo(_dir);
        Vector3 vel2 = m_rigid.velocity;
        if(!IsGrounded && !IsOnSlope) { vel2.y = vel.y; m_rigid.velocity = vel2; }
    }
    public void ForceMove(Vector2 _dir)
    {
        transform.position += Time.deltaTime * new Vector3(_dir.x, 0, _dir.y);
    }


    public const float JumpStaminaUse = 1.5f;
    public const float RollStaminaUse = 2.5f;
    public Vector2 JumpRollDirection { get; set; }              // 점프 or 구르기 방향
    public bool CanJump { get { return IsGrounded && ((IsOnSlope && CurSurfaceAngle <= m_maxSlopeAngle) || !IsOnSlope) && !IsTouchingWall &&
                JumpCoolTime <= 0 && JumpPressing && CurStamina >= JumpStaminaUse; } }  // 점프 가능 여부
    public bool CanRoll { get { return (IsGrounded || IsOnSlope) && RollCoolTime <= 0 && RollPressing && CurStamina >= RollStaminaUse; } }
    public bool CanThrow { get { return IsUpperIdleAnim; } }
    public void JumpAction()                                   // 점프 상태 시작
    {
        ResetAnim();
        JumpAnim();
        m_rigid.velocity += JUMP_POWER * Vector3.up;
        UseStamina(JumpStaminaUse);
    }
    public void JumpDone()
    {
        JumpRollDone();
        JumpCoolTime = JUMP_DELAY;
    }
    public void RollAction()
    {
        ResetAnim();
        RollAnim();
        UseStamina(RollStaminaUse);
    }
    public void RollDone()
    {
        JumpRollDone();
        RollCoolTime = ROLL_DELAY;
    }

    private void JumpRollDone()                                  // 점프 or 구르기 중단
    {
        if (InputVector != Vector2.zero)
        {
            ChangeState(EPlayerState.MOVE);
        }
        else
        {
            ChangeState(EPlayerState.IDLE);
        }
    }


    public void StartFall()
    {
        FallAnim();
    }


    private const float LAND_VELOCITY = -15;
    private const float FALL_DAMAGE_VELOCITY = -21.6f;
    private const float FALL_DEATH_VELOCITY = -44;
    public void PlayerLand(float _velocity)
    {
        if (_velocity <= FALL_DEATH_VELOCITY) { GetDamage(MaxHP); }
        else if (_velocity <= FALL_DAMAGE_VELOCITY) 
        {
            LandAnim();
            float ratio = 0.125f + ((FALL_DAMAGE_VELOCITY - _velocity) / (FALL_DAMAGE_VELOCITY - FALL_DEATH_VELOCITY)) * 7 / 8f;
            float damage = MaxHP * Mathf.Round(ratio * 1000) * 0.001f;
            GetDamage(damage);
        }
        else if (_velocity <= LAND_VELOCITY) { LandAnim(); }
    }




    public void TeleportPlayer(Vector3 _pos)
    {
        transform.position = _pos;
    }
}
