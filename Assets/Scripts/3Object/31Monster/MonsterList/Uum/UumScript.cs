using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class UumScript : AnimatedAttackMonster
{
    [SerializeField]
    private VisualEffect m_headFire;

    public override void SetDestination(Vector3 _destination)
    {
        base.SetDestination(_destination);
        StartMoveAnim();
    }
    public override void StopMove()
    {
        base.StopMove();
        StopMoveAnim();
    }



    private readonly float NarrowAttackMultiplier = 1.5f;

    public override void StartAttack()
    {
        AttackIdx = Random.Range(0, 5);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        base.StartAttack();
    }
    public override void CreateAttack()
    {
        float attack = Attack;
        if(AttackIdx == 2) { attack *= NarrowAttackMultiplier; }
        AttackObject = m_normalAttacks[AttackIdx].GetComponent<ObjectAttackScript>();
        AttackObject.SetAttack(this, attack);
        AttackObject.AttackOn();
    }
    public override void AttackTriggerOn()
    {
        switch (AttackIdx)
        {
            case 0:
            case 1:
                AttackObject = m_normalAttacks[AttackIdx].GetComponent<ObjectAttackScript>();
                AttackObject.SetAttack(this, Attack);
                break;
            case 3:
            case 4:
                AttackObject = m_normalAttacks[AttackIdx-3].GetComponent<ObjectAttackScript>();
                AttackObject.SetAttack(this, Attack);
                AttackObject.SetCCType(ECCType.KNOCKBACK);
                break;
        }
        AttackObject.AttackOn();
    }
    public override void AttackTriggerOff()
    {
        AttackObject.AttackOff();
    }
    public override void AttackDone()
    {
        AttackObject.ResetCCType();
        base.AttackDone();
    }
    public override void LookTarget()
    {
        if(AttackIdx == 3 || AttackIdx == 4) { return; }
        base.LookTarget();
    }


    public override void StartDissolve()
    {
        base.StartDissolve();
        m_headFire.Stop();
    }
}
