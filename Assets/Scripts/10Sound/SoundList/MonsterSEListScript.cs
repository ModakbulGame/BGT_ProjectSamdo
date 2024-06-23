using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterSE
{
    MONSTER_ATTACK1,
    MONSTER_ATTACK2,
    MONSTER_ATTACK3,
    MONSTER_ATTACK4,
    MONSTER_ATTACK5,
    LAST
}

public class MonsterSEListScript : SoundListScript
{
    public override bool ChkList { get { return m_soundList.Length == (int)EMonsterSE.LAST; } }
}
