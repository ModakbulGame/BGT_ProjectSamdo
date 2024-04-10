using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHPBarScript : IngameTrackUIScript                // 오브젝트 HP바
{
    private Slider m_hpSlider;


    public void SetMaxHP(float _max)
    {
        m_hpSlider.maxValue = (int)_max;
        SetCurHP(_max);
    }

    public void SetCurHP(float _hp)
    {
        m_hpSlider.value = (int)_hp;
    }


    private void SetComps()
    {
        m_hpSlider = GetComponentInChildren<Slider>();
    }
    private void Awake()
    {
        SetComps();
    }
}
