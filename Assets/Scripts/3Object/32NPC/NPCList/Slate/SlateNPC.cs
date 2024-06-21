using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateNPC : NPCScript
{
    [SerializeField]
    private ESlateName m_slateName;

    public void SetSlate(uint _idx, NPCScriptable _scriptable)
    {
        m_slateName = (ESlateName)_idx;
        SetScriptable(_scriptable);
    }

    public override void NPCInteraction()
    {
        base.NPCInteraction();
        PlayManager.OpenSlateUI(this);
    }
}
