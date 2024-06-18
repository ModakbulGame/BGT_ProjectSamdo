using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrailEffect : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer[] m_effects;


    public void SetTrail(bool _on)
    {
        foreach(TrailRenderer effect in m_effects) { effect.enabled = _on; }
    }
}
