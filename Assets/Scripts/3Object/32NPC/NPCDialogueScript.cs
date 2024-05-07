using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogueScript : MonoBehaviour      // 기존에 만들어져 있던 OasisUIScript와 비슷해서 추후 통합 작업 진행할 듯
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
        PlayManager.StopPlayerInteract();                       // 이 두 줄은
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);  // m_npc의 nullexception 해결 이후 삭제 예정

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
        tmpText = "도적이 되고싶은 자는 나에게로...";
        m_btn = GetComponentInChildren<Button>();
        m_btn.onClick.AddListener(CloseUI);
    }

    private void Start()
    {
        SetComps();
        OpenUI();
    }
}
