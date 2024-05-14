using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIScript : MonoBehaviour
{
    private bool m_isMapUIToggle = false;

    public void ToggleQuestUI()
    {
        if (!m_isMapUIToggle)
        {
            gameObject.SetActive(true);
            m_isMapUIToggle = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            gameObject.SetActive(false);
            m_isMapUIToggle = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Start()
    {

    }
}
