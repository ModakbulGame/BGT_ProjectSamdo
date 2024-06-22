using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUISE
{
    REWARD_POPUP,
    REWARD_SELECT,
    SHOW_DATA,
    LAST
}

public class UISEListScript : SoundListScript
{
    public override bool ChkList { get { return m_soundList.Length == (int)EUISE.LAST; } }
}
