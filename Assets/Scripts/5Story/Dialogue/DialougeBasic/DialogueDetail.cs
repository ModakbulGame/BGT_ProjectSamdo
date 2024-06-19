using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NPCDialogue
{
    public SNPC NPC;
    public int Idx;
    public NPCDialogue(SNPC _npc, int _idx) { NPC = _npc; Idx = _idx; }
}