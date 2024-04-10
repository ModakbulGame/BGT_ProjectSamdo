using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.MOVE; } }

    public void ChangeTo(PlayerController _player)
    {
        if (m_player == null) { m_player = _player; }

        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 inputDir = m_player.InputVector;
        Vector2 aimDir = m_player.PlayerAimDirection;

        m_player.MoveDirection(inputDir);           // 이동    
        m_player.SetMoveAnimation(inputDir);        // 애니    
        m_player.RotateTo(aimDir);                  // 회전
    }

    public void Proceed()
    {
        if (m_player.LightTrigger)
        {
            m_player.LightChange();
        }

        Vector2 inputDir = m_player.InputVector;
        if (m_player.CanJump)
        {
            m_player.ChangeState(EPlayerState.JUMP);
            return;
        }
        if (m_player.CanAttack)
        {
            m_player.ChangeState(EPlayerState.ATTACK);
            return;
        }
        if (m_player.CanUseSkill)
        {
            m_player.ChangeState(EPlayerState.SKILL);
            return;
        }
        if (inputDir == Vector2.zero)
        {
            m_player.ChangeState(EPlayerState.IDLE);
            return;
        }
        if (m_player.CanThrow && m_player.ThrowItemTrigger)
        {
            m_player.ReadyThrow();
            return;
        }
    }

    public void FixedProceed()
    {
        MovePlayer();
    }
}