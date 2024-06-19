using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum EPowerTrailType
{
    NONE,
    POWER1,

    LAST
}



public class WeaponTrailEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject normalObj;
    [SerializeField]
    private GameObject powerObj;

    private TrailRenderer[] m_normalTrails;
    private PowerTrailEffectScript[] m_powerTrails;

    private int CurIdx { get; set; }

    public void SetNormalTrail(bool _on)
    {
        foreach (TrailRenderer trail in m_normalTrails) { trail.enabled = _on; }
    }

    public void PowerTrailOn(EPowerTrailType _type)
    {
        CurIdx = (int)_type - 1;
        m_powerTrails[CurIdx].SerPowerTrail(true);
    }
    public void PowerTrailOff()
    {
        m_powerTrails[CurIdx].SerPowerTrail(false);
    }


    public void SetComps()
    {
        m_normalTrails = normalObj.GetComponentsInChildren<TrailRenderer>();
        SetNormalTrail(false);
        m_powerTrails = powerObj.GetComponentsInChildren<PowerTrailEffectScript>();
        foreach (PowerTrailEffectScript trail in m_powerTrails) { trail.SetComps(); }
    }
}
