using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using StylizedWater2;

public class MapUIScript : MinimapScript
{
    [SerializeField]
    private GameObject m_oasisImage;
    private RectTransform OasisRect
    {
        get
        {
            Image oasis = m_oasisImage.GetComponent<Image>();
            if (oasis != null) return oasis.rectTransform;
            return null;
        }
    }

    private OasisNPC[] OasisList { get { return PlayManager.OasisList; } }
    private SlateScript[] SlateList { get { return PlayManager.SlateList; } }
    private AltarScript[] AltarList { get { return PlayManager.AltarList; } }


    public void OpenUI()                                    // UI 열기
    {
        gameObject.SetActive(true);
        SetComps();
    }

    public void CloseUI() { GameManager.SetControlMode(EControlMode.THIRD_PERSON); gameObject.SetActive(false); }      // 닫기

    private void SetOasisPosition()
    {
        for (uint i = 0; i < 9; i++)
        {
            Vector2 oasis = OasisList[i].Position2;
            Debug.Log(OasisList[i].Position2);
            Vector2 oasisOffset = oasis / MapHeight;
            OasisRect.pivot = oasisOffset;
        }
    }

    private void SetComps()
    {
        base.Start();
        for (uint i = 0; i < OasisList.Length; i++)
        {
            GameObject OasisImage = Instantiate(m_oasisImage, Vector3.zero, Quaternion.identity, m_mapImg.transform);
        }
        // SetOasisPosition();
    }

    protected override void Update()
    {
        base.Update();
        // SetOasisPosition();
    }
}
