using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogueScript : MonoBehaviour      // ������ ������� �ִ� OasisUIScript�� ����ؼ� ���� ���� �۾� ������ ��
{
    public TextMeshProUGUI m_TypingText;
    private string tmpText;
    [SerializeField] 
    private float m_Speed = 0.2f;
   
    private Button m_btn;
    private bool Opened { get; set; }                    

    private bool ButtonClicked { get; set; }

    private NPCScript m_npc;
    public void SetNPC(NPCScript _npc)
    {
        m_npc = _npc;
    }

    public void OpenUI()
    {
        gameObject.SetActive(true);
        if (!Opened) { SetComps(); }

        ButtonClicked = false;
    }

    public void OpenUI(NPCScript _npc)
    {
        SetNPC(m_npc);
        OpenUI();
        StartCoroutine(Typing(tmpText));
    }

    public void CloseUI()
    {
        // m_npc.StopInteract();
        PlayManager.StopPlayerInteract();                       // �� �� ����
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);  // m_npc�� nullexception �ذ� ���� ���� ����

        gameObject.SetActive(false);
    }


    IEnumerator Typing(string _contents)
    {
        m_TypingText.text = null;
        for(int i = 0; i < _contents.Length; i++)
        {
            m_TypingText.text += _contents[i];
            yield return new WaitForSeconds(m_Speed);
        }
    }

    private void SetComps()
    {
        tmpText = "������ �ǰ���� �ڴ� �����Է�...";
        m_btn = GetComponentInChildren<Button>();
        m_btn.onClick.AddListener(CloseUI);
    }

    private void Start()
    {
        SetComps();
        OpenUI();
    }
}
