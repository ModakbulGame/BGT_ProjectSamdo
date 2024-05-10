using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HmmScript : AnimatedAttackMonster
{




    public override void SetStates()
    {
        base.SetStates();
        ReplaceState(EMonsterState.APPROACH, gameObject.AddComponent<HmmApproachScript>());
    }
}
