using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPBarScript : MonoBehaviour
{
    private Slider m_slider;
    private float m_curHP;
    private float m_maxHP;
    private TextMeshProUGUI m_hpValue;

    public void SetMaxHP(float _hp)
    {
        m_slider.maxValue = _hp;
        SetCurHP(_hp);
        m_maxHP = _hp;
    }

    public void SetCurHP(float _hp)
    {
        m_slider.value = _hp;
        m_curHP = _hp;
    }


    public void SetComps()
    {
        m_slider = GetComponent<Slider>();
        m_hpValue = GetComponentInChildren<TextMeshProUGUI>();
        m_hpValue.text = $"{m_curHP} / {m_maxHP}"; 
    }
}
