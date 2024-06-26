using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S303 : ProjectilePowerScript
{
    public override void CollideTarget()
    {
        GameObject effect = GameManager.GetEffectObj(EEffectName.HIT_POWER);
        effect.transform.position = transform.position;
    }

    public override void ReleaseToPool()
    {
        ActiveExplodes();
        base.ReleaseToPool();
    }
}
