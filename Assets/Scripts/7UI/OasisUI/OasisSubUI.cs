using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OasisSubUI : BaseUI
{
    protected OasisUIScript m_parent;
    public void SetParent(OasisUIScript _parent) { m_parent = _parent; }


    public override void OpenUI()
    {
        base.OpenUI();
        GameManager.UIControlInputs.CloseUI.started += delegate { CloseUI(); };
    }
    public override void CloseUI()
    {
        GameManager.UIControlInputs.CloseUI.started -= delegate { CloseUI(); };
        m_parent.FunctionDone();
        base.CloseUI();
    }
}
