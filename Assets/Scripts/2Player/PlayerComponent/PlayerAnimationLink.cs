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
    public void GuardDoneTiming()
    {
        m_player.GuardDone();
    }
    public void CreateThrowItem()           // ������ ������ Ÿ�̹�
    {
        m_player.CreateThrowItem();
    }
    public void ThrowDoneTiming()
    {
        m_player.DoneThrow();
    }

    public void HealTiming()
    {
        m_player.UseHealItem();
    }
    public void HealDone()
    {
        m_player.HealDone();
    }

    public void LandingDone()
    {
        m_player.ChangeState(EPlayerState.IDLE);
    }

    public void CreatePower()
    {
        m_player.CreatePower();
    }
    public void PowerDone()
    {
        m_player.PowerDone();
    }

    public void PowerTrailOn()
    {
        m_player.PowerTrailOn();
    }
    public void PowerTrailOff()
    {
        m_player.PowerTrailOff();
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
