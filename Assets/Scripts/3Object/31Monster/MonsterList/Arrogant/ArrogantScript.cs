using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrogantScript : MonsterScript
{
    private readonly Vector3 NormalAttackOffset = new(-0.07f, 2.09f, 2.31f);





    public override void CreateAttack()
    {
        CreateNormalAttack(NormalAttackOffset);
    }
}
