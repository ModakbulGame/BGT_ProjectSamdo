using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MiniQuestUIScript : MonoBehaviour
{
    private GridLayoutGroup m_grid;

    private TextMeshProUGUI m_title;
    private TextMeshProUGUI m_summary;

    private int idx = 0;

    public void ExpressCurQuestInfo()
    {
        m_grid = GetComponentInChildren<GridLayoutGroup>();
        Transform gridTransform = m_grid.transform;

        m_title = gridTransform.GetChild(idx).GetComponentsInChildren<TextMeshProUGUI>()[0];
        m_summary = gridTransform.GetChild(idx).GetComponentsInChildren<TextMeshProUGUI>()[1];

        m_title.text = PlayManager.CurQuestList[idx].Title;
        m_summary.text = PlayManager.CurQuestList[idx].Description;

        idx++;
    }

    private void SetComps()
    {

    }

    void Start()
    {
        SetComps();
    }
}
