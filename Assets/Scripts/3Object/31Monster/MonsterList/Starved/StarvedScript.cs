using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarvedScript : MonsterScript
{
    private readonly Vector3 NormalAttackOffset = new(0, 1.1f, 1.8f);

    public override void CreateAttack()
    {
        CreateNormalAttack(NormalAttackOffset);
    }
}
