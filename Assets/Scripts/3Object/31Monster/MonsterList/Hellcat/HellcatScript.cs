using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EHellcatAttack
{
    JUMP_ATTACK,
    DOUBLE_SCRATCH,

    LAST
}

public class HellcatScript : MonsterScript
{
    public override bool CanPurify => !HittedPlayer;

    private bool HittedPlayer { get; set; }


    [Tooltip("점프 기준 거리")]
    [SerializeField]
    private float m_jumpAttackDist = 1.75f;
    private readonly float JumpMoveSpeed = 3.5f;

    [Tooltip("기본 공격 데미지 배율")]
    [SerializeField]
    private float[] m_normalDamageMultiplier = new float[(int)EHellcatAttack.LAST]
        { 1, 1 };


    public bool IsJumpAttack { get; private set; }
    public bool IsJumping { get; private set; }
    public bool HasJumped { get; private set; }
    private int AttackStack { get; set; } = 0;
    private Vector2 JumpDir { get; set; }

    public override void StartAttack()
    {
        IsJumpAttack = TargetDistance >= m_jumpAttackDist;
        m_anim.SetBool("JUMP_ATTACK", IsJumpAttack);
        base.StartAttack();
        AttackStack = 0;
        IsJumping = false;
        HasJumped = false;
    }

    public void JumpAttackMove()
    {
        if (!IsJumping) { return; }
        Vector3 dir = JumpMoveSpeed * new Vector3(JumpDir.x, 0, JumpDir.y);
        m_rigid.velocity = dir;
    }

    public void StartJump()
    {
        IsJumping = true;
        HasJumped = true;
        if (CurTarget == null) { JumpDir = Vector2.zero; return; }
        JumpDir = (CurTarget.Position2 - Position2).normalized;

        CurClawEffect = null;
        foreach (CombinedEffect effect in m_clawEffects) { effect.EffectOn(); }
    }
    public void StopJump()
    {
        IsJumping = false;
        m_rigid.velocity = Vector3.zero;
    }


    [SerializeField]
    private CombinedEffect[] m_clawEffects;
    private CombinedEffect CurClawEffect { get; set; }

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

        int damageIdx = _idx == 0 ? 0 : 1;
        float damage = Attack * m_normalDamageMultiplier[damageIdx];

        AttackObject.SetAttack(this, damage);
        AttackObject.AttackOn();

        if (_idx <= 2)
        {
            if (_idx == 1) { CurClawEffect.EffectOff(); }

            CurClawEffect = m_clawEffects[_idx];
            CurClawEffect.EffectOn();
        }
        else { foreach (CombinedEffect effect in m_clawEffects) { effect.EffectOff(); } }
    }
    private void CreateJumpAttack()
    {
        AttackTriggerOn(2);
    }
    public override void AttackDone()
    {
        if (CurClawEffect != null) { CurClawEffect.EffectOff(); }
        base.AttackDone();
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
