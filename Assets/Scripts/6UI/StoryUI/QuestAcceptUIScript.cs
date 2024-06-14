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

    TextMeshProUGUI m_firstBtnTxt;
    TextMeshProUGUI m_secondBtnTxt;

    public void ShowNPCQuestUI()
    {
        gameObject.SetActive(true);
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

            PlayManager.AcceptQuest("Q002");
            PlayManager.ExpressCurQuestInfo();  // 퀘스트 창에 표시
        }
        else PlayManager.CompleteQuest("Q002"); // 완료 버튼; 퀘스트 클리어
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
        m_questRewards.text = $"보상 {PlayManager.QuestList[1].Reward} {PlayManager.QuestList[0].RewardNum}개";

        m_btns[0].onClick.AddListener(AcceptQuest);   // 수락, 완료 버튼
        m_btns[1].onClick.AddListener(DenyQuest);     // 거절, 확인 버튼
    }

    private void Awake()
    {
        SetComps();
    }
}
