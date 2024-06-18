using System;

[Serializable]
public struct NPCSaveData
{
    public SNPC NPC;
    public bool[] DialDone;
    public readonly bool IsNull { get { return NPC == SNPC.Null; } }
    public static NPCSaveData Null { get { return new(SNPC.Null, new bool[0]); } }
    public NPCSaveData(SNPC _npc, bool[] _states)
    {
        NPC = _npc; DialDone = new bool[_states.Length];
        for (int i = 0; i<_states.Length; i++)
        {
            DialDone[i] = _states[i];
        }
    }
    public NPCSaveData(NPCSaveData _other) : this(_other.NPC, _other.DialDone) { }
}