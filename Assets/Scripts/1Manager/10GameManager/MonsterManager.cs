using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 몬스터 관련 enum -> MonsterEnum에 있음

public class MonsterInfo
{
    public string MonsterID { get; private set; }
    public string MonsterName { get; private set; }
    public string MonsterDescription { get; private set; }
    public bool Cleared { get; private set; }
    public MonsterInfo(MonsterScriptable _scriptable)
    {
        MonsterID = _scriptable.ID;
        MonsterName = _scriptable.MonsterName;
        MonsterDescription = _scriptable.Description;
    }
}

public class MonsterDropItemInfo
{
    private readonly List<SDropItem> m_dropItems;
    public string GetDropItem() { return m_dropItems[0].ID; }

    public MonsterDropItemInfo(string _id)
    {

    }
}


public class MonsterManager : MonoBehaviour
{
    private readonly MonsterInfo[] m_monsterInfo = new MonsterInfo[(int)EMonsterName.LAST];
    public MonsterInfo GetMonsterInfo(EMonsterName _monster) { return m_monsterInfo[(int)_monster]; }

    [SerializeField]
    private GameObject[] m_monsterPrefabs = new GameObject[(int)EMonsterName.LAST];


    public GameObject GetMonsterPrefab(EMonsterName _monster)
    {
        return m_monsterPrefabs[(int)_monster];
    }


    public void SetManager()
    {
        for (int i = 0; i<(int)EMonsterName.LAST; i++)
        {
            m_monsterInfo[i] = new MonsterInfo(GameManager.GetMonsterRawData((EMonsterName)i));
        }
    }
}
