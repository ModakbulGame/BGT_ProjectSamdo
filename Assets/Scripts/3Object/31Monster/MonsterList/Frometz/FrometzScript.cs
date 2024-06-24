using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrometzScript : RangedAttackMonster
{
    public override void AddIdleState() { m_monsterStates[(int)EMonsterState.IDLE] = gameObject.AddComponent<FrometzIdleState>(); }
    public override void AddApproachState() { m_monsterStates[(int)EMonsterState.APPROACH] = gameObject.AddComponent<FrometzApproachState>(); }

    public override bool CanPurify => PlayerDistance > 0 && PlayerDistance <= PurifyDistance;

    public override Vector3 AttackOffset => new(0, 1, 1.25f);

    private readonly float PurifyDistance = 8;
    private float PlayerDistance { get { return PlayManager.GetDistToPlayer(Position); } }

    public override void StartIdle() { }
    public override void StartApproach() { }
}
