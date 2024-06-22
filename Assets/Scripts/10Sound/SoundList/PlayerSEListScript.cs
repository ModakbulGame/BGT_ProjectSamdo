using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerSE
{
    DASH,
    UPGRADE,
    GET_SKILL,
    HIT,
    DIE,
    HEAL,
    LAST
}

public class PlayerSEListScript : SoundListScript
{
    public override bool ChkList { get { return m_soundList.Length == (int)EPlayerSE.LAST; } }
}
