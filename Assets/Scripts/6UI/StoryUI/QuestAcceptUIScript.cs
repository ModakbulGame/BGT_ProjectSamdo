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

    // ����Ʈ ����
    [SerializeField]
    private TextMeshProUGUI m_questTitle;
    [SerializeField]
    private TextMeshProUGUI m_questDescription;
    [SerializeField]
    private TextMeshProUGUI m_questRewards;

    private TextMeshProUGUI m_firstBtnTxt;       // ����, �Ϸ� ��ư
    private TextMeshProUGUI m_secondBtnTxt;      // ����, Ȯ�� ��ư
    private List<QuestScriptable> m_npcQuests;   // StartObject�� �´� ����Ʈ���� List
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
        m_firstBtnTxt.text = "�Ϸ�";
        m_secondBtnTxt.text = "Ȯ��";
    }

    private void AcceptQuest()
    {
        if (m_firstBtnTxt.text == "����")
        {
            Debug.Log("����Ʈ ����!"); 

            PlayManager.AcceptQuest(m_npcQuests[idx].Id);
            PlayManager.ExpressCurQuestInfo();  // ����Ʈ â�� ǥ��
        }
        else PlayManager.CompleteQuest(m_npcQuests[idx].Id); // �Ϸ� ��ư; ����Ʈ Ŭ����
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
        m_questRewards.text = $"���� {m_npcQuests[idx].Reward} {m_npcQuests[idx].RewardNum}��";

        for (int i = 0; i < m_npcQuests.Count; i++) Debug.Log(m_npcQuests[i]);

        m_btns[0].onClick.AddListener(AcceptQuest);   // ����, �Ϸ� ��ư
        m_btns[1].onClick.AddListener(DenyQuest);     // ����, Ȯ�� ��ư
    }

    private void Awake()
    {
        SetComps();
    }
}
