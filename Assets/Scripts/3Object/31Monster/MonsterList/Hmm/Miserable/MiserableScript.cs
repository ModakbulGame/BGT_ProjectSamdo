using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiserableScript : HmmScript
{
    private int CurAttackIdx { get; set; }
    private bool IsAttackSkill { get { return CurAttackIdx == 3; } }
    private int SkillIdx { get; set; }
    public override void StartAttack()
    {
        CurAttackIdx = Random.Range(0, 2);
        if(IsAttackSkill) { SkillIdx = 0; }
        m_anim.SetInteger("ATTACK_IDX", CurAttackIdx);
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
}
