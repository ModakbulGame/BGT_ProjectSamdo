using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMiserableAttack
{
    LEFT_HEAD,
    RIGHT_HEAD,
    HEAD_BUMP,
    TRIPLE_ATTACK,

    LAST
}

public class MiserableScript : HmmScript
{
    public override bool CanPurify => IsMelancholy;


    [Tooltip("3연격 시 전진 힘")]
    [SerializeField]
    private float m_tripleAttackForward = 8;
    [Tooltip("기본 공격 별 데미지 배율")]
    [SerializeField]
    private float[] m_normalDamageMultiplier = new float[(int)EMiserableAttack.LAST]
        { 1, 1, 1, 1 };



    private bool IsTripleAttack { get { return AttackIdx == (int)EMiserableAttack.TRIPLE_ATTACK; } }
    private int SkillIdx { get; set; }
    public override void StartAttack()
    {
        if(IsTripleAttack) { SkillIdx = 0; }
        AttackIdx =  Random.Range(0, (int)EMiserableAttack.LAST);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        base.StartAttack();
    }
    public override void CreateAttack()
    {
        m_rigid.velocity = m_tripleAttackForward * transform.forward;
    }
    public override void AttackTriggerOn()
    {
        int idx;
        if (!IsTripleAttack) { idx = 0; }
        else { idx = SkillIdx % 2 + 1; SkillIdx++; }
        AttackTriggerOn(idx);
        float damage = Attack * m_normalDamageMultiplier[AttackIdx];
        AttackObject.SetAttack(this, damage);
    }
    public override void AttackTriggerOff()
    {
        AttackObject.AttackOff();
    }
    public override void AttackDone()
    {
        base.AttackDone();
        if(AttackIdx <= 1) { AttackTimeCount *= 0.5f; }
    }
}
