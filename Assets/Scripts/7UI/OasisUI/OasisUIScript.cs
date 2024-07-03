using UnityEngine;
using UnityEngine.UI;

public enum EOasisFunctionName
{
    REST,
    TRANSPORT,
    TRADE,
    RESET,
    LAST
}

public class OasisUIScript : BaseUI
{
    private OasisNPC m_oasis;
    public void SetNPC(OasisNPC _oasis) { m_oasis = _oasis; }
    public OasisNPC Oasis { get { return m_oasis; } }

    [SerializeField]
    private Button m_closeBtn;
    [SerializeField]
    private Button[] m_functionBtns;
    [SerializeField]
    private OasisSubUI[] m_uis = new OasisSubUI[(int)EOasisFunctionName.LAST];


    private bool IsButtonClicked { get; set; }


    public void OpenUI(OasisNPC _npc)
    {
        base.OpenUI();
        IsButtonClicked = false;
        SetNPC(_npc);
        GameManager.UIControlInputs.CloseUI.started += delegate { CloseUI(); };
    }

    public override void CloseUI()                       // 닫기
    {
        if (IsButtonClicked) { return; }
        GameManager.UIControlInputs.CloseUI.started -= delegate { CloseUI(); };
        m_oasis.StopInteract();
        base.CloseUI();
    } 


    private void ClickButton(EOasisFunctionName _function)
    {
        if(IsButtonClicked) { return; }

        m_uis[(int)_function].OpenUI();
        IsButtonClicked = true;
    }
    public void FunctionDone()
    {
        IsButtonClicked = false;
    }


    public override void SetComps()
    {
        base.SetComps();
        foreach(OasisSubUI ui in m_uis) { ui.SetParent(this); }
        SetBtns();
    }
    private void SetBtns()
    {
        for (int i = 0; i<(int)EOasisFunctionName.LAST; i++)
        {
            EOasisFunctionName function = (EOasisFunctionName)i;
            m_functionBtns[i].onClick.AddListener(delegate { ClickButton(function); });
        }
        m_closeBtn.onClick.AddListener(CloseUI);
    }
}
