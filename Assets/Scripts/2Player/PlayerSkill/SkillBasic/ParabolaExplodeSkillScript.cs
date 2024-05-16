using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaExplodeSkillScript : ParabolaSkillScript
{
    [SerializeField]
    private GameObject m_skillExplode;

    private void ExplodeSkill()
    {
        m_skillExplode.SetActive(true);
        m_skillExplode.transform.SetParent(null);
        PlayerSkillScript explosion = m_skillExplode.GetComponent<PlayerSkillScript>();
        explosion.SetDamage(5);
    }
}
