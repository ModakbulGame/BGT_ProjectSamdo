using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiserableScript : HmmScript
{
    public override bool CanPurify => IsMelancholy;

    private bool IsAttackSkill { get { return AttackIdx == 3; } }
    private int SkillIdx { get; set; }
    public override void StartAttack()
    {
        if(IsAttackSkill) { SkillIdx = 0; }
        AttackIdx =  Random.Range(0, 4);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        base.StartAttack();
    }
    public override void CreateAttack()
    {
        m_rigid.velocity = 8 * transform.forward;
    }
    public override void AttackTriggerOn()
    {
        int idx;
        if (!IsAttackSkill) { idx = 0; }
        else { idx = SkillIdx % 2 + 1; SkillIdx++; }
        AttackTriggerOn(idx);
        AttackObject.SetAttack(this, Attack);
    }
    public override void AttackTriggerOff()
    {
        AttackObject.AttackOff();
    }
    public override void AttackDone()
    {
        base.AttackDone();
        if(AttackIdx <=1) { AttackTimeCount *= 0.5f; }
    }
}
