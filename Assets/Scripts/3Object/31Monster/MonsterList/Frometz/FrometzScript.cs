using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrometzScript : MonsterScript
{

    public override void StartRoaming() { }
    public override void StartApproach() { }








    public override void SetStates()
    {
        base.SetStates();
        ReplaceState(EMonsterState.ROAMING, gameObject.AddComponent<FrometzRoamingState>());
        ReplaceState(EMonsterState.APPROACH, gameObject.AddComponent<FrometzApproachState>());
    }
}
