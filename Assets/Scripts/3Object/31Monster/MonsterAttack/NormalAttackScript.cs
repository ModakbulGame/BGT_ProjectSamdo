using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackScript : ObjectAttackScript
{
    private const float DestoryTime = 0.33f;


    private void Awake()
    {
        IsAttacking = true;
        Destroy(gameObject, DestoryTime);
    }

    public override void Start() { }
}
