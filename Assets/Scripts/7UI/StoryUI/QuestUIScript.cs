using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIScript : MonoBehaviour
{
    private bool m_isQuestUIToggle = false;

    public void ToggleQuestUI()
    {
        if (!m_isQuestUIToggle)
        {
            gameObject.SetActive(true);
            m_isQuestUIToggle = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            gameObject.SetActive(false);
            m_isQuestUIToggle = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
