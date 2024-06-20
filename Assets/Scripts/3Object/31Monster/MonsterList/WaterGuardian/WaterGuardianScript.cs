using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWaterGuardianSkill
{
    SLASH,
    DASH,
    ICE,

    LAST
}

public class WaterGuardianScript : AnimatedAttackMonster
{
    public override void SetDestination(Vector3 _destination)
    {
        base.SetDestination(_destination);
        m_anim.SetBool("IS_MOVING", true);
    }
    public override void StopMove()
    {
        base.StopMove();
        m_anim.SetBool("IS_MOVING", false);
    }


    private bool AttackProceed { get; set; }
    private readonly float[] AttackAngle = new float[4] { 15, -15, 0, 75 };
    private readonly float SkillAngle = 25;


    public override void LookTarget()
    {
        if (CurTarget == null) { return; }

        Vector2 dir = (CurTarget.Position2 - Position2).normalized;
        if (IsTracing)
        {
            float rot = FunctionDefine.VecToDeg(dir);
            if (IsSkilling) { rot += SkillAngle; }
            else { rot += AttackAngle[AttackIdx]; }
            if (rot < 0) { rot += 360; }
            dir = FunctionDefine.DegToVec(rot);
        }
        RotateToDir(dir, ERotateSpeed.SLOW);
    }

    public override void StartAttack()
    {
        StopMove();
        AttackAnimation();
        AttackIdx = Random.Range(0, 4);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
    }
    public override void AttackTriggerOn()
    {
        foreach (GameObject attack in m_normalAttacks)
        {
            AnimateAttackScript script = attack.GetComponent<AnimateAttackScript>();
            script.AttackOn();
            script.SetDamage(Attack);
        }

        AttackObject.SetDamage(Attack);

        AttackProceed = AttackIdx < 2 && Random.Range(0, 2) == 0;
        m_anim.SetBool("PROCEED_ATTACK", AttackProceed);
        if (AttackIdx == 2)
        {
            Vector3 dir = (CurTarget.Position-Position).normalized;
            m_rigid.AddForce(15 * dir);
        }
    }
    public override void AttackTriggerOff()
    {
        foreach (GameObject attack in m_normalAttacks)
        {
            AnimateAttackScript script = attack.GetComponent<AnimateAttackScript>();
            script.AttackOff();
        }
    }
    public override void AttackDone()
    {
        IsTracing = false;
        if (!AttackProceed)
        {
            base.AttackDone();
        }
        else { AttackIdx++; }
    }



    // 스킬
    private readonly float AnySkillCooltime = 8;                // 스킬 간 최소 간격
    private float AnySkillTimeCount { get; set; }

    [SerializeField]
    private float[] m_skillDamage = new float[3];
    [SerializeField]
    private float[] m_skillCooltime = new float[3];

    public int NextSkillIdx { get; set; }
    public override int SkillNum => (int)EWaterGuardianSkill.LAST;
    public override bool CanSkill => AnySkillTimeCount <= 0 && HasTarget && TargetInAttackRange && SkillTimeCount[NextSkillIdx] <= 0;
    public override float AttackRange => (NextSkillIdx == IceIdx && AnySkillTimeCount <= 0 && SkillTimeCount[IceIdx] <= 0) ? base.AttackRange * 4 : base.AttackRange;

    private readonly int SlashIdx = (int)EWaterGuardianSkill.SLASH;
    private readonly int RushIdx = (int)EWaterGuardianSkill.DASH;
    private readonly int IceIdx = (int)EWaterGuardianSkill.ICE;

    private ObjectAttackScript CurSkill { get; set; }
    public bool CreatedSkill { get; private set; }

    private readonly Vector3 Skill3Offset = Vector3.up * 12;

    private readonly float DashPower = 15;
    private readonly float DashUp = 3;

    public override void StartSkill()
    {
        CurSkillIdx = NextSkillIdx;
        if (CurSkillIdx == -1) { return; }
        m_anim.SetInteger("SKILL_IDX", CurSkillIdx);
        m_anim.SetBool("IS_SKILLING", true);
        StopMove();
        CreatedSkill = false;
    }
    public override void SkillOn()
    {
        if (CurSkillIdx == SlashIdx)
        {
            CurSkill = SkillList[SlashIdx];
            CurSkill.SetAttack(this, m_skillDamage[SlashIdx]);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
        }
        else if (CurSkillIdx == RushIdx)
        {
            CurSkill = SkillList[RushIdx];
            CurSkill.SetAttack(this, m_skillDamage[RushIdx]);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
            m_rigid.velocity = DashPower * transform.forward + Vector3.up * DashUp;
            CreatedSkill = true;
        }
    }
    public override void CreateSkill()
    {
        if (CurSkillIdx == IceIdx)
        {
            CurSkill = SkillList[IceIdx];
            CurSkill.AttackOn();
            CurSkill.SetAttack(this, m_skillDamage[IceIdx]);

            Vector3 pos = CurTarget.Position + new Vector3(CurTarget.Velocity2.x,0,CurTarget.Velocity2.y);

            CurSkill.gameObject.transform.position = pos + Skill3Offset;
            CurSkill.gameObject.transform.SetParent(null);
            m_anim.SetBool("IS_SKILLING", false);
        }
    }
    public override void SkillOff()
    {
        if (CurSkillIdx == SlashIdx)
        {
            CurSkill.AttackOff();
            CreatedSkill = true;
            IsTracing = false;
        }
        else if (CurSkillIdx == RushIdx)
        {
            CurSkill.AttackOff();
            m_rigid.velocity = Vector3.zero;
        }
        m_anim.SetBool("IS_SKILLING", false);
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
        m_monsterStates[(int)EMonsterState.SKILL] = gameObject.AddComponent<WaterGuardianSkillState>();
    }
}