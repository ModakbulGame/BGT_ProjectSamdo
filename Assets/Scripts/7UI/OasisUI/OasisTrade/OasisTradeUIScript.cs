using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OasisTradeUIScript : OasisSubUI
{
    private OasisProducts m_productList;
    private PatternRegisterScript m_patternRegister;
    private OasisSoulInfoScript m_soulInfo;


    private OasisNPC Oasis { get { return m_parent.Oasis; } }

    [SerializeField]
    private Button m_closeBtn;

    public override void UpdateUI()
    {
        m_productList.UpdateUI(Oasis);
        m_patternRegister.UpdateUI();
        m_soulInfo.UpdateUI();
    }

    private void SetBtns()
    {
        m_closeBtn.onClick.AddListener(CloseUI);
    }

    public override void SetComps()
    {
        base.SetComps();
        m_productList = GetComponentInChildren<OasisProducts>();
        m_productList.SetParent(this); m_productList.SetComps();
        m_patternRegister = GetComponentInChildren<PatternRegisterScript>();
        m_patternRegister.SetComps();
        m_soulInfo = GetComponentInChildren<OasisSoulInfoScript>();

        SetBtns();
    }
}
