using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScriptable : ScriptableObject
{
    public uint Idx;
    public string ID;
    public string NPCName;
    public string[] Dialogues;

    public void SetNPCScriptable(uint _idx, string[] _data)
    {
        Idx = _idx;     
        ID = _data[(int)EnpcAttribute.ID];
        NPCName = _data[(int)EnpcAttribute.NAME];
    }
    public void SetNPCDialogues(uint _idx, string[] _data)
    {
        Dialogues = new string[_data.Length];

        for (int i = 0; i < _data.Length; i++) { Dialogues[i] = _data[i]; }
    }
}
