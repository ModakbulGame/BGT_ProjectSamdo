using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIScript : MonoBehaviour
{
    public bool QuestAvailable { get; set; }
    public bool QuestRunning { get; set; }

    private bool m_isQuestUIToggle = false;
    private bool m_isQuestLogActive = false;

    public GameObject m_questPanel;
    public GameObject m_questLogPanel;

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

    public void SetComps()
    {

    }

    private void Start()
    {
        SetComps();
    }
}
