using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBoxSlotScript : MonoBehaviour
{
    private SkillBoxSlotElmScript[] m_slots;
    public RectTransform[] ElmTrans { get; set; } = new RectTransform[ValueDefine.MAX_SKILL_SLOT];

    public void UpdateSlot(ESkillName[] _skillSlot)
    {
        for (int i = 0; i<ValueDefine.MAX_SKILL_SLOT; i++)
        {
            m_slots[i].SetSkill(_skillSlot[i]);
        }
    }


    public void SetComps()
    {
        m_slots = GetComponentsInChildren<SkillBoxSlotElmScript>();
        for(int i=0;i<ValueDefine.MAX_SKILL_SLOT;i++)
        {
            m_slots[i].SetComps();
            ElmTrans[i] = m_slots[i].GetComponent<RectTransform>();
        }
    }
}
