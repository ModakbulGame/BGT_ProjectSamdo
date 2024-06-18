using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MiniQuestUIScript : MonoBehaviour
{
    private GridLayoutGroup m_grid;
    private Transform m_gridTransform;

    private TextMeshProUGUI m_title;    // 퀘스트 이름
    private TextMeshProUGUI m_summary;  // 퀘스트 설명 요약
    
    private Image m_clearImg;           // 보상 수령 버튼
    public Image[] ClearImg { get; private set; }

    private int idx = 0;

    public void ExpressCurQuestInfo()
    {
        m_title = m_gridTransform.GetChild(idx).GetComponentsInChildren<TextMeshProUGUI>()[0];    
        m_summary = m_gridTransform.GetChild(idx).GetComponentsInChildren<TextMeshProUGUI>()[1];

        m_title.text = PlayManager.CurQuestList[idx].Name;
        m_summary.text = PlayManager.CurQuestList[idx].Description;

        idx++;
    }

    private void SetComps()
    {
        m_grid = GetComponentInChildren<GridLayoutGroup>();
        m_gridTransform = m_grid.transform;

        ClearImg = new Image[m_gridTransform.childCount];

        for (int i = 0; i < m_gridTransform.childCount; i++)
        {
            m_clearImg = m_gridTransform.GetChild(i).GetChild(2).GetComponentInChildren<Image>();
            ClearImg[i] = m_clearImg;
            m_clearImg.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        SetComps();
    }
}
