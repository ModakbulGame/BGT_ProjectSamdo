using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBGM
{
    TITLE,
    MAIN,
    BOSS_ENTER,
    BOSS_BATTLE,
    GAME_OVER,
    CLEAR,
    LAST
}

public class BGMListScript : SoundListScript
{
    public override bool ChkList { get { return m_soundList.Length == (int)EBGM.LAST; } }
}
