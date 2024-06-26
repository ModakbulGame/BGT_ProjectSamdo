using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrailEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject powerObj;

    private PowerTrailEffectScript[] m_powerTrails;

    private int CurIdx { get; set; }

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
        m_powerTrails = powerObj.GetComponentsInChildren<PowerTrailEffectScript>();
        foreach (PowerTrailEffectScript trail in m_powerTrails) { trail.SetComps(); }
    }
}
