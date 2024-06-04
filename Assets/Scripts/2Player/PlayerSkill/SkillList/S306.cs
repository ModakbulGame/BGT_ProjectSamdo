using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class S306 : ParabolaSkillScript
{
    [SerializeField]
    private GameObject[] skillObject;
    [SerializeField]
    private float angleBetweenObjects = 60f;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Quaternion rotation1 = Quaternion.AngleAxis(-angleBetweenObjects / 2, transform.up);
        Quaternion rotation2 = Quaternion.AngleAxis(angleBetweenObjects / 2, transform.up);
    }
}