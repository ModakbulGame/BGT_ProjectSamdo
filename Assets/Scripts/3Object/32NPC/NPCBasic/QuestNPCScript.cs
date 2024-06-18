using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCScript : NPCScript
{
    private bool m_isQuestStarted;
    private bool m_isQuestEnded;

    public List<QuestScriptable> NPCQuestList;

    public bool IsQuestStarted { get { return m_isQuestStarted; } }
    public bool IsQuestEnded { get { return m_isQuestEnded; } }

    public void StartQuest() { m_isQuestStarted = true; }
    public void EndQuest() { m_isQuestEnded = true; }

    public override void StartInteract()
    {
        PlayManager.OpenNPCUI(this);
        base.StartInteract();

    }

    public void Start()
    {
        for (int i = 0; i < PlayManager.QuestList.Count; i++)
        {
            if (PlayManager.QuestList[i].StartObject == NPCName)
            {
                NPCQuestList.Add(PlayManager.QuestList[i]);
                Debug.Log($"{NPCName} has {PlayManager.QuestList[i]}");
            }
        }
        PlayManager.SetQuestStartObjectStatus(NPCName);
        Debug.Log($"{NPCName} is {m_isQuestStarted}");
        
    }
}
