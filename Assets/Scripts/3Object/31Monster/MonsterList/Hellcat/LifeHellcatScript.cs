using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeHellcatScript : HellcatScript
{
    private readonly float HealPercent = 0.1f;

    public override void GaveDamage(ObjectScript _target, float _damage)
    {
        HealObj(_damage * HealPercent);
    }
}
