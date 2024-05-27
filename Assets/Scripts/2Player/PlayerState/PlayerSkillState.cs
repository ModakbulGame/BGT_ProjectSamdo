using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.SKILL; } }

    private bool IsReady { get; set; }

    public void ChangeTo(PlayerController _player)
    {
        if(m_player == null) { m_player = _player; }

        IsReady = true;
        m_player.ReadySkill();
    }


    private void DrawSkillAim()
    {
        m_player.TraceSkillAim();
    }

    private void SkillMove()
    {
        Vector2 inputDir = m_player.InputVector;
        Vector2 aimDir = m_player.PlayerAimDirection;
        m_player.MoveDirection(inputDir, 0.8f);                 // �̵�
        m_player.SetMoveAnimation(inputDir);                    // �ִϸ����� ����
        m_player.RotateTo(aimDir);                              // ȸ��
    }


    public void Proceed()
    {
        if (IsReady)            // �غ� ������ ��
        {
            if (m_player.AttackTrigger)
            {
                m_player.FireSkill();
                IsReady = false;
                return;
            }
            if (m_player.GuardTrigger)
            {
                m_player.CancelSkill();
                return;
            }
        }
    }

    public void FixedProceed()
    {
        if (IsReady)
        {
            SkillMove();
            DrawSkillAim();
            if (m_player.IsRaycastSkill) { m_player.CheckRaycast(); }
        }
    }
}
