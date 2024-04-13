using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrometzScript : RangedAttackMonster
{
    public override Vector3 AttackOffset => new(0, 1, 1.25f);


    public override void StartRoaming() { }
    public override void StartApproach() { }


    public override void SetStates()
    {
        base.SetStates();
        ReplaceState(EMonsterState.ROAMING, gameObject.AddComponent<FrometzRoamingState>());
        ReplaceState(EMonsterState.APPROACH, gameObject.AddComponent<FrometzApproachState>());
    }
}
