using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yum2Script : MonsterScript
{
    public override void CreateAttack()
    {
        GameObject attack = Instantiate(m_normalAttackPrefab, transform);
        attack.transform.parent = null;

        Vector3 dir = (CurTarget.Position - Position).normalized;

        MonsterProjectileScript script = attack.GetComponent<MonsterProjectileScript>();
        script.SetAttack(this, dir, Attack);
    }
}
