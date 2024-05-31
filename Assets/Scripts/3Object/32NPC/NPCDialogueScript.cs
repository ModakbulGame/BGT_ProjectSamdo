using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NPCDialogueScript : MonoBehaviour
{ 
    [SerializeField]
    private TextMeshProUGUI m_typingText;
    [SerializeField]
    private float m_textSpeed = 0.1f;

    private Button m_btn;

    private bool IsOpened { get; set; }
    public bool IsDialogueOpened { get; private set; }
    private bool ButtonClicked { get; set; }

    private NPCScript CurNPC { get; set; }
    private string[] CurDialogue { get; set; }
    private int DialogueCount { get; set; } = 0;
    
    private bool IsQuestExisted;

    public void SetNPC(NPCScript _npc)
    {
        CurNPC = _npc;
        CurDialogue = _npc.m_npcDialogue;
        IsQuestExisted = _npc.IsQuestExisted;
    }

    public void OpenUI()
    {
        gameObject.SetActive(true);
        IsDialogueOpened = true;
        if (!IsOpened) { SetComps(); }
        ButtonClicked = false;
        DialogueCount = 0;
    }

    public void OpenUI(NPCScript _npc)
    {
        OpenUI();
        SetNPC(_npc);
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
        if(IsQuestExisted)
        {
            PlayManager.ShowNPCQuestUI();
        }
        else CloseUI();
    }

    private void SetComps()
    {
        m_btn = GetComponentInChildren<Button>(); // 닫기 버튼\
        m_btn.onClick.AddListener(ShowAllDialogue);
    }
}