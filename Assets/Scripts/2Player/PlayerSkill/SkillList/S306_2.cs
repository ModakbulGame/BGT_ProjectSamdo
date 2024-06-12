using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class S306_2 : ParabolaExplodeScript
{
    [SerializeField]
    private float angle = 20f;

    public override void FixedUpdate()
    {
        float radian = angle * Mathf.Deg2Rad;
        Vector3 leftForce=new Vector3(Mathf.Cos(radian),0,Mathf.Sin(radian));
        base.FixedUpdate();
    }
}