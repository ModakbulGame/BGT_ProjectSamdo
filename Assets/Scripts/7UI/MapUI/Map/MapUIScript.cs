using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using StylizedWater2;
using System;
using Pathfinding;

public class MapUIScript : MinimapScript
{
    [SerializeField]
    private RectTransform m_mapImage;
    private OasisIconScript[] m_oasisList;

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

    private void SetOasisPosition()
    {
        for (int i = 0; i < (int)EOasisName.LAST; i++)
        {
            Vector2 oasis = OasisList[i].Position2;
            Vector2 oasisOffset = oasis / MapHeight;
            // OasisRect[i].pivot = oasisOffset;
        }
    }

    private void SetComps()
    {
        base.Start();
        m_oasisList = GetComponentsInChildren<OasisIconScript>();
        for (int i = 0; i < m_oasisList.Length; i++)
        {
            m_oasisList[i].SetParent(this);
            m_oasisList[i].SetComps((EOasisName)i, m_mapImage);
        }
    }

    protected override void Update()
    {
        base.Update();
        SetOasisPosition();
    }
}
