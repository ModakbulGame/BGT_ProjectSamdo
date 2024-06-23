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

    private VolumeCtrlScript[] m_volumes;

    public override void UpdateUI()
    {
        
    }

    public override void CloseUI()
    {
        base.CloseUI();
        m_parent.PopupClosed();
    }

    // 사운드
    public void SetBGMPoint(int _vol)
    {
        // m_volumes[0].SetPoint(_vol);
    }
    public void SetSEPoint(int _vol)
    {
        // m_volumes[1].SetPoint(_vol);
    }
    public void InitValues()
    {
        SetBGMPoint(GameManager.BGMVolume);
        SetSEPoint(GameManager.SEVolume);
    }
    public void SetVolume(EVolumeType _type, int _vol)
    {
        if (_type == EVolumeType.BGM)
            GameManager.SetBGMVolume(_vol);
        else if (_type == EVolumeType.SE)
            GameManager.SetSEVolume(_vol);
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
