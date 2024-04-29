using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarvedScript : MonsterScript
{
    public override void AttackTriggerOn()
    {
        base.AttackTriggerOn();
        AttackObject.SetDamage(Attack);
    }
}
