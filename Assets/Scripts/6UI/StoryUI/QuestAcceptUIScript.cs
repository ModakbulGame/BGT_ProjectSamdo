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

    public List<QuestScriptable> m_availableQuest = new List<QuestScriptable>();
    public List<QuestScriptable> m_activeQuest = new List<QuestScriptable>();

    // 퀘스트 정보
    [SerializeField]
    private TextMeshProUGUI m_questTitle;
    [SerializeField]
    private TextMeshProUGUI m_questDescription;
    [SerializeField]
    private TextMeshProUGUI m_questRewards;

    public void CheckQuests(QuestObject _obj)
    {
    

    }

    public void ShowNPCQuestUI()
    {
        gameObject.SetActive(true);
    }

    private void AcceptQuest()
    {
        Debug.Log("퀘스트 수락!");
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

        m_btns = GetComponentsInChildren<Button>();
        m_btns[0].onClick.AddListener(AcceptQuest); // 수락 버튼
        m_btns[1].onClick.AddListener(DenyQuest);   // 거절 버튼
    }

    private void Start()
    {
        SetComps();
    }
}
