using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedAttackMonster : MonsterScript
{
    public override void AttackTriggerOn()
    {
        base.AttackTriggerOn();
        AttackObject.SetDamage(Attack);
    }
}
