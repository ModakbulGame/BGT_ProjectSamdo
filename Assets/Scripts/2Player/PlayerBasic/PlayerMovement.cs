using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    // ������ ���� ��ġ
    private const float JUMP_POWER = 8;                 // ���� �Ŀ�
    private const float JUMP_DELAY = 0.15f;             // ���� ����
    private const float ROLL_SPEED = 12;                // ������ �̵� �ӵ�
    public const float TIME_TO_ROLL = 0.1f;             // ���� -> ������� �ٲ�� ���� �����̽� ���� �ð�
    public const float ROLLING_TIME = 0.85f;            // ������ ���� �ð�


    // ������ �Լ�
    private Vector2 GetMoveVector(Vector2 _dir) { return FunctionDefine.RotateVector2(_dir, PlayManager.CameraRotation); }      // ���ӿ� ���� ���� ȸ��
    public void MoveDirection(Vector2 _dir)                     // �������� �̵�
    {
        MoveDirection(_dir, 1);
    }
    public void MoveDirection(Vector2 _dir, float _mul)         // �������� �̵� + �ӵ� ����
    {
        Vector2 move = GetMoveVector(_dir) * _mul;
        Vector3 move3 = new(move.x, 0, move.y);
        //transform.position += CurSpeed * Time.deltaTime * new Vector3(move.x, 0, move.y);
        if (IsOnSlope) { move3 = AdjSlopeMoveDir(move3); }
        MoveTo(move3);
    }
    public override void StopMove()                                      // �̵� ����
    {
        MoveTo(Vector3.zero);
    }
    public void RotateDirection(Vector2 _dir)                   // �������� ȸ��
    {
        Vector2 move = GetMoveVector(_dir);
        RotateTo(move);
    }

    public const float JumpStaminaUse = 1.5f;
    public const float RollStaminaUse = 2.5f;
    public Vector2 JumpRollDirection { get; set; }              // ���� or ������ ����
    public bool CanJump { get { return (IsGrounded || IsOnSlope) && JumpCoolTime <= 0 && JumpPressing && CurStamina >= JumpStaminaUse; } }  // ���� ���� ����
    public bool CanRoll { get { return CurStamina >= RollStaminaUse; } }
    public bool CanThrow { get { return IsUpperIdleAnim; } }
    public bool Jumped { get; private set; }                    // ���� ������
    public void JumpStarted()                                   // ���� ���� ����
    {
        Jumped = false;
        ResetAnim();
        JumpAnim();
    }
    public void JumpAction()                                    // ������ ���� ������ ���� �� ���� �ൿ
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
    public void RollMovement(Vector2 _dir)                      // ������ ������
    {
        Vector2 move = GetMoveVector(_dir);
        Vector3 rollMove = new Vector3(move.x, 0, move.y);
        if (IsOnSlope) { rollMove = AdjSlopeMoveDir(rollMove); }
        m_rigid.velocity = Vector3.SmoothDamp(m_rigid.velocity, ROLL_SPEED * rollMove, ref velocityRef, 0.5f);
        //transform.position += RollSpeed * Time.deltaTime * new Vector3(move.x, 0, move.y);
    }

    public void JumpRollDone()                                  // ���� or ������ �ߴ�
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
