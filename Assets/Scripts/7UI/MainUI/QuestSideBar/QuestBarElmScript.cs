using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestBarElmScript : MonoBehaviour
{
    private TextMeshProUGUI m_descTxt;
    private TextMeshProUGUI m_stateTxt;

    public void SetElm(QuestInfo _info)
    {
        if (!gameObject.activeSelf) { gameObject.SetActive(true); }

        string desc = _info.QuestData.Description;
        string prog = $"({_info.QuestProgress}/{_info.QuestData.Content.Amount})";
        m_descTxt.text = $"{desc} {prog}";
        m_stateTxt.text = _info.State == EQuestState.ACCEPTED ? "진행중" : "완료됨";
    }

    public void HideElm()
    {
        gameObject.SetActive(false);
    }


    public void SetComps()
    {
        TextMeshProUGUI[] txts = GetComponentsInChildren<TextMeshProUGUI>();
        m_descTxt = txts[0];
        m_stateTxt = txts[1];
    }
}
