using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterSE
{
    CLOCK_ATTACK,
    CLOCK_BIRD,
    GOLEM_CRUSH,
    GOLEM_ATTACK,
    SPIDER_ATTACK,
    BOOK_ATTACK,
    CAULDRON_ATTACK,
    CAULDRON_SPAWN,
    ELEMENTAL_ATTACK,
    MIMIC_ATTACK,
    BOSS_NORMAL,
    BOSS_NORMAL_EXP,
    BOSS_A_MARKER,
    BOSS_A_EXP,
    BOSS_B_LOAD,
    BOSS_B_IMPACT,
    BOSS_B_ALARM,
    LAST
}

public class MonsterSEListScript : SoundListScript
{
    public override bool ChkList { get { return m_soundList.Length == (int)EMonsterSE.LAST; } }
}
