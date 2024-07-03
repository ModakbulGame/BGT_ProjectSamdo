using UnityEngine.UI;

public class OasisRestUIScript : OasisSubUI
{
    public override void OpenUI()
    {
        base.OpenUI();
        GameManager.UIControlInputs.UIConfirm.started += delegate { RestInPeace(); };
    }


    private void RestInPeace()
    {
        PlayManager.RestAtPoint(m_parent.Oasis);
        CloseUI();
        m_parent.CloseUI();
    }

    private void CancelUI()
    {
        CloseUI();
    }

    public override void CloseUI()
    {
        GameManager.UIControlInputs.UIConfirm.started -= delegate { RestInPeace(); };
        base.CloseUI();
    }


    public override void SetComps()
    {
        base.SetComps();
        Button[] btns = GetComponentsInChildren<Button>();
        btns[0].onClick.AddListener(RestInPeace);
        btns[1].onClick.AddListener(CancelUI);
    }
}
