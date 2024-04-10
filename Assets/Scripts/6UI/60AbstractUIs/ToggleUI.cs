using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_toggleUI;

    public void SetToggle()
    {
        if (m_toggleUI == null) { Debug.LogError("UI not exists"); return; }

        m_toggleUI.SetActive(!m_toggleUI.activeSelf);
    }
}
