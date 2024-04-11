using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    private static readonly Dictionary<int, ObjectPool<GameObject>> m_pools = new();
    private readonly Dictionary<int, GameObject> m_objectList = new();

    [SerializeField]
    private Transform m_poolListTransform;

    private static int CurHash { get; set; }

    public static GameObject GetObject(GameObject _obj)
    {
        int hash = _obj.GetHashCode();
        if (!m_pools.ContainsKey(hash)) { Debug.LogError("풀에 추가되지 않은 오브젝트"); return null; }
        CurHash = hash;
        return m_pools[hash].Get();
    }

    private void CreatePools(GameObject[] _skills, GameObject[] _effects)
    {
        // 스킬
        for(int i=28;i<(int)ESkillName.LAST;i++)
        {
            CreatePool(_skills[i]);
        }
        // 이펙트
        for (int i = 0; i<(int)EEffectName.LAST; i++)
        {
            CreatePool(_effects[i]);
        }
    }

    private void CreatePool(GameObject _obj)
    {
        int hash = _obj.GetHashCode();
        CurHash = hash;
        m_pools[CurHash] = new(CreateItem, GetItem, ReturnItem, DestroyItem, true, 10, 64);
        m_objectList[CurHash] = _obj;
        for (int i = 0; i<10; i++)
        {
            GameObject newItem = CreateItem();
            newItem.GetComponent<IPoolable>().m_originPool.Release(newItem);
        }
    }


    private GameObject CreateItem()
    {
        GameObject item = Instantiate(m_objectList[CurHash]);
        item.GetComponent<IPoolable>().m_originPool = m_pools[CurHash];
        return item;
    }
    private void GetItem(GameObject _item) { _item.SetActive(true); _item.transform.SetParent(null); }
    private void ReturnItem(GameObject _item) { _item.transform.SetParent(m_poolListTransform); _item.SetActive(false); }
    private void DestroyItem(GameObject _item) { Destroy(_item); }


    public void SetManager(GameObject[] _skills, GameObject[] _effects)
    {
        CreatePools(_skills, _effects);
    }
}
