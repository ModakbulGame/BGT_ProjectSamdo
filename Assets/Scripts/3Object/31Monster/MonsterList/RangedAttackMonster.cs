using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Timeline;

public class RangedAttackMonster : MonsterScript
{
    protected ObjectPool<GameObject> m_attackPool;

    [SerializeField]
    private int m_attackMaxNum = 5;

    public virtual Vector3 AttackOffset { get { return Vector3.zero; } }

    public override void CreateAttack()
    {
        GameObject attack = m_attackPool.Get();
        attack.transform.localPosition = AttackOffset;
        attack.transform.parent = null;

        Vector3 dir = (CurTarget.Position - Position).normalized;

        MonsterProjectileScript script = attack.GetComponent<MonsterProjectileScript>();
        script.SetAttack(this, dir, Attack);
        script.AttackOn();
    }

    private void InitPool()
    {
        m_attackPool = new(OnPoolCreate, OnPoolGet, OnPoolRelease, OnPoolDestroy, true, m_attackMaxNum, m_attackMaxNum);
        for (int i = 0; i<m_attackMaxNum; i++) { GameObject obj = OnPoolCreate(); obj.GetComponent<MonsterProjectileScript>().ReleaseTopool(); }
    }
    private GameObject OnPoolCreate()
    {
        GameObject obj = Instantiate(m_normalAttacks[0], transform);
        obj.GetComponent<MonsterProjectileScript>().SetPool(m_attackPool);
        return obj;
    }
    private void OnPoolGet(GameObject _obj) { _obj.SetActive(true); }
    private void OnPoolRelease(GameObject _obj) { _obj.transform.SetParent(transform); _obj.SetActive(false); }
    private void OnPoolDestroy(GameObject _obj) { Destroy(_obj); }


    public override void Awake()
    {
        base.Awake();
        InitPool();
    }
}
