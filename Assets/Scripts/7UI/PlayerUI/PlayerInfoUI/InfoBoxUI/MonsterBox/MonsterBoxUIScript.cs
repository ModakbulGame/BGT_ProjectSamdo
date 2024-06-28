using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBoxUIScript : PlayerInfoBoxScript
{
    private MonsterBoxElmScript[] m_elms;

    [SerializeField]
    private Button[] m_pageBtns = new Button[2];

    private static int CurPage { get; set; } = 0;
    private int MaxPage { get { return ((int)EMonsterName.LAST - 1) / ElmPerPage; } }
    private const int ElmPerPage = 8;

    public override void InitUI()
    {
        m_parent.ShowMonterImgUI();
        UpdateUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        m_parent.HideMonsterImgUI();
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
        int start = CurPage * ElmPerPage;
        for (int i = 0; i<ElmPerPage; i++)
        {
            int idx = start + i;
            if (idx >= (int)EMonsterName.LAST) { m_elms[i].HideElm(); continue; }
            if (!m_elms[i].gameObject.activeSelf) { m_elms[i].gameObject.SetActive(true); }
            m_elms[i].SetMonsterInfo((EMonsterName)idx);
        }
        SetPageBtn();
    }

    private void SetBtns()
    {
        m_pageBtns[0].onClick.AddListener(delegate { Changepage(false); });
        m_pageBtns[1].onClick.AddListener(delegate { Changepage(true); });
    }

    public void SetMonsterImg(EMonsterName _name)
    {
        m_parent.SetMonsterImg(_name);
    }

    public override void SetComps()
    {
        m_elms = GetComponentsInChildren<MonsterBoxElmScript>();
        if(m_elms.Length != ElmPerPage) { Debug.LogError("UI 개수 틀림"); }
        foreach (MonsterBoxElmScript elm in m_elms) elm.SetParent(this);
        SetBtns();
    }
}
