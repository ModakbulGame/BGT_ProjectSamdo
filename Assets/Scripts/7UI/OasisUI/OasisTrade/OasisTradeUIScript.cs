using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OasisTradeUIScript : BaseUI, IOasisUI
{
    private OasisUIScript m_parent;

    [SerializeField]
    private Button m_closeBtn;

    public void OpenUI(OasisUIScript _parent)
    {
        base.OpenUI();
        if(m_parent == null) { m_parent = _parent; }
    }


    private void CancelUI()
    {
        CloseUI();
    }

    public override void CloseUI()
    {
        m_parent.FunctionDone();
        base.CloseUI();
    }


    public override void SetComps()
    {
        base.SetComps();
        m_closeBtn.onClick.AddListener(CancelUI);

    }
}
