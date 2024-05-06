using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class NPCDialogueScript : MonoBehaviour
{
    public TextMeshProUGUI m_TypingText;
    public string m_Message;
    [SerializeField] 
    private float m_Speed = 0.2f;

    private EventTrigger m_trigger;

    IEnumerator Typing(TextMeshProUGUI _typingText, string _message, float _speed)
    {
        for (int i = 0; i < _message.Length; i++)
        {
            _typingText.text = _message.Substring(0, i + 1);
            yield return new WaitForSeconds(_speed);
        }
    }

    private void ShowContents()
    {
        
    }

    private void Start()
    {
        m_Message = @"¾È³ç Èû¼¼°í °­ÇÑ Ä£±¸ ³ª´Â ¿Ðµµ.";

        StartCoroutine(Typing(m_TypingText, m_Message, m_Speed));
        m_trigger = GetComponent<EventTrigger>();
    }
}
