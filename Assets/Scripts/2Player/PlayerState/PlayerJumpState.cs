using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.JUMP; } }

    private Vector2 JumpDirection { get { return m_player.JumpRollDirection; } set { m_player.JumpRollDirection = value; } }         // ���� ����
    private const float MoveAdjustRate = 0.33f;         // ���� ���� ����

    public void ChangeTo(PlayerController _player)
    {
        if (m_player == null) { m_player = _player; }

        m_player.JumpAction();
        if (m_player.IsGuarding) { m_player.GuardStop(); }
        JumpDirection = m_player.InputVector;       // ���� ���� ����
    }

    private void JumpMove()                         // ���� ���� ���� + ���� ����Ű �Է¿� ���� ���� �̵�
    {
        Vector2 move = JumpDirection;
        if (m_player.InputVector != Vector2.zero)
        {
            move += m_player.InputVector * MoveAdjustRate;
            if (move.magnitude > JumpDirection.magnitude)
                move = JumpDirection;
        }
        m_player.MoveDirection(move);
    }

    public void Proceed()
    {
        if (m_player.IsGrounded)
        {
            m_player.JumpDone();
            return;
        }
    }

    public void FixedProceed()
    {
        JumpMove();
    }
}