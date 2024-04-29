using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 몬스터 관련 enum -> MonsterEnum에 있음

public class MonsterInfo
{
    public MonsterScriptable MonsterData { get; private set; }
    public string MonsterName { get { return MonsterData.MonsterName; } }
    public string MonsterDescription { get { return MonsterData.Description; } }
    public bool Cleared { get; private set; }
    public void ClearMonster() { Cleared = true; }
    public MonsterInfo(MonsterScriptable _scriptable)
    {
        MonsterData = _scriptable;
        Cleared = false;
    }
}

public class MonsterManager : MonoBehaviour
{
    private readonly MonsterInfo[] m_monsterInfo = new MonsterInfo[(int)EMonsterName.LAST];
    public MonsterInfo GetMonsterInfo(EMonsterName _monster) { return m_monsterInfo[(int)_monster]; }

    [SerializeField]
    private GameObject[] m_monsterPrefabs = new GameObject[(int)EMonsterName.LAST];
    public GameObject[] MonsterArray { get { return m_monsterPrefabs; } }

    public GameObject GetMonsterObj(EMonsterName _monster)
    {
        return PoolManager.GetObject(m_monsterPrefabs[(int)_monster]);
    }


    public void SetManager()
    {
        for (int i = 0; i<(int)EMonsterName.LAST; i++)
        {
            m_monsterInfo[i] = new MonsterInfo(GameManager.GetMonsterRawData((EMonsterName)i));
        }
    }
}
