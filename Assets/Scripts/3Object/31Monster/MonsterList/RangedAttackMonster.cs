using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RangedAttackMonster : MonsterScript
{
    protected ObjectPool<GameObject> m_attackPool;

    [SerializeField]
    private int m_attackMaxNum = 5;


    private void InitPool()
    {
        m_attackPool = new(OnPoolCreate, OnPoolGet, OnPoolRelease, OnPoolDestroy, true, m_attackMaxNum, m_attackMaxNum);
        for (int i = 0; i<m_attackMaxNum; i++) { GameObject skurraby = OnPoolCreate(); skurraby.GetComponent<MonsterProjectileScript>().ReleaseTopool(); }
    }
    private GameObject OnPoolCreate()
    {
        GameObject skurraby = Instantiate(m_normalAttacks, transform);
        skurraby.GetComponent<MonsterProjectileScript>().SetPool(m_attackPool);
        return skurraby;
    }
    private void OnPoolGet(GameObject _skurraby) { _skurraby.SetActive(true); }
    private void OnPoolRelease(GameObject _skurraby) { _skurraby.transform.SetParent(transform); _skurraby.SetActive(false); }
    private void OnPoolDestroy(GameObject _skurraby) { Destroy(_skurraby); }


    public override void Awake()
    {
        base.Awake();
        InitPool();
    }
}
