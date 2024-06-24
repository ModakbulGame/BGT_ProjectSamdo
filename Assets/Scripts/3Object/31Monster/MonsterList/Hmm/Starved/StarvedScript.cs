using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStarvedAttack
{
    SHORT,
    LONG,
    BUMP,

    LAST
}

public class StarvedScript : HmmScript
{
    public override bool CanPurify => IsExtorted;

    [Tooltip("기본 공격 별 데미지 배율")]
    [SerializeField]
    private float[] m_normalDamageMultiplier = new float[(int)EStarvedAttack.LAST]
        { 1, 1, 1 };

    public override void StartAttack()
    {
        AttackIdx = Random.Range(0, (int)EStarvedAttack.LAST);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        base.StartAttack();
    }
    public override void AttackTriggerOn()
    {
        bool isBump = AttackIdx == (int)EStarvedAttack.BUMP;
        int idx = !isBump ? 0 : 1;
        AttackTriggerOn(idx);
        float damage = Attack * m_normalDamageMultiplier[AttackIdx];
        AttackObject.SetAttack(this, damage);
    }
    public override void AttackTriggerOff()
    {
        AttackObject.AttackOff();
    }
}
