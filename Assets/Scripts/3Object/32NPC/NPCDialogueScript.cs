using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NPCDialogueScript : MonoBehaviour      // 기존에 만들어져 있던 OasisUIScript와 비슷해서 추후 통합 작업 진행할 듯
{
    [SerializeField]
    private TextMeshProUGUI m_TypingText;
    private string m_tmpText;
    [SerializeField]
    private float m_Speed = 0.2f;

    private Button[] m_btn;
    private EventTrigger m_trigger;

    private bool Opened { get; set; }
    private bool ButtonClicked { get; set; }

    private NPCScript m_npc;
    private string[] m_dialogues;
    private int m_dialogueCount;        

    public void SetNPC(NPCScript _npc)
    {
        m_npc = _npc;
        m_dialogues = _npc.m_NPCDialogue;
    }

    public void OpenUI()
    {
        gameObject.SetActive(true);
        if (!Opened) { SetComps(); }
        ButtonClicked = false;
    }

    public void OpenUI(NPCScript _npc)
    {
        SetNPC(_npc);
        OpenUI();
        StartCoroutine(Typing(m_dialogues[m_dialogueCount]));
    }

    public void CloseUI()
    {
        m_npc.StopInteract();
        gameObject.SetActive(false);
    }


    IEnumerator Typing(string _contents)
    {
        m_TypingText.text = null;

        for (int i = 0; i < _contents.Length; i++)
        {
            m_TypingText.text += _contents[i];
            if (ButtonClicked)
            {
                m_TypingText.text = _contents;
                ButtonClicked = false;
                break;
            }
            yield return new WaitForSeconds(m_Speed);
        }
    }

    private void ShowAllDialogue(PointerEventData _data)
    {
        if (_data.button == PointerEventData.InputButton.Left)
        {
            ButtonClicked = true;
            StopCoroutine(Typing(m_dialogues[m_dialogueCount]));
        }
    }

    private void NextDialogue()
    {
        StopCoroutine(m_dialogues[m_dialogueCount]);
        m_TypingText.text = "";
        if (m_dialogueCount < m_npc.m_NPCDialogue.Length)
        {
            m_dialogueCount++;
            StartCoroutine(Typing(m_dialogues[m_dialogueCount]));
        }

    }

    private void SetComps()
    {
        m_dialogueCount = 0;
        m_trigger = GetComponent<EventTrigger>();
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerClick, ShowAllDialogue);
        m_btn = GetComponentsInChildren<Button>();
        m_btn[0].onClick.AddListener(NextDialogue);
        m_btn[1].onClick.AddListener(CloseUI);
    }

    private void Start()
    {
        SetComps();
        OpenUI();
    }
}
