using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestAcceptUIScript : MonoBehaviour
{
    public bool QuestAvailable { get; set; }
    public bool QuestRunning { get; set; }

    private QuestObject m_curQuestObject;
    private Button[] m_btns;

    // ����Ʈ ����
    [SerializeField]
    private TextMeshProUGUI m_questTitle;
    [SerializeField]
    private TextMeshProUGUI m_questDescription;
    [SerializeField]
    private TextMeshProUGUI m_questRewards;

    public void ShowNPCQuestUI()
    {
        gameObject.SetActive(true);
    }

    private void AcceptQuest()
    {
        Debug.Log("����Ʈ ����!");
        PlayManager.AcceptQuest("Q001");
        PlayManager.ExpressCurQuestInfo();  // ����Ʈ â�� ǥ��
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
        QuestAvailable = false;
        QuestRunning = false;

        // ������Ÿ������ Q001�� �̿��� ����Ʈ�� ����
        m_questTitle.text = PlayManager.QuestList[0].Title;
        m_questDescription.text = PlayManager.QuestList[0].Description;
        m_questRewards.text = $"���� {PlayManager.QuestList[0].Reward} {PlayManager.QuestList[0].RewardNum}��";

        m_btns = GetComponentsInChildren<Button>();
        m_btns[0].onClick.AddListener(AcceptQuest); // ���� ��ư
        m_btns[1].onClick.AddListener(DenyQuest);   // ���� ��ư
    }

    private void Awake()
    {
        SetComps();
    }
}
