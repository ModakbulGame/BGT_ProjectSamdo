using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerSE
{
    PARRYING,
    SLASH1,
    LAST
}

public class PlayerSEListScript : SoundListScript
{
    public override bool ChkList { get { return m_soundList.Length == (int)EPlayerSE.LAST; } }
}
