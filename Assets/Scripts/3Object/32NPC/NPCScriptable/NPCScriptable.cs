using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCScriptable : ScriptableObject
{
    public uint Idx;
    public SNPC NPC;
    public string NPCName;
    public string[] Dialogues;
    public List<QuestScriptable> QuestList;

    public void AddQuest(QuestScriptable _quest) { QuestList.Add(_quest); }

    public virtual void SetNPCScriptable(uint _idx, string[] _data)
    {
        Idx =       _idx;     
        NPC =       StoryManager.String2NPC(_data[(int)ENPCAttribute.SNPC]);
        NPCName =   _data[(int)ENPCAttribute.NAME];

        Dialogues = new string[_data.Length - (int)ENPCAttribute.DIALOGUES];
        for(int i = (int)ENPCAttribute.DIALOGUES; i < _data.Length; i++)
        {
            int idx = i - (int)ENPCAttribute.DIALOGUES;
            string line = _data[i];
            if (line == "") { Dialogues = Dialogues.Take(idx).ToArray(); break; }
            Dialogues[idx] = _data[i];
        }

        QuestList = new();
    }
}
