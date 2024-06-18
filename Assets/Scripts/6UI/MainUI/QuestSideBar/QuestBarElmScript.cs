using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestBarElmScript : MonoBehaviour
{
    private TextMeshProUGUI m_nameTxt;

    public void SetElm(QuestInfo _info)
    {
        if (!gameObject.activeSelf) { gameObject.SetActive(true); }

    }

    public void HideElm()
    {
        gameObject.SetActive(false);
    }


    public void SetComps()
    {

    }
}
