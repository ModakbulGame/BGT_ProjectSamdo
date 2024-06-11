using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCScript : NPCScript
{
    [SerializeField]
    private bool m_isQuestStarted;  // 시트에 따라 퀘스트가 존재하는 npc인 경우에는 내부처리로 조정 예정
    [SerializeField]
    private bool m_isQuestEnded;    // 얘도

    public bool IsQuestStarted { get { return m_isQuestStarted; } }
    public bool IsQuestEnded { get { return m_isQuestEnded; } }

    public void StartQuest() { m_isQuestStarted = true; }
    public void EndQuest() { m_isQuestEnded = false; }

    public override void StartInteract()
    {
        PlayManager.OpenNPCUI(this);
        base.StartInteract();
    }
}
