using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESkillSE
{
    SHOOTING,
    SHOOT_NONE_HIT,
    SHOOT_FIRE_HIT,
    SHOOT_POISON_HIT,
    SHOOT_ELECTRIC_HIT,
    SHOOT_ICE_HIT,
    THROWING,
    THROW_NONE_HIT,
    THROW_FIRE_HIT,
    THROW_POISON_HIT,
    THROW_ELECTRIC_HIT,
    THROW_ICE_HIT,
    AROUND_NONE,
    AROUND_NONE_IMPACT,
    AROUND_POISON,
    AROUND_ELECTRIC,
    AROUND_ICE,
    SECTOR_FIRE,
    LAST
}

public class SkillSEListScript : SoundListScript
{
    public override bool ChkList { get { return m_soundList.Length == (int)ESkillSE.LAST; } }
}
