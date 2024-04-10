using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBoxSlotElmScript : MonoBehaviour
{
    private Image m_skillImg;



    public void SetSkill(ESkillName _skill)
    {
        if(_skill == ESkillName.LAST) { m_skillImg.gameObject.SetActive(false); return; }
        else if(!m_skillImg.gameObject.activeSelf) { m_skillImg.gameObject.SetActive(true); }
        m_skillImg.sprite = GameManager.GetSkillSprite(_skill);
    }





    public void SetComps()
    {
        m_skillImg = GetComponentsInChildren<Image>()[1];
    }
}
