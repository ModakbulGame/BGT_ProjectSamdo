using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlotUIScript : MonoBehaviour
{
    private SkillSlotElmUIScript[] m_elms;


    public void UpdateUI()
    {
        ESkillName[] slot = GameManager.SkillSlot;
        for (int i = 0; i<ValueDefine.MAX_SKILL_SLOT; i++)
        {
            m_elms[i].SetSkill(slot[i]);
        }
    }

    public void UseSkill(int _idx, float _cooltime)
    {
        m_elms[_idx].UseSkill(_cooltime);
    }


    public void SetComps()
    {
        m_elms = GetComponentsInChildren<SkillSlotElmUIScript>();
        foreach(SkillSlotElmUIScript elm in m_elms) { elm.SetComps(); }
    }
}
