using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    // 움직임 관련 수치
    private const float JUMP_POWER = 8;                 // 점프 파워
    private const float JUMP_DELAY = 0.15f;             // 점프 간격
    private const float ROLL_SPEED = 12;                // 구르기 이동 속도
    public const float TIME_TO_ROLL = 0.1f;             // 점프 -> 구르기로 바뀌기 위해 스페이스 유지 시간
    public const float ROLLING_TIME = 0.85f;            // 구르기 진행 시간


    // 움직임 함수
    private Vector2 GetMoveVector(Vector2 _dir) { return FunctionDefine.RotateVector2(_dir, PlayManager.CameraRotation); }      // 에임에 맞춰 벡터 회전
    public void MoveDirection(Vector2 _dir)                     // 방향으로 이동
    {
        MoveDirection(_dir, 1);
    }
    public void MoveDirection(Vector2 _dir, float _mul)         // 방향으로 이동 + 속도 배율
    {
        Vector2 move = GetMoveVector(_dir) * _mul;
        Vector3 move3 = new(move.x, 0, move.y);
        //transform.position += CurSpeed * Time.deltaTime * new Vector3(move.x, 0, move.y);
        if (IsOnSlope) { move3 = AdjSlopeMoveDir(move3); }
        MoveTo(move3);
    }
    public override void StopMove()                                      // 이동 중지
    {
        MoveTo(Vector3.zero);
    }
    public void RotateDirection(Vector2 _dir)                   // 방향으로 회전
    {
        Vector2 move = GetMoveVector(_dir);
        RotateTo(move);
    }

    public const float JumpStaminaUse = 1.5f;
    public const float RollStaminaUse = 2.5f;
    public Vector2 JumpRollDirection { get; set; }              // 점프 or 구르기 방향
    public bool CanJump { get { return (IsGrounded || IsOnSlope) && JumpCoolTime <= 0 && JumpPressing && CurStamina >= JumpStaminaUse; } }  // 점프 가능 여부
    public bool CanRoll { get { return CurStamina >= RollStaminaUse; } }
    public bool CanThrow { get { return IsUpperIdleAnim; } }
    public bool Jumped { get; private set; }                    // 점프 중인지
    public void JumpStarted()                                   // 점프 상태 시작
    {
        Jumped = false;
        ResetAnim();
        JumpAnim();
    }
    public void JumpAction()                                    // 구르지 않은 것으로 판정 시 점프 행동
    {
        m_rigid.velocity = Vector3.up * JUMP_POWER;
        Jumped = true;
        UseStamina(JumpStaminaUse);
    }
    public void RollAction()
    {
        RollAnim();
        UseStamina(RollStaminaUse);
    }
    public void RollMovement(Vector2 _dir)                      // 구르기 움직임
    {
        Vector2 move = GetMoveVector(_dir);
        Vector3 rollMove = new Vector3(move.x, 0, move.y);
        if (IsOnSlope) { rollMove = AdjSlopeMoveDir(rollMove); }
        m_rigid.velocity = Vector3.SmoothDamp(m_rigid.velocity, ROLL_SPEED * rollMove, ref velocityRef, 0.5f);
        //transform.position += RollSpeed * Time.deltaTime * new Vector3(move.x, 0, move.y);
    }

    public void JumpRollDone()                                  // 점프 or 구르기 중단
    {
        if (InputVector != Vector2.zero)
        {
            ChangeState(EPlayerState.MOVE);
        }
        else
        {
            ChangeState(EPlayerState.IDLE);
        }
        JumpCoolTime = JUMP_DELAY;
    }


    public void StartFall()
    {
        FallAnim();
    }


    public void TeleportPlayer(Vector3 _pos)
    {
        transform.position = _pos;
    }
}
