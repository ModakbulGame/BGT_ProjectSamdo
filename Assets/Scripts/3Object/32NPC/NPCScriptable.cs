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
        Dialogues = new string[_data.Length - 2];

        for(int i = 2; i < _data.Length; i++)
        {
            Dialogues[i - 2] = _data[i];
        }
    }
}
