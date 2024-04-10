using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimUIScript : MonoBehaviour
{
    private StateUIScript m_stateUI;

    public void SetStaminaRate(float _rate)
    {
        m_stateUI.SetStaminaRate(_rate);
    }

    public void SetLightRate(float _rate)
    {
        m_stateUI.SetLightRate(_rate);
    }


    private void SetComps()
    {
        m_stateUI = GetComponentInChildren<StateUIScript>();
        m_stateUI.SetComps();
    }

    private void Awake()
    {
        SetComps();
    }
}
