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
            if (rot < -180) { rot += 360; } else if(rot > 180) { rot -= 360; }
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
        if(TargetDistance > AttackRange) { MoveForward(); }

        if (SequenceCount++ >= MaxSequence) { NextAttack = (int)ENextAttack.NONE; }
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
    private readonly float AnySkillCooltime = 8;                // 스킬 간 최소 간격
    private float AnySkillTimeCount { get; set; }

    [SerializeField]
    private float[] m_skillDamage = new float[(int)ECrystalGuardianSkill.LAST];
    [SerializeField]
    private float[] m_skillCooltime = new float[(int)ECrystalGuardianSkill.LAST];

    public int NextSkillIdx { get; set; }
    public override int SkillNum => (int)ECrystalGuardianSkill.LAST;
    public override bool CanSkill => AnySkillTimeCount <= 0 && HasTarget && TargetInAttackRange && SkillTimeCount[NextSkillIdx] <= 0;
    public override float AttackRange => (AnySkillTimeCount <= 0) ? base.AttackRange * 2 : base.AttackRange;

    private readonly int DoubleSwingIdx = (int)ECrystalGuardianSkill.DOUBLE_SWING;
    private readonly int JumpSkillIdx = (int)ECrystalGuardianSkill.JUMP_SKILL;
    private readonly int ImpactIdx = (int)ECrystalGuardianSkill.IMPACT;

    private ObjectAttackScript CurSkill { get; set; }
    public bool CreatedSkill { get; private set; }
    private int DoubleIdx { get; set; }

    private readonly float ForwardForce = 7.5f;

    public override void StartSkill()
    {
        CurSkillIdx = NextSkillIdx;
        if (CurSkillIdx == -1) { return; }
        m_anim.SetInteger("SKILL_IDX", CurSkillIdx);
        m_anim.SetBool("IS_SKILLING", true);
        StopMove();
        CreatedSkill = false;
        if(CurSkillIdx == DoubleSwingIdx) { DoubleIdx = 0; }
    }
    public override void SkillOn()
    {
        if (CurSkillIdx == DoubleSwingIdx)
        {
            CurSkill = m_normalAttacks[DoubleIdx++].GetComponent<ObjectAttackScript>();
            CurSkill.SetAttack(this, m_skillDamage[DoubleSwingIdx]);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
        }
    }
    public override void CreateSkill()
    {
        if (CurSkillIdx == DoubleSwingIdx)
        {
            MoveForward();
        }
        else if (CurSkillIdx == JumpSkillIdx)
        {
            for (int i = 0; i<2; i++)
            {
                SkillList[i].SetAttack(this, m_skillDamage[JumpSkillIdx]);
                SkillList[i].AttackOn();
            }
            m_anim.SetBool("IS_SKILLING", false);
        }
        else if (CurSkillIdx == ImpactIdx)
        {
            SkillList[CurSkillIdx].SetAttack(this, m_skillDamage[JumpSkillIdx]);
            SkillList[CurSkillIdx].AttackOn();
            m_anim.SetBool("IS_SKILLING", false);
        }
    }
    public override void SkillOff()
    {
        if (CurSkillIdx == DoubleSwingIdx)
        {
            CurSkill.AttackOff();
            if(DoubleIdx == 2) { m_anim.SetBool("IS_SKILLING", false); }
        }
    }
    public override void SkillDone()
    {
        base.SkillDone();

        SkillTimeCount[CurSkillIdx] = SkillCooltime[CurSkillIdx];
        AnySkillTimeCount = AnySkillCooltime;

        SetNextSkill();
    }
    private void SetNextSkill()
    {
        int minIdx = 0;
        float minTime = SkillTimeCount[minIdx];
        for (int i = 1; i<SkillNum; i++)
        {
            float curTime = SkillTimeCount[i];
            if (curTime < minTime) { minIdx = i; }
            else if (curTime == minTime) { minIdx = Random.Range(0, 2) == 0 ? i : minIdx; }
        }
        NextSkillIdx = minIdx;
    }

    private void MoveForward()
    {
        m_rigid.AddForce(ForwardForce * transform.forward, ForceMode.VelocityChange);
    }


    public override void ProcCooltime()
    {
        base.ProcCooltime();
        if (AnySkillTimeCount > 0) { AnySkillTimeCount -= Time.deltaTime; }
        for (int i = 0; i<SkillNum; i++)
        {
            if (SkillTimeCount[i] > 0) { SkillTimeCount[i] -= Time.deltaTime; }
        }
    }

    public override void OnSpawned()
    {
        base.OnSpawned();
        NextSkillIdx = Random.Range(0, SkillNum);
    }
    public override void InitSkillInfo()
    {
        base.InitSkillInfo();
        SkillCooltime = new float[3] { m_skillCooltime[0], m_skillCooltime[1], m_skillCooltime[2] };
    }


    public override void SetStates()
    {
        base.SetStates();
        m_monsterStates[(int)EMonsterState.SKILL] = gameObject.AddComponent<CrystalGuardianSkillState>();
    }

    public override void SetAttackObject()
    {
        m_normalAttacks[2].GetComponent<ObjectAttackScript>().SetAttack(this, Attack);
        SkillList[0].GetComponent<ObjectAttackScript>().SetAttack(this, m_skillDamage[1]);
        SkillList[1].GetComponent<ObjectAttackScript>().SetAttack(this, m_skillDamage[1]);
    }
}
