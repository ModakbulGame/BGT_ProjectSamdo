using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGuardianScript : AnimatedAttackMonster
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
        SlowRotate(dir);
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


    private readonly float AnySkillCooltime = 8;
    private float AnySkillTimeCount { get; set; }

    public override int SkillNum => 3;
    public override bool CanSkill => AnySkillTimeCount <= 0 && HasTarget && TargetInAttackRange && CheckCurSkill != -1;

    private ObjectAttackScript CurSkill { get; set; }
    public bool CreatedSkill { get; private set; }
    public bool RushStarted { get; private set; }

    public override void StartSkill()
    {
        CurSkillIdx = CheckCurSkill;
        if(CurSkillIdx == -1) { return; }
        m_anim.SetInteger("SKILL_IDX", CurSkillIdx);
        m_anim.SetBool("IS_SKILLING", true);
        StopMove();
        CreatedSkill = false;
    }
    public override void SkillOn()
    {
        if (CurSkillIdx == 0)
        {
            CurSkill = SkillList[0];
            CurSkill.SetAttack(this, 10);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
        }
        else if (CurSkillIdx == 1)
        {
            CurSkill = SkillList[1];
            CurSkill.SetAttack(this, 10);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
        }
        else if (CurSkillIdx == 2)
        {
            CurSkill = SkillList[2];
            CurSkill.SetAttack(this, 10);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
            RushStarted = true;
        }
        CreatedSkill = true;
    }
    public override void SkillOff()
    {
        if (CurSkillIdx == 0)
        {
            CurSkill.AttackOff();
            CurSkill.gameObject.SetActive(false);
        }
        else if (CurSkillIdx == 1)
        {
            CurSkill.AttackOff();
            CurSkill.gameObject.SetActive(false);
        }
        else if (CurSkillIdx == 2)
        {
            CurSkill.AttackOff();
            CurSkill.gameObject.SetActive(false);
            RushStarted = false;
        }
        m_anim.SetBool("IS_SKILLING", false);
    }
    public void RushForward()
    {
        m_rigid.velocity = 8 * transform.forward;
    }
    public override void SkillDone()
    {
        base.SkillDone();

        SkillTimeCount[CurSkillIdx] = SkillCooltime[CurSkillIdx];
        AnySkillTimeCount = AnySkillCooltime;
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

    public override void InitSkillInfo()
    {
        base.InitSkillInfo();
        SkillCooltime = new float[3] { 10, 10, 10 };
    }

    public override void SetStates()
    {
        base.SetStates();
        m_monsterStates[(int)EMonsterState.SKILL] = gameObject.AddComponent<LifeGuardianSkillState>();
    }
}