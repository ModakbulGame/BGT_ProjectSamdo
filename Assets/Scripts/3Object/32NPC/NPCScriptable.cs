using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScriptable : ScriptableObject
{
    public uint Idx;
    public string ID;
    public string NPCName;
    public List<string> Dialogues;

    public void SetNPCScriptable(uint _idx, string[] _data, List<string> _dialogue)
    {
        Idx = _idx;     
        ID = _data[(int)EnpcAttribute.ID];
        NPCName = _data[(int)EnpcAttribute.NAME];

        Dialogues = new List<string>();
        Dialogues.AddRange(_data);
    }
}
