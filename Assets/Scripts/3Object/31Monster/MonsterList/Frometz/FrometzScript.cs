using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrometzScript : MonsterScript
{
    public override void CreateAttack()
    {
        GameObject attack = Instantiate(m_normalAttackPrefab, transform);
        attack.transform.parent = null;

        Vector3 dir = (CurTarget.Position - Position).normalized;

        MonsterProjectileScript script = attack.GetComponent<MonsterProjectileScript>();
        script.SetAttack(this, dir, Attack);
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
