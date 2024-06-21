using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELifeGuardianSkill
{
    SPIKE,
    DRAIN,
    RUSH,

    LAST
}

public class LifeGuardianScript : AnimatedAttackMonster
{
    private bool AttackProceed { get; set; }
    private readonly float[] AttackAngle = new float[4] { 0, 30, -30, -15 };


    public override void LookTarget()
    {
        if (CurTarget == null) { return; }

        Vector2 dir = (CurTarget.Position2 - Position2).normalized;
        if (IsTracing)
        {
            float rot = FunctionDefine.VecToDeg(dir);
            rot += AttackAngle[AttackIdx];
            if(rot < 0) { rot += 360; }
            dir = FunctionDefine.DegToVec(rot);
        }
        RotateToDir(dir, ERotateSpeed.SLOW);
    }

    public override void StartAttack()
    {
        AttackIdx = Random.Range(0, 4);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        base.StartAttack();
    }
    public override void AttackTriggerOn()
    {
        base.AttackTriggerOn();
        AttackObject.SetDamage(Attack);

        AttackProceed = AttackIdx < 2 && Random.Range(0, 2) == 0;
        m_anim.SetBool("PROCEED_ATTACK", AttackProceed);
    }
    public override void AttackDone()
    {
        IsTracing = false;
        if (AttackIdx >= 2 || !AttackProceed)
        {
            base.AttackDone();
        }
        else { AttackIdx++; }
    }


    // ½ºÅ³
    private readonly float AnySkillCooltime = 8;
    private float AnySkillTimeCount { get; set; }

    [SerializeField]
    private float[] m_skillDamage = new float[3];
    [SerializeField]
    private float[] m_skillCooltime = new float[3];


    private int NextSkillIdx { get; set; }

    public override int SkillNum => (int)ELifeGuardianSkill.LAST;
    public override bool CanSkill => AnySkillTimeCount <= 0 && HasTarget && TargetInAttackRange && SkillTimeCount[NextSkillIdx] <= 0;


    private readonly int SpikeIdx = (int)ELifeGuardianSkill.SPIKE;
    private readonly int DrainIdx = (int)ELifeGuardianSkill.DRAIN;
    private readonly int RushIdx = (int)ELifeGuardianSkill.RUSH;


    private ObjectAttackScript CurSkill { get; set; }
    public bool CreatedSkill { get; private set; }
    public bool RushStarted { get; private set; }

    private readonly float RushSpeed = 8;

    public override void StartSkill()
    {
        CurSkillIdx = NextSkillIdx;
        if(CurSkillIdx == -1) { return; }
        m_anim.SetInteger("SKILL_IDX", CurSkillIdx);
        m_anim.SetBool("IS_SKILLING", true);
        StopMove();
        CreatedSkill = false;
    }
    public override void SkillOn()
    {
        if (CurSkillIdx == SpikeIdx)
        {
            CurSkill = SkillList[SpikeIdx];
            CurSkill.SetAttack(this, m_skillDamage[SpikeIdx]);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
        }
        else if (CurSkillIdx == DrainIdx)
        {
            CurSkill = SkillList[DrainIdx];
            CurSkill.SetAttack(this, m_skillDamage[DrainIdx]);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
        }
        else if (CurSkillIdx == RushIdx)
        {
            CurSkill = SkillList[RushIdx];
            CurSkill.SetAttack(this, m_skillDamage[RushIdx]);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
            RushStarted = true;
        }
        CreatedSkill = true;
    }
    public override void SkillOff()
    {
        if (CurSkillIdx == SpikeIdx)
        {
            CurSkill.AttackOff();
            CurSkill.gameObject.SetActive(false);
        }
        else if (CurSkillIdx == DrainIdx)
        {
            CurSkill.AttackOff();
            CurSkill.gameObject.SetActive(false);
        }
        else if (CurSkillIdx == RushIdx)
        {
            CurSkill.AttackOff();
            CurSkill.gameObject.SetActive(false);
            RushStarted = false;
        }
        m_anim.SetBool("IS_SKILLING", false);
    }
    public void RushForward()
    {
        if (!CreatedSkill)
        {
            Vector2 dir = (CurTarget.Position2-Position2).normalized;
            RotateToDir(dir, ERotateSpeed.DEFAULT);
        }

        m_rigid.velocity = RushSpeed * transform.forward;
    }
    public override void SkillDone()
    {
        SkillTimeCount[CurSkillIdx] = SkillCooltime[CurSkillIdx];
        AnySkillTimeCount = AnySkillCooltime;

        if (CurSkillIdx == DrainIdx)
        {
            ChangeState(EMonsterState.ATTACK);
        }
        else
        {
            base.SkillDone();
        }

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



    public override void ProcCooltime()
    {
        base.ProcCooltime();
        if(AnySkillTimeCount > 0) { AnySkillTimeCount -= Time.deltaTime; }
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
        m_monsterStates[(int)EMonsterState.SKILL] = gameObject.AddComponent<LifeGuardianSkillState>();
    }
}