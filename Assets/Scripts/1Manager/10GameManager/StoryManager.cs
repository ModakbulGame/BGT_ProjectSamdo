using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SNPC
{
    public ENPCType Type;
    public int Idx;
    public bool IsNull { get { return Type == ENPCType.LAST; } }
    public static SNPC Null { get { return new(ENPCType.LAST, -1); } }
    public SNPC(ENPCType _type, int _idx) { Type =_type; Idx = _idx; }

    public static bool operator ==(SNPC _npc1, SNPC _npc2) { return _npc1.Type == _npc2.Type && _npc1.Idx == _npc2.Idx; }
    public static bool operator !=(SNPC _npc1, SNPC _npc2) { return !(_npc1 == _npc2); }

    public readonly override bool Equals(object _obj)
    {
        return _obj is SNPC sNPC&&
               Type==sNPC.Type&&
               Idx==sNPC.Idx;
    }
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Type, Idx);
    }
}
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
                return m_npcData[_npc.Idx + (int)EOasisName.LAST];
            case ENPCType.OTHER:
                Debug.Log("아직 없는 유형");
                break;
        }
        return null;
    }

    public static SNPC String2NPC(string _data)     // 종류별로 9개씩만 가능
    {
        if(_data == "") { return SNPC.Null; }
        int stringLen = _data.Length - 2;
        string type = _data[..(stringLen)];
        int.TryParse(_data[(stringLen)..], out int idx);
        return type switch
        {
            "OASIS" => new(ENPCType.OASIS, idx),
            "ALTAR" => new(ENPCType.ALTAR, idx),
            "SLATE" => new(ENPCType.SLATE, idx),
            "OTHER" => new(ENPCType.OTHER, idx),

            _ => SNPC.Null
        };
    }
}
