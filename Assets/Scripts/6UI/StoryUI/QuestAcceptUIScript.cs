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

    TextMeshProUGUI m_firstBtnTxt;
    TextMeshProUGUI m_secondBtnTxt;

    public void ShowNPCQuestUI()
    {
        gameObject.SetActive(true);
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

            PlayManager.AcceptQuest("Q002");
            PlayManager.ExpressCurQuestInfo();  // ����Ʈ â�� ǥ��
        }
        else PlayManager.CompleteQuest("Q002"); // �Ϸ� ��ư; ����Ʈ Ŭ����
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

        m_questTitle.text = PlayManager.QuestList[1].Title;
        m_questDescription.text = PlayManager.QuestList[1].Description;
        m_questRewards.text = $"���� {PlayManager.QuestList[1].Reward} {PlayManager.QuestList[0].RewardNum}��";

        m_btns[0].onClick.AddListener(AcceptQuest);   // ����, �Ϸ� ��ư
        m_btns[1].onClick.AddListener(DenyQuest);     // ����, Ȯ�� ��ư
    }

    private void Awake()
    {
        SetComps();
    }
}
