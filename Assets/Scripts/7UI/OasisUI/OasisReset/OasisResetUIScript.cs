using UnityEngine;
using UnityEngine.UI;

public class OasisResetUIScript : OasisSubUI
{
    [SerializeField]
    private Button m_cancelBtn;

    private OasisStatResetUI m_statResetUI;
    private OasisTraitResetUI m_traitResetUI;

    public override void UpdateUI()
    {
        m_statResetUI.UpdateUI();
        m_traitResetUI.UpdateUI();
    }


    private void SetBtns()
    {
        m_cancelBtn.onClick.AddListener(CloseUI);
    }

    public override void SetComps()
    {
        base.SetComps();
        m_statResetUI = GetComponentInChildren<OasisStatResetUI>();
        m_statResetUI.SetParent(this); m_statResetUI.SetComps();
        m_traitResetUI = GetComponentInChildren<OasisTraitResetUI>();
        m_traitResetUI.SetParent(this); m_traitResetUI.SetComps();
        SetBtns();
    }
}
