using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HmmScript : AnimatedAttackMonster
{
    public override void SetDestination(Vector3 _destination)
    {
        base.SetDestination(_destination);
        StartMoveAnim();
    }
    public override void StopMove()
    {
        base.StopMove();
        StopMoveAnim();
    }

}
