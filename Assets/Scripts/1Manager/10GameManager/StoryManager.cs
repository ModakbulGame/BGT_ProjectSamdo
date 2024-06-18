using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField]
    private QuestScriptable[] m_questData;
    [SerializeField]
    private NPCScriptable[] m_npcData;

    public QuestScriptable GetQuestData(EQuestEnum _quest) { return m_questData[(int)_quest]; }
    public NPCScriptable GetNPCData(EnpcEnum _npc) { return m_npcData[(int)_npc]; }

    public void SetQuestData(List<QuestScriptable> _quest)
    {
        m_questData = new QuestScriptable[_quest.Count];
        for(int i = 0; i<_quest.Count; i++) { m_questData[i] = _quest[i]; }
    }
    public void SetNPCData(List<NPCScriptable> _npc)
    {
        m_npcData = new NPCScriptable[_npc.Count];
        for (int i = 0; i<_npc.Count; i++) { m_npcData[i] = _npc[i]; }
    }
}
