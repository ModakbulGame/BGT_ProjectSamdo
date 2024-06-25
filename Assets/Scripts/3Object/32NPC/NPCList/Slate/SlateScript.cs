using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateScript : NPCScript
{
    [SerializeField]
    private ESlateName m_slateName;

    [SerializeField]
    private SlateUIScript m_slateUI;

    [SerializeField]
    private float m_displayTime = 5;
    public float DisplayTime { get { return m_displayTime; } }

    public string SlateText { get { return m_scriptable.DefaultLine; } }

    private bool ShowingSlateText { get; set; }

    public override string InfoTxt => "읽기";

    public void SetSlate(uint _idx, NPCScriptable _scriptable)
    {
        m_slateName = (ESlateName)_idx;
        SetScriptable(_scriptable);
    }

    public override bool CanInteract => !ShowingSlateText;

    public override void StartInteract()
    {
        PlayManager.StopPlayerInteract();
        if(m_scriptable == null || ShowingSlateText) { return; }
        ShowSlateText();
        ShowingSlateText = true;
    }

    private void ShowSlateText()
    {
        m_slateUI.ShowSlateText(this);
    }

    public void DisplayDone()
    {
        PlayManager.StopPlayerInteract(InteractManager);
        ShowingSlateText = false;
    }
}
