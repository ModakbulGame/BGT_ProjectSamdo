using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using MalbersAnimations;

public class NPCDialogueScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_nameText;
    [SerializeField]
    private TextMeshProUGUI m_typingText;
    [SerializeField]
    private float m_textSpeed = 0.1f;

    private Button m_btn;

    private bool IsOpened { get; set; }
    public bool IsDialogueOpened { get; private set; }
    private bool ButtonClicked { get; set; }

    private QuestNPCScript CurNPC { get; set; }
    private string CurNPCName { get; set; }
    private string[] CurDialogue { get; set; }
    private int DialogueCount { get; set; } = 0;

    private bool m_isQuestStarted;
    private bool m_isQuestEnded;
    private int idx = 0;

    public void SetNPC(QuestNPCScript _npc)
    {
        CurNPC = _npc;

        CurNPCName = _npc.NPCName;
        CurDialogue = _npc.NPCDialogues;
        m_isQuestStarted = _npc.IsQuestStarted;
        m_nameText.text = CurNPCName;

        if (PlayManager.CheckQuestCompleted("Q002"))
        {
            PlayManager.SetQuestEndObjectStatus("Q002");
            m_isQuestStarted = false;
            m_isQuestEnded = true;
        }
        Debug.Log($"start is {m_isQuestStarted}, end is {m_isQuestEnded}");
    }

    public void OpenUI()
    {
        gameObject.SetActive(true);
        IsDialogueOpened = true;
        if (!IsOpened) { SetComps(); }
        ButtonClicked = false;
        DialogueCount = 0;
    }

    public void OpenUI(QuestNPCScript _npc)
    {
        Debug.Log(m_isQuestEnded);
        SetNPC(_npc);
        OpenUI();
        StartCoroutine(Typing(CurDialogue[DialogueCount]));
    }

    public void CloseUI()
    {
        CurNPC.StopInteract();
        gameObject.SetActive(false);
        IsDialogueOpened = false;
    }

    IEnumerator Typing(string _contents)
    {
        m_typingText.text = "";

        for (int i = 0; i < _contents.Length; i++)
        {
            m_typingText.text += _contents[i];
            yield return new WaitForSeconds(m_textSpeed);

            if (ButtonClicked)                          // 좌클릭하면 전체 내용 한 번에 출력
            {
                m_typingText.text = _contents;
                ButtonClicked = false;
                break;
            }
        }
        while (!ButtonClicked) { yield return null; }
        NextDialogue();
    }

    public void ShowAllDialogue()
    {
        ButtonClicked = true;
    }

    public void NextDialogue()                     // 다음 대사 출력
    {
        DialogueCount++;
        if (DialogueCount < CurDialogue.Length)
        {
            ButtonClicked = false;
            StartCoroutine(Typing(CurDialogue[DialogueCount]));
            return;
        }
        if (m_isQuestStarted && !m_isQuestEnded)
        {
            PlayManager.ShowNPCQuestUI(CurNPC);
        }
        else if (m_isQuestEnded && !m_isQuestStarted)
        {
            PlayManager.ChangeBtnsTxt();
            PlayManager.ShowNPCQuestUI(CurNPC);
        }
        else CloseUI();
    }

    private void SetComps()
    {
        m_btn = GetComponentInChildren<Button>(); // 닫기 버튼\
        m_btn.onClick.AddListener(ShowAllDialogue);
    }
}