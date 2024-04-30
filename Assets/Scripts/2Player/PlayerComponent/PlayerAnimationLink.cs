using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationLink : MonoBehaviour
{
    private PlayerController m_player;

    public void ChkAttackDone()             // 공격 애니메이션 완료
    {
        m_player.ChkAttackDone();
    }
    public void UpperAnimDone()             // 상체 애니메이션 종료
    {
        m_player.UpperAnimDone();
    }
    public void CreateThrowItem()           // 아이템 던지기 타이밍
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

    public void StartInvincible()           // 무적 시작
    {
        m_player.StartInvincible();
    }
    public void StopInvincible()            // 무적 중단
    {
        m_player.StopInvincible();
    }



    private void Awake()
    {
        m_player = GetComponentInParent<PlayerController>();
    }
}
