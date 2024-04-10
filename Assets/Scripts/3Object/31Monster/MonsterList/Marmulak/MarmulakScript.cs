using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarmulakScript : MonsterScript
{
    private readonly Vector3 NormalAttackOffset = new();


    public override void CreateAttack()
    {
        CreateNormalAttack(NormalAttackOffset);
    }


}
