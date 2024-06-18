using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarNPC : QuestNPCScript
{
    [SerializeField]
    private EAltarName m_altarName;

    public void SetAltar(uint _idx, NPCScriptable _scriptable)
    {
        m_altarName = (EAltarName)_idx;
        SetScriptable(_scriptable);
    }


    public override void StartInteract()
    {
        // PlayManager.OpenAlterUI(this);
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
    }
}
