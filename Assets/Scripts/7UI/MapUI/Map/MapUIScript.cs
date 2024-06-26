using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using StylizedWater2;
using System;

public class MapUIScript : MinimapScript
{
    private OasisIconScript[] m_oasisList;
    public RectTransform[] OasisRect
    {
        get
        {
            for(int i = 0; i < (int)EOasisName.LAST; i++)
            {
                RectTransform[] oasisRects = m_oasisList[i].GetComponents<RectTransform>();
                if(oasisRects != null) return oasisRects;
            }
            return null;
        }
    }
    private int idx = 0;

    public OasisNPC[] OasisList { get { return PlayManager.OasisList; } }
    public SlateScript[] SlateList { get { return PlayManager.SlateList; } }
    public AltarScript[] AltarList { get { return PlayManager.AltarList; } }


    public void OpenUI()                                    // UI 열기
    {
        gameObject.SetActive(true);
        SetComps();
    }

    public void CloseUI() 
    { 
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
        gameObject.SetActive(false);
    }

    public void InitOasisPosition(Image _oasis)
    {
        Transform mapOasisTransform = OasisList[idx].transform;

        _oasis.rectTransform.anchoredPosition = new Vector2(m_mapImg.rectTransform.sizeDelta.x * PlayManager.NormalizeLocation(mapOasisTransform).x,
            m_mapImg.rectTransform.sizeDelta.y * PlayManager.NormalizeLocation(mapOasisTransform).y);
        idx++;
    }

    private void SetOasisPosition()
    {
        for (int i = 0; i < (int)EOasisName.LAST; i++)
        {
            Vector2 oasis = OasisList[i].Position2;
            Vector2 oasisOffset = oasis / MapHeight;
            OasisRect[i].pivot = oasisOffset;
        }
    }


    private void SetComps()
    {
        base.Start();
        m_oasisList = GetComponentsInChildren<OasisIconScript>();
        foreach(OasisIconScript oasis in m_oasisList) { oasis.SetParent(this); oasis.SetComps(); }
    }

    protected override void Update()
    {
        base.Update();
        SetOasisPosition();
    }
}
