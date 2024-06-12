using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCScript : NPCScript
{
    private bool m_isQuestStarted;
    private bool m_isQuestEnded;

    public bool IsQuestStarted { get { return m_isQuestStarted; } }
    public bool IsQuestEnded { get { return m_isQuestEnded; } }

    public void StartQuest() { m_isQuestStarted = true; }
    public void EndQuest() { m_isQuestEnded = false; }

    public override void StartInteract()
    {
        PlayManager.OpenNPCUI(this);
        base.StartInteract();

    }

    private void Start()
    {
        PlayManager.SetQuestStartObjectStatus(NPCName);
        Debug.Log($"{NPCName} quest status is {m_isQuestStarted}");
    }
}
