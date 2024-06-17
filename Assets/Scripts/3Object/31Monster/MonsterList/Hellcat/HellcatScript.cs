using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellcatScript : MonsterScript
{
    public override bool CanPurify => !HittedPlayer;

    private bool HittedPlayer { get; set; }


    private const float JumpAttackDist = 1.75f;
    private const float JumpMoveSpeed = 3.5f;


    public bool IsJumpAttack { get; private set; }
    public bool IsJumping { get; private set; }
    public bool HasJumped { get; private set; }
    private int AttackStack { get; set; } = 0;
    private Vector2 JumpDir { get; set; }

    public override void StartAttack()
    {
        IsJumpAttack = TargetDistance >= JumpAttackDist;
        m_anim.SetBool("JUMP_ATTACK", IsJumpAttack);
        base.StartAttack();
        AttackStack = 0;
        IsJumping = false;
        HasJumped = false;
    }

    public void JumpAttackMove()
    {
        if(!IsJumping) { return; }
        Vector3 dir = JumpMoveSpeed * new Vector3(JumpDir.x, 0, JumpDir.y);
        m_rigid.velocity = dir;
    }

    public void StartJump()
    {
        IsJumping = true;
        HasJumped = true;
        if(CurTarget == null) { JumpDir = Vector2.zero; return; }
        JumpDir = (CurTarget.Position2 - Position2).normalized;
    }
    public void StopJump()
    {
        IsJumping = false;
        m_rigid.velocity = Vector3.zero;
    }

    public override void CreateAttack()
    {
        if (IsJumpAttack) 
        {
            CreateJumpAttack();
        }
        else
        {
            if (AttackStack == 0)
            {
                AttackTriggerOn(0);
                AttackStack = 1;
            }
            else
            {
                AttackTriggerOn(1);
            }
        }
    }
    public override void AttackTriggerOn(int _idx)
    {
        m_normalAttacks[_idx].SetActive(true);
        AttackObject = m_normalAttacks[_idx].GetComponent<NormalAttackScript>();
        AttackObject.SetAttack(this, Attack);
        AttackObject.SetDamage(Attack);
        AttackObject.AttackOn();
    }
    public override void AttackTriggerOff()
    {
        if (!AttackObject) return;
        AttackObject.AttackOff();
        AttackObject.gameObject.SetActive(false);
    }
    private void CreateJumpAttack()
    {
        AttackTriggerOn(2);
    }


    public override void AttackedPlayer(HitData _hit)
    {
        base.AttackedPlayer(_hit);
        HittedPlayer = true;
    }
    public override void OnSpawned()
    {
        base.OnSpawned();
        HittedPlayer = false;
    }

    public override void SetStates()
    {
        base.SetStates();
        ReplaceState(EMonsterState.ATTACK, gameObject.AddComponent<HellcatAttackState>());
    }
}
