using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPowerScript : PlayerPowerScript
{
    public void SummonDoneTiming()
    {
        PowerSummoned();
    }
    public virtual void PowerSummoned()
    {

    }

    public override void AttackOn() { }
}
