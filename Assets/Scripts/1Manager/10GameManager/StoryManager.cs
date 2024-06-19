using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
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
    private NPCScriptable[] m_npcData;
    [SerializeField]
    private DialogueScriptable[] m_dialogueData;
    [SerializeField]
    private QuestScriptable[] m_questData;

    private Dictionary<SNPC, List<DialogueScriptable>> m_dialogueDictionary;

    public DialogueScriptable GetDialogueData(SNPC _npc, int _idx) { return m_dialogueDictionary[_npc][_idx]; }
    public DialogueScriptable GetDialogueData(EDialogueName _dial) { return m_dialogueData[(int)_dial]; }
    public QuestScriptable GetQuestData(EQuestName _quest) { return m_questData[(int)_quest]; }

    public void SetNPCData(List<NPCScriptable> _npc)
    {
        m_npcData = new NPCScriptable[_npc.Count];
        for (int i = 0; i<_npc.Count; i++) { m_npcData[i] = _npc[i]; }
    }
    public void SetDialogueData(List<DialogueScriptable> _dial)
    {
        m_dialogueData = new DialogueScriptable[_dial.Count];
        for (int i = 0; i<_dial.Count; i++) { m_dialogueData[i] = _dial[i]; }
    }
    public void SetQuestData(List<QuestScriptable> _quest)
    {
        m_questData = new QuestScriptable[_quest.Count];
        for(int i = 0; i<_quest.Count; i++) { m_questData[i] = _quest[i]; }
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
                Debug.Log("���� ���� ����");
                break;
        }
        return null;
    }


    public static EQuestName ID2Quest(string _id)
    {
        if(_id == "") { return EQuestName.LAST; }
        int.TryParse(_id[1..], out int idx);
        return (EQuestName)(idx-1);
    }
    public static SNPC String2NPC(string _data)     // �������� 9������ ����
    {
        if(_data == "") { return SNPC.Null; }
        int stringLen = _data.Length - 2;
        string type = _data[..(stringLen)];
        int.TryParse(_data[(stringLen)..], out int num);
        int idx = num-1;
        return type switch
        {
            "OASIS" => new(ENPCType.OASIS, idx),
            "ALTAR" => new(ENPCType.ALTAR, idx),
            "SLATE" => new(ENPCType.SLATE, idx),
            "OTHER" => new(ENPCType.OTHER, idx),

            _ => SNPC.Null
        };
    }


    private void CreateDialogueDictionary()
    {
        m_dialogueDictionary = new();
        foreach (DialogueScriptable dial in m_dialogueData)
        {
            SNPC npc = dial.NPC;
            if (!m_dialogueDictionary.ContainsKey(dial.NPC)) { m_dialogueDictionary[npc] = new(); }
            m_dialogueDictionary[npc].Add(dial);
        }
    }

    public void SetManager()
    {
        CreateDialogueDictionary();
    }
}
