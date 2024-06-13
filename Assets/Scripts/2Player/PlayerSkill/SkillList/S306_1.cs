using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class S306_1 : ParabolaExplodeScript
{
    [SerializeField]
    private float angle = 20f;

    public override void FixedUpdate()
    {
        float radian = angle * Mathf.Deg2Rad;
        Vector3 rightForce=new Vector3(Mathf.Sin(radian),0,Mathf.Cos(radian));
        base.FixedUpdate();
    }
}