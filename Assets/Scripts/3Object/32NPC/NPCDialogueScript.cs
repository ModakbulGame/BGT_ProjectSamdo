using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class NPCDialogueScript : BaseUI
{
    private InputSystem.UIControlActions UIInput { get { return GameManager.UIControlInputs; } }    // Input System Player 입력

    [SerializeField]
    private TextMeshProUGUI m_nameText;
    [SerializeField]
    private TextMeshProUGUI m_typingText;
    [SerializeField]
    private float m_textSpeed = 0.1f;

    private Button m_btn;

    private NPCScript CurNPC { get; set; }
    private string CurNPCName { get { return CurNPC.NPCName; } }
    private DialogueScriptable CurDialogue { get; set; }
    private DialLine[] CurLines { get { if (CurDialogue == null) { return CurNPC.DefaultLine; } return CurDialogue.Lines; } }
    private int DialogueCount { get; set; } = 0;

    private bool IsQuestConfirmed { get; set; }
    private bool IsButtonClicked { get; set; }


    public override void OpenUI()
    {
        base.OpenUI();
        IsButtonClicked = false;
        DialogueCount = 0;
        UIInput.UIInteract.performed += NextDialogue;
    }

    public void OpenUI(NPCScript _npc, int _idx)
    {
        OpenUI();
        SetNPC(_npc);
        CurDialogue = _idx >= 0 ? _npc.DialogueList[_idx] : null;
        StartCoroutine(ProcLine(CurLines[DialogueCount]));
    }

    private void SetNPC(NPCScript _npc)
    {
        CurNPC = _npc;

        m_nameText.text = CurNPCName;
    }


    IEnumerator ProcLine(DialLine _line)
    {
        m_typingText.text = "";

        string txt = _line.Text;

        for (int i = 0; i < txt.Length; i++)
        {
            m_typingText.text += txt[i];
            yield return new WaitForSeconds(m_textSpeed);

            if (IsButtonClicked)                          // 좌클릭하면 전체 내용 한 번에 출력
            {
                m_typingText.text = txt;
                IsButtonClicked = false;
                break;
            }
        }
        IsQuestConfirmed = !_line.HasQuest;
        if (_line.HasQuest)
        {
            DialQuest quest = _line.TriggerQuest;
            PlayManager.ShowNPCQuestUI(quest.Quest, true, ConfirmQuest);
        }
        bool isQuestConfirmed = !_line.HasQuest;
        while (!isQuestConfirmed)
        {
            yield return null;
        }
        while (!IsButtonClicked) { yield return null; }
        NextDialogue();
    }
    private void ConfirmQuest()
    {
        IsQuestConfirmed = true;
    }

    public void ShowAllDialogue()
    {
        IsButtonClicked = true;
    }

    public void NextDialogue(InputAction.CallbackContext _action)                     // 다음 대사 출력
    {
        NextDialogue();
    }
    private void NextDialogue()
    {
        DialogueCount++;
        if (DialogueCount == CurLines.Length) { CloseUI(); return; }

        IsButtonClicked = false;
        StartCoroutine(ProcLine(CurLines[DialogueCount]));
    }

    public override void CloseUI()
    {
        CurNPC.StopInteract();
        UIInput.UIInteract.performed -= NextDialogue;
        base.CloseUI();
    }


    public override void SetComps()
    {
        base.SetComps();
        m_btn = GetComponentInChildren<Button>();
        m_btn.onClick.AddListener(ShowAllDialogue);
    }
}