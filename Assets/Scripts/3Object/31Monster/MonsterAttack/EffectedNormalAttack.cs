using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectedNormalAttack : NormalAttackScript
{
    [SerializeField]
    private AttackEffect m_effect;

    public override void AttackOn()
    {
        base.AttackOn();
        m_effect.EffectOn(transform);
    }
}
