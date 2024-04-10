using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrailScript : EffectScript
{
    private Vector3 StartAngle { get; set; }


    private void AdjAngle()
    {
        float angle = PlayManager.CameraAngle;
        if (angle < -180) { angle += 360; }
        float adj = angle + 11;
        transform.localEulerAngles = StartAngle - Vector3.up * adj;
    }


    private void Start()
    {
        StartAngle = transform.localEulerAngles;
    }

    private void Update()
    {
        AdjAngle();
    }
}
