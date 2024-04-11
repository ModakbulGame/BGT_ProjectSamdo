using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillBoxUIScript : PlayerInfoBoxScript
{
    private SkillBoxSlotScript m_slot;
    private SkillBoxElmScript[] m_elms;

    [SerializeField]
    private Button[] m_pageBtns = new Button[2];

    public RectTransform[] SlotTrans { get { return m_slot.ElmTrans; } }


    private readonly List<ESkillName> m_obtainedList = new();
    private static int CurPage { get; set; } = 0;
    private int MaxPage { get { if (m_obtainedList.Count == 0) return 0; return (m_obtainedList.Count - 1) / ElmPerPage; } }
    private const int ElmPerPage = 12;

    public override void InitUI()
    {
        UpdateUI();
    }

    private void SetPageBtn()
    {
        m_pageBtns[0].interactable = (CurPage > 0);
        m_pageBtns[1].interactable = (CurPage < MaxPage);
    }

    private void Changepage(bool _toNext)
    {
        if (_toNext) { CurPage++; }
        else { CurPage--; }
        UpdateUI();
    }


    public override void UpdateUI()
    {
        ESkillName[] slot = GameManager.SkillSlot;
        m_slot.UpdateSlot(slot);

        m_obtainedList.Clear();
        for (int i = 0; i<(int)ESkillName.LAST; i++)
        {
            ESkillName skill = (ESkillName)i;
            if (GameManager.GetSkillInfo(skill).Obtained) { m_obtainedList.Add(skill); }
        }
        int start = CurPage * ElmPerPage;
        for (int i = 0; i<ElmPerPage; i++)
        {
            int idx = start + i;
            if (idx >= m_obtainedList.Count) { m_elms[i].HideElm(); continue; }
            if (!m_elms[i].gameObject.activeSelf) { m_elms[i].gameObject.SetActive(true); }
            ESkillName skill = (ESkillName)idx;
            m_elms[i].SetSkillInfo(skill, slot.Contains(skill));
        }

        SetPageBtn();
    }



    private void SetBtns()
    {
        m_pageBtns[0].onClick.AddListener(delegate { Changepage(false); });
        m_pageBtns[1].onClick.AddListener(delegate { Changepage(true); });
    }

    public override void SetComps()
    {
        m_slot = GetComponentInChildren<SkillBoxSlotScript>();
        m_slot.SetComps();
        m_elms = GetComponentsInChildren<SkillBoxElmScript>();
        foreach (SkillBoxElmScript elm in m_elms) { elm.SetParent(this); }
        SetBtns();
    }
}
