using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSettingUI : BaseUI
{
    private OptionUIScript m_parent;
    public void SetParent(OptionUIScript _parent) { m_parent = _parent; }

    [SerializeField]
    private Button m_closeBtn;


    public override void UpdateUI()
    {
        
    }

    public override void CloseUI()
    {
        base.CloseUI();
        m_parent.PopupClosed();
    }



    private void SetBtns()
    {
        m_closeBtn.onClick.AddListener(CloseUI);
    }

    public override void SetComps()
    {
        base.SetComps();
        SetBtns();
    }
}
