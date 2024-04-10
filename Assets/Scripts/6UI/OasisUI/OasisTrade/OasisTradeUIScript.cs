using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OasisTradeUIScript : MonoBehaviour, IOasisUI
{
    private OasisUIScript m_parent;
    public void OpenUI(OasisUIScript _parent) { m_parent = _parent; SetComps(); }


    private void CancelUI()
    {
        CloseUI();
    }

    public void CloseUI()
    {
        m_parent.FunctionDone();
        Destroy(gameObject);
    }


    private void SetComps()
    {
        Button[] btns = GetComponentsInChildren<Button>();
        btns[0].onClick.AddListener(CancelUI);
    }
}
