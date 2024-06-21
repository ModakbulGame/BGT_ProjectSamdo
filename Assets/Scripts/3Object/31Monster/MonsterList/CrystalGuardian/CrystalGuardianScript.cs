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
    // 공격
    public enum ENextAttack { SWING, SPIKE, NONE };
    private int NextAttack { get; set; }

    private readonly int MaxSequence = 5;
    private int SequenceCount { get; set; } = 0;
    private bool IsSequencing { get; set; }

    private readonly float[] AttackAngle = new float[(int)ECrystalGuardianAttack.LAST] { 0, 0, 22, -22, 0 };
    private readonly float[] SkillAngle = new float[(int)ECrystalGuardianSkill.LAST] { 0, 0, 0 };

    public override void LookTarget()
    {
        if (CurTarget == null) { return; }

        Vector2 dir = (CurTarget.Position2 - Position2).normalized;
        if (IsTracing)
        {
            float rot = FunctionDefine.VecToDeg(dir);
            if (IsSkilling) { rot += SkillAngle[CurSkillIdx]; }
            else { rot += AttackAngle[AttackIdx]; }
            if (rot < 0) { rot += 360; }
            dir = FunctionDefine.DegToVec(rot);
        }
        RotateToDir(dir, ERotateSpeed.SLOW);
    }

    public override void StartAttack()
    {
        AttackIdx = Random.Range(0, (int)ECrystalGuardianAttack.LAST);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        SequenceCount = 0;
        base.StartAttack();
    }
    public override void AttackTriggerOn()
    {
        int objIdx = AttackIdx % 2;
        AttackObject = m_normalAttacks[objIdx].GetComponent<AnimateAttackScript>();

        AttackObject.SetDamage(Attack);
        AttackObject.AttackOn();

        if (SequenceCount >= MaxSequence) { NextAttack = (int)ENextAttack.NONE; }
        else { NextAttack = Random.Range(0, (int)ENextAttack.NONE + 1); }

        m_anim.SetInteger("PROCEED_IDX", NextAttack);
        IsSequencing = NextAttack < (int)ENextAttack.NONE;
    }
    public override void AttackTriggerOff()
    {
        AttackObject.AttackOff();
        AttackIdx = AttackIdx % 2 == 0 ? NextAttack * 2 + 1 : NextAttack * 2;
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
        if (IsSequencing) { return; }
        base.AttackDone();
        IsTracing = false;
    }


    // 스킬










    public override void SetAttackObject()
    {
        m_normalAttacks[2].GetComponent<ObjectAttackScript>().SetAttack(this, Attack);
    }
}
