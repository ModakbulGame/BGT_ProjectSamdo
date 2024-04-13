using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrometzScript : RangedAttackMonster
{
    private readonly Vector3 AttackOffset = new(0, 1, 1.25f);


    public override void CreateAttack()
    {
        GameObject attack = m_attackPool.Get();
        attack.transform.localPosition = AttackOffset;
        attack.transform.parent = null;

        Vector3 dir = (CurTarget.Position - Position).normalized;

        MonsterProjectileScript script = attack.GetComponent<MonsterProjectileScript>();
        script.SetAttack(this, dir, Attack);
        script.AttackOn();
    }

    public override void StartRoaming() { }
    public override void StartApproach() { }


    public override void SetStates()
    {
        base.SetStates();
        ReplaceState(EMonsterState.ROAMING, gameObject.AddComponent<FrometzRoamingState>());
        ReplaceState(EMonsterState.APPROACH, gameObject.AddComponent<FrometzApproachState>());
    }
}
