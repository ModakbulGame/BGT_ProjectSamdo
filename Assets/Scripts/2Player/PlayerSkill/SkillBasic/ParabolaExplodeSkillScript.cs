using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaExplodeSkillScript : ParabolaSkillScript
{
    [SerializeField]
    private GameObject m_skillExplode;
    [SerializeField]
    private Vector2 m_spawnRangeMax;
    [SerializeField]
    private Vector2 m_spawnRangeMin;
    private readonly float NumberOfPrefabsSpawn = 3;

    private void ExplodeSkill()
    {
        float randomX = Random.Range(m_spawnRangeMin.x, m_spawnRangeMax.x);
        float randomZ = Random.Range(m_spawnRangeMin.y, m_spawnRangeMax.y);
        Vector3 spawnPosition = new Vector3(randomX, 0, randomZ);
        m_skillExplode.SetActive(true);
        m_skillExplode.transform.SetParent(null);
        PlayerSkillScript explosion = m_skillExplode.GetComponent<PlayerSkillScript>();
        explosion.SetDamage(5);
    }

    public override void ReleaseToPool()
    {
        base.ReleaseToPool();
    }

}
