using Pathfinding;
using System;

[Serializable]
public struct NPCSaveData
{
    public SNPC NPC;
    public DialogueInfo[] DialInfo;
    public readonly bool IsNull { get { return NPC == SNPC.Null; } }
    public static NPCSaveData Null { get { return new(SNPC.Null, new DialogueInfo[0]); } }
    public NPCSaveData(SNPC _npc, DialogueInfo[] _info)
    {
        NPC = _npc; DialInfo = new DialogueInfo[_info.Length];
        for (int i = 0; i<_info.Length; i++)
        {
            DialInfo[i] = new(_info[i]);
        }
    }
    public NPCSaveData(NPCSaveData _other) : this(_other.NPC, _other.DialInfo) { }
}