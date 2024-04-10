using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlokanScript : BloScript
{
    private int AttackIdx { get; set; }

    private Vector3[] NormalAttackOffsets = new Vector3[2]
        { new(0,1.5f,1.3f), new(0,1,1.3f) };

    public override void AttackAnimation()
    {
        AttackIdx = Random.Range(0, 2);
        m_anim.SetInteger("RANDOM_IDX", AttackIdx);
        base.AttackAnimation();
    }

    public override void CreateAttack()
    {
        CreateNormalAttack(NormalAttackOffsets[AttackIdx]);
    }
}
