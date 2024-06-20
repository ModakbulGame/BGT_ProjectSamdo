using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECrystalGuardianAttack
{
    LEFT_SWING,
    RIGHT_SWING,
    LEFT_SPIKE,
    RIGHT_SPIKE,
    JUMP_ATTACK,

    LAST
}

public enum ECrystalGuardianSkill
{
    DOUBLE_SWING,
    JUMP_SKILL,
    IMPACT,

    LAST
}

public class CrystalGuardianScript : MonsterScript
{
    public enum ENextAttack { SWING, SPIKE, NONE };
    private int NextAttack { get; set; }

    public override void StartAttack()
    {
        AttackIdx = Random.Range(0, (int)ECrystalGuardianAttack.LAST);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        base.StartAttack();
    }
    public override void AttackTriggerOn()
    {
        base.AttackTriggerOn();

        int objIdx = AttackIdx % 2;
        AttackObject = m_normalAttacks[objIdx].GetComponent<AnimateAttackScript>();

        AttackObject.SetDamage(Attack);

        NextAttack = Random.Range(0, (int)ENextAttack.NONE + 1);
        m_anim.SetInteger("PROCEED_IDX", NextAttack);
    }
    public override void CreateAttack()
    {
        if (AttackIdx == (int)ECrystalGuardianAttack.JUMP_ATTACK)
        {
            AttackObject = m_normalAttacks[2].GetComponent<NormalAttackScript>();

            AttackObject.SetDamage(Attack);
            AttackObject.AttackOn();
        }
    }
    public override void AttackDone()
    {
        IsTracing = false;
        if (AttackIdx == (int)ECrystalGuardianAttack.JUMP_ATTACK || !CanAttack || NextAttack  >= (int)ENextAttack.NONE)
        {
            base.AttackDone();
            return;
        }
    }


    public override void SetAttackObject()
    {
        m_normalAttacks[2].GetComponent<ObjectAttackScript>().SetAttack(this, Attack);

    }
}
