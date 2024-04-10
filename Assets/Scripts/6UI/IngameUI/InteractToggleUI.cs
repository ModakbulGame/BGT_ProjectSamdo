using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractToggleUI : IngameTrackUIScript
{
    private TextMeshProUGUI m_infoTxt;

    public void SetInfoTxt(string _info)
    {
        m_infoTxt.text = _info;
    }

    private void Awake()
    {
        m_infoTxt = GetComponentInChildren<TextMeshProUGUI>();
    }
}
