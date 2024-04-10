using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkBeakScript : MonsterScript
{
    private readonly Vector3 NormalAttackOffset = new(0, 0.75f, 1.25f);

    public override void CreateAttack()
    {
        CreateNormalAttack(NormalAttackOffset);
    }
}
