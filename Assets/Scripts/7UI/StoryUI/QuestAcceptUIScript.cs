using MalbersAnimations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestAcceptUIScript : BaseUI
{
    // ����Ʈ ����
    [SerializeField]
    private TextMeshProUGUI m_questTitle;
    [SerializeField]
    private TextMeshProUGUI m_questDescription;
    [SerializeField]
    private TextMeshProUGUI m_questRewards;

    private Button m_btn1;
    private Button m_btn2;

    private TextMeshProUGUI m_btn1Txt;       // ����, �Ϸ� ��ư
    private TextMeshProUGUI m_btn2Txt;      // ����, Ȯ�� ��ư

    private QuestScriptable CurQuest { get; set; }
    private bool IsStart { get; set; }
    private FPointer ConfirmFunction { get; set; }

    public void ShowNPCQuestUI(EQuestName _quest, bool _isStart, FPointer _confirm)
    {
        CurQuest = GameManager.GetQeustData(_quest);
        IsStart = _isStart;
        ConfirmFunction = _confirm;
        base.OpenUI();
    }

    public override void UpdateUI()
    {
        m_questTitle.text = CurQuest.Name;
        m_questDescription.text = CurQuest.Description;
        m_questRewards.text = $"���� {CurQuest.Reward.Type} {CurQuest.Reward.Amount}��";
        SetBtnsTxts(IsStart);
    }


    public void SetBtnsTxts(bool _isStart)
    {
        m_btn1Txt.text = _isStart ? "����" : "�Ϸ�";
        m_btn2Txt.text = _isStart ? "����" : "Ȯ��";
    }

    private void Btn1Function()
    {
        if (IsStart)
        {
            AcceptQuest();
        }
        else
        {
            FinishQuest();
        }
        ConfirmFunction();
        CloseUI();
    }
    private void AcceptQuest()
    {
        PlayManager.SetQuestStatus(CurQuest.Enum, EQuestState.ACCEPTED);
        PlayManager.UpdateQuestSidebar();
    }
    private void FinishQuest()
    {
        PlayManager.SetQuestStatus(CurQuest.Enum, EQuestState.FINISH);
        PlayManager.UpdateQuestSidebar();
    }

    private void Btn2Function()
    {
        ConfirmFunction();
        CloseUI();
    }


    private void SetBtns()
    {
        m_btn1.onClick.AddListener(Btn1Function);       // ����, �Ϸ� ��ư
        m_btn2.onClick.AddListener(Btn2Function);       // ����, Ȯ�� ��ư
    }

    public override void SetComps()
    {
        base.SetComps();
        Button[] btns = GetComponentsInChildren<Button>();
        m_btn1 = btns[0]; m_btn2 = btns[1];
        m_btn1Txt = m_btn1.GetComponentInChildren<TextMeshProUGUI>();
        m_btn2Txt = m_btn2.GetComponentInChildren<TextMeshProUGUI>();
        SetBtns();
    }
}
