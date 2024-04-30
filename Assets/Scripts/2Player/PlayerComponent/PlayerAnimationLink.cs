using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationLink : MonoBehaviour
{
    private PlayerController m_player;

    public void ChkAttackDone()             // ���� �ִϸ��̼� �Ϸ�
    {
        m_player.ChkAttackDone();
    }
    public void UpperAnimDone()             // ��ü �ִϸ��̼� ����
    {
        m_player.UpperAnimDone();
    }
    public void CreateThrowItem()           // ������ ������ Ÿ�̹�
    {
        m_player.CreateThrowItem();
    }

    public void LandingDone()
    {
        m_player.ChangeState(EPlayerState.IDLE);
    }

    public void CreateSkill()
    {
        m_player.CreateSkill();
    }
    public void SkillDone()
    {
        m_player.SkillDone();
    }

    public void StartInvincible()           // ���� ����
    {
        m_player.StartInvincible();
    }
    public void StopInvincible()            // ���� �ߴ�
    {
        m_player.StopInvincible();
    }



    private void Awake()
    {
        m_player = GetComponentInParent<PlayerController>();
    }
}
