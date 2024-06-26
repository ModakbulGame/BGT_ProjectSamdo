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
    private AltarIconScript[] m_altarList;

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

    private void SetComps()
    {
        base.Start();
        m_oasisList = GetComponentsInChildren<OasisIconScript>();
        m_altarList = GetComponentsInChildren<AltarIconScript>();
        for (int i = 0; i < m_oasisList.Length; i++)
        {
            m_oasisList[i].SetParent(this);
            m_oasisList[i].SetComps((EOasisName)i, m_mapImage);
        }
        for (int i = 0; i < m_altarList.Length; i++)
        {
            m_altarList[i].SetParent(this);
            m_altarList[i].SetComps((EAltarName)i, m_mapImage);
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
