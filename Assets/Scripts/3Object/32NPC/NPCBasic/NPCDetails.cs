using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENPCAttribute
{
    SNPC,
    NAME,
    DIALOGUES,
    LAST
}

public enum ENPCName
{
    FOO,
    BAR,
    BAZ,
    LAST
}

public enum ENPCType
{
    OASIS,
    ALTAR,
    OTHER,

    LAST
}

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