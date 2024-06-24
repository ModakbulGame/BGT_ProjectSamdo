using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMonster : MonsterScript
{
    [SerializeField]
    protected float m_anySkillCooltime = 8;

    [SerializeField]
    protected float[] m_skillDamage;
    [SerializeField]
    protected float[] m_skillCooltime;
}
