using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleHitObject : MonoBehaviour, IHittable
{
    public bool IsPlayer { get{ return false; } }

    public bool IsMonster { get { return false; } }

    public void GetHit(HitData _hit)
    {
        Debug.Log(_hit.Damage);
    }
}
