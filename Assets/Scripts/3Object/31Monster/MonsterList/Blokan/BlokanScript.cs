using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlokanScript : BloScript
{
    public override bool CanPurify => AbsorbedSoul;

    public override int AbsorbAmount => 5;


    private readonly Vector3[] NormalAttackOffsets = new Vector3[2]
        { new(0,1.5f,1.3f), new(0,1,1.3f) };

    public override void AttackAnimation()
    {
        AttackIdx = Random.Range(0, 2);
        m_anim.SetInteger("RANDOM_IDX", AttackIdx);
        base.AttackAnimation();
    }

    public override void CreateAttack()
    {
        base.CreateAttack();
        m_normalAttacks[0].transform.localPosition = NormalAttackOffsets[AttackIdx];
    }
}
