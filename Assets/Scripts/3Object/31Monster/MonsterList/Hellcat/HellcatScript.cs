using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellcatScript : MonsterScript
{
    private const float JumpAttackDist = 1.75f;
    private const float JumpMoveSpeed = 3.5f;

    [SerializeField]
    private GameObject m_jumpAttackPrefab;

    private readonly Vector3 Attack1Offset = new(0.016f, 0.835f, 0.881f);
    private readonly Vector3 Attack2Offset = new(-0.016f, 0.835f, 0.881f);
    
    private readonly Vector3 JumpAttackOffset = new(0, 0, 1.8f);

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
                CreateNormalAttack(Attack1Offset, Attack / 2);
                AttackStack = 1;
            }
            else
            {
                CreateNormalAttack(Attack2Offset, Attack / 2);
            }
        }
    }
    private void CreateJumpAttack()
    {
        GameObject attack = Instantiate(m_jumpAttackPrefab, transform);
        attack.transform.localPosition = JumpAttackOffset;
        NormalAttackScript script = attack.GetComponent<NormalAttackScript>();
        script.SetAttack(this, Attack);
    }


    public override void SetStates()
    {
        base.SetStates();
        ReplaceState(EMonsterState.ATTACK, gameObject.AddComponent<HellcatAttackState>());
    }
}
