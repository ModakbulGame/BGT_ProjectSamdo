using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTrailEffectScript : MonoBehaviour
{
    TrailRenderer[] m_renderers;

    public void SerPowerTrail(bool _on)
    {
        foreach(TrailRenderer ren in m_renderers) { ren.enabled = _on; }
    }

    public void SetComps()
    {
        m_renderers = GetComponentsInChildren<TrailRenderer>();
        SerPowerTrail(false);
    }
}
