using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private float m_fenceRange = 30;
    public float FenceRange { get { return m_fenceRange; } }
    public Vector3 SpawnPosition { get { return transform.position; } }


    [SerializeField]
    private List<MonsterScript> m_spawnedMonsters = new();
    public void AddMonster(MonsterScript _monster) 
    { 
        if(m_spawnedMonsters.Contains(_monster)) { return; }
        m_spawnedMonsters.Add(_monster);
    }
}
