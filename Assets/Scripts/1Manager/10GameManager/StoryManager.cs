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

    public NPCScriptable GetNPCData(SNPC _npc)
    {
        switch (_npc.Type)
        {
            case ENPCType.OASIS:
                return m_npcData[_npc.Idx];
            case ENPCType.ALTAR:
                return m_npcData[_npc.Idx + (int)EOasisPointName.LAST];
            case ENPCType.OTHER:
                Debug.Log("아직 없는 유형");
                break;
        }
        return null;
    }

    public static SNPC String2NPC(string _data)     // 종류별로 9개씩만 가능
    {
        if(_data == "") { return SNPC.Null; }
        string type = _data[..(_data.Length-1)];
        int idx = _data[_data.Length-1] - '1';
        return type switch
        {
            "OASIS" => new(ENPCType.OASIS, idx),
            "ALTAR" => new(ENPCType.ALTAR, idx),
            "OTHER" => new(ENPCType.OTHER, idx),

            _ => SNPC.Null
        };
    }
}
