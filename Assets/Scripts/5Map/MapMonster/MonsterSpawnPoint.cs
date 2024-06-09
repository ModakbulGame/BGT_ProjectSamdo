using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAreaType
{
    AISLE,
    FIELD,


    LAST
}

public class MonsterSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private MonsterSpawner[] m_spawners;

    [SerializeField]
    private EAreaType m_areaType;

    private readonly float[] m_rangeMultiplier = new float[(int)EAreaType.LAST] { 1, 1.5f };

    public float RangeMultiplier { get { return m_rangeMultiplier[(int)m_areaType]; } }

    public Vector3 SpawnPosition { get { return transform.position; } }


    [SerializeField]
    private List<MonsterScript> m_spawnedMonsters = new();
    public void AddMonster(MonsterScript _monster) 
    { 
        if(m_spawnedMonsters.Contains(_monster)) { return; }
        m_spawnedMonsters.Add(_monster);
    }


    private void SetComps()
    {
        m_spawners = GetComponentsInChildren<MonsterSpawner>();
        foreach (MonsterSpawner spawner in m_spawners)
        {
            spawner.SetPoint(this);
        }
    }

    private void Awake()
    {
        SetComps();
    }
    private void Start()
    {
        foreach (MonsterSpawner spawner in m_spawners)
        {
            spawner.SpawnMonster();
        }
    }
}
