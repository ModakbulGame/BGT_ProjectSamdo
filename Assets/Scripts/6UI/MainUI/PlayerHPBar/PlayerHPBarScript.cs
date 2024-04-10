using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBarScript : MonoBehaviour
{
    private Slider m_slider;


    public void SetMaxHP(float _hp)
    {
        m_slider.maxValue = _hp;
        SetCurHP(_hp);
    }

    public void SetCurHP(float _hp)
    {
        m_slider.value = _hp;
    }


    public void SetComps()
    {
        m_slider = GetComponent<Slider>();
    }
}
