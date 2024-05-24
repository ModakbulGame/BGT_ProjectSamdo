using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarvedScript : HmmScript
{
    private int CurAttackIdx { get; set; }
    public override void StartAttack()
    {
        CurAttackIdx = Random.Range(0, 3);
        m_anim.SetInteger("ATTACK_IDX", CurAttackIdx);
        base.StartAttack();
    }
    public override void AttackTriggerOn()
    {
        int idx = CurAttackIdx < 2 ? 0 : 1;
        AttackTriggerOn(idx);
        AttackObject.SetAttack(this, Attack);
    }
    public override void AttackTriggerOff()
    {
        AttackObject.AttackOff();
    }
}
