using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiserableScript : MonsterScript
{
    private readonly Vector3 NormalAttackOffset = new(0, 1.78f, 1.795f);





    public override void CreateAttack()
    {
        CreateNormalAttack(NormalAttackOffset);
    }
}
