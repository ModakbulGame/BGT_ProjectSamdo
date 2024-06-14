using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private MonsterSpawnPoint m_point;
    public void SetPoint(MonsterSpawnPoint _point, int _idx) { m_point = _point; SpawnerIdx = _idx; }

    [SerializeField]
    private EMonsterName m_spawnMonster;
    [SerializeField]
    private int m_spawnNum = 1;
    [SerializeField]
    private float m_respawnTime = 10;

    public int SpawnerIdx { get; private set; }
    public Vector3 SpawnPosition { get { return transform.position; } }
    public float RangeMultiplier { get { return m_point.RangeMultiplier; } }


    public void SpawnMonster()
    {
        GetComponent<MeshRenderer>().enabled = false;

        for (int i = 0; i<m_spawnNum; i++)
        {
            GameObject monster = GameManager.GetMonsterObj(m_spawnMonster);

            float dist = Random.Range(1.5f, 2);
            Vector2 dir = dist * FunctionDefine.RotateVector2(Vector2.right, Random.Range(0, 360f));

            Vector3 offset = new(dir.x, 0, dir.y);
            monster.transform.position = transform.position + offset;

            MonsterScript script = monster.GetComponent<MonsterScript>();
            m_point.AddMonster(script);
            script.SetSpawnPoint(this);
        }
    }

    public void DespawnMonster(MonsterScript _monster)
    {
        m_point.RemoveMonster(_monster);
        StartCoroutine(RespawnCoroutine());
    }
    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(m_respawnTime);
        SpawnMonster();
    }
}
