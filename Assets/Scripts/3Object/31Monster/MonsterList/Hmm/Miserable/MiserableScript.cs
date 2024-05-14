using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiserableScript : HmmScript
{
    private int CurAttackIdx { get; set; }
    private bool IsAttackSkill { get { return CurAttackIdx == 2; } }
    private int SkillIdx { get; set; }
    public override void StartAttack()
    {
        CurAttackIdx = Random.Range(0, 3);
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
        int idx = CurAttackIdx;
        if (IsAttackSkill) { idx = SkillIdx % 2; }
        AttackTriggerOn(idx);
        AttackObject.SetAttack(this, Attack);
    }
    public override void AttackTriggerOff()
    {
        AttackObject.AttackOff();
    }
}
