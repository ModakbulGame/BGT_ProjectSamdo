using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlaySE
{
    DOOR_OPEN,
    DOOR_CLOSE,
    MONSTER_SPAWN,
    ELEMENT_NOTICE,
    ELEMENT_DEST,
    DEFENSE_START,
    DEFENSE_HIT,
    DEFENSE_FAIL,
    SPIKETRAP,
    RETRY,
    ENTER_GAME,
    ARANED,
    LAST
}

public class PlaySEListScript : SoundListScript
{
    public override bool ChkList { get { return m_soundList.Length == (int)EPlaySE.LAST; } }
}
