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
    private GameObject m_mapOasisImage;
    private bool m_isMapUIToggle = false;

    private OasisNPC[] OasisList { get { return PlayManager.OasisList; } }


    public void OpenUI()                                    // UI 열기
    {
        gameObject.SetActive(true);
        SetComps();
    }

    public void CloseUI() { GameManager.SetControlMode(EControlMode.THIRD_PERSON); gameObject.SetActive(false); }      // 닫기

    private void SynchronizeOasisLocation()
    {
        for (uint i = 0; i < OasisList.Length; i++)
        {
            GameObject OasisImage = Instantiate(m_mapOasisImage, Vector3.zero, Quaternion.identity, m_mapImg.transform);
            Image mapOasisImage = OasisImage.GetComponent<Image>();
            Transform mapOasisTransform = OasisList[i].transform;

            mapOasisImage.rectTransform.anchoredPosition = new Vector2(m_mapImg.rectTransform.sizeDelta.x * PlayManager.NormalizeLocation(mapOasisTransform).x, 
                m_mapImg.rectTransform.sizeDelta.y * PlayManager.NormalizeLocation(mapOasisTransform).y);
        }
    }

    private void SetComps()
    {
        base.Start();
        // SynchronizeOasisLocation();
    }

    protected override void Update()
    {
        base.Update();
    }
}
