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
    private float[] AttackAngle = new float[4] { 0, 30, -30, -45 };


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
        AttackIdx = 0/*Random.Range(0, 4)*/;
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
}