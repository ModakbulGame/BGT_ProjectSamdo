using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIScript : BaseUI
{

    public override void OpenUI()
    {
        base.OpenUI();
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
    }

    public override void UpdateUI()
    {

    }

    public override void CloseUI()
    {
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
        base.CloseUI();
    }

    public override void SetComps()
    {
        base.SetComps();
    }
}
