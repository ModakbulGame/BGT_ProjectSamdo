using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestAcceptUIScript : MonoBehaviour
{
    private QuestObject m_curQuestObject;
    private Button[] m_btns;

    // 퀘스트 정보
    [SerializeField]
    private TextMeshProUGUI m_questTitle;
    [SerializeField]
    private TextMeshProUGUI m_questDescription;
    [SerializeField]
    private TextMeshProUGUI m_questRewards;

    private TextMeshProUGUI m_firstBtnTxt;       // 수락, 완료 버튼
    private TextMeshProUGUI m_secondBtnTxt;      // 거절, 확인 버튼
    private List<QuestScriptable> m_npcQuests;   // StartObject에 맞는 퀘스트들의 List
    private int idx = 0;

    public void ShowNPCQuestUI(QuestNPCScript _npc)
    {
        SetNPCQuestList(_npc);
        gameObject.SetActive(true);
    }

    private void SetNPCQuestList(QuestNPCScript _npc)
    {
        m_npcQuests = _npc.NPCQuestList;
    }

    public void ChangeBtnsText()
    {
        m_firstBtnTxt.text = "완료";
        m_secondBtnTxt.text = "확인";
    }

    private void AcceptQuest()
    {
        if (m_firstBtnTxt.text == "수락")
        {
            Debug.Log("퀘스트 수락!"); 

            PlayManager.AcceptQuest(m_npcQuests[idx].Id);
            PlayManager.ExpressCurQuestInfo();  // 퀘스트 창에 표시
        }
        else PlayManager.CompleteQuest(m_npcQuests[idx].Id); // 완료 버튼; 퀘스트 클리어
        CloseNPCQuestUI();
    }

    private void DenyQuest()
    {
        CloseNPCQuestUI();
    }

    private void CloseNPCQuestUI()
    {
        gameObject.SetActive(false);
        PlayManager.CloseNPCUI();
    }

    public void SetComps()
    {
        m_btns = GetComponentsInChildren<Button>();
        m_firstBtnTxt = m_btns[0].GetComponentInChildren<TextMeshProUGUI>();
        m_secondBtnTxt = m_btns[1].GetComponentInChildren<TextMeshProUGUI>();

        m_questTitle.text = m_npcQuests[idx].Title;
        m_questDescription.text = m_npcQuests[idx].Description;
        m_questRewards.text = $"보상 {m_npcQuests[idx].Reward} {m_npcQuests[idx].RewardNum}개";

        for (int i = 0; i < m_npcQuests.Count; i++) Debug.Log(m_npcQuests[i]);

        m_btns[0].onClick.AddListener(AcceptQuest);   // 수락, 완료 버튼
        m_btns[1].onClick.AddListener(DenyQuest);     // 거절, 확인 버튼
    }

    private void Awake()
    {
        SetComps();
    }
}
