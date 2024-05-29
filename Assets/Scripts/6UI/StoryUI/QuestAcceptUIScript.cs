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

    public void ShowNPCQuestUI()
    {
        gameObject.SetActive(true);
    }

    private void AcceptQuest()
    {
        Debug.Log("퀘스트 수락!");
        PlayManager.QuestScriptable[0].Status = EQuestStatus.ACCEPTED;  // 퀘스트 수락
        PlayManager.CurQuestList.Add(PlayManager.QuestScriptable[0]);   // 수락된 퀘스트는 CurQuestList에 추가
        PlayManager.ExpressCurQuestInfo();                              // 퀘스트 창에 표시
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

        // 프로토타입으로 Q001을 이용한 퀘스트를 구현
        m_questTitle.text = PlayManager.QuestScriptable[0].Title;
        m_questDescription.text = PlayManager.QuestScriptable[0].Description;
        m_questRewards.text = $"보상 {PlayManager.QuestScriptable[0].Reward} {PlayManager.QuestScriptable[0].RewardNum}개";

        m_btns = GetComponentsInChildren<Button>();
        m_btns[0].onClick.AddListener(AcceptQuest); // 수락 버튼
        m_btns[1].onClick.AddListener(DenyQuest);   // 거절 버튼
    }

    private void Awake()
    {
        SetComps();
    }
}
