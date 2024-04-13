using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class MonsterProjectileScript : ObjectAttackScript, IHittable, IPoolable
{
    protected Rigidbody m_rigid;

    [SerializeField]
    private float m_moveSpeed = 6;
    [SerializeField]
    private float m_attackRange;
    [SerializeField]
    private float m_lastTime;

    private Vector3 MoveDir { get; set; }

    public bool IsPlayer => false;
    public bool IsMonster => false;

    public ObjectPool<GameObject> OriginalPool { get; private set; }

    public void SetPool(ObjectPool<GameObject> _pool) { OriginalPool = _pool; }
    public void OnPoolGet() { }
    public void ReleaseTopool()
    {
        m_rigid.velocity = Vector3.zero;
        AttackOff();
        OriginalPool.Release(gameObject);
    }


    public void SetAttack(ObjectScript _attacker, Vector3 _dir, float _damage) { SetAttack(_attacker, _damage); MoveDir = new(_dir.x, 0, _dir.z); }



    public void GetHit(HitData _hit)
    {
        Debug.Log("ÆÐ¸µ!");
        DestroyAttack();
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag(ValueDefine.PLAYER_TAG))
        {
            DestroyAttack();
        }
        else if (_other.CompareTag(ValueDefine.TERRAIN_TAG))
        {
            DestroyAttack();
        }
    }

    public virtual void DestroyAttack()
    {
        ReleaseTopool();
    }



    private IEnumerator ReleaseDelay()
    {
        yield return new WaitForSeconds(m_lastTime);
        if (!gameObject.activeSelf) { ReleaseTopool(); }
    }

    public virtual void OnEnable()
    {
        AttackOn();
        StartCoroutine(ReleaseDelay());
    }

    private void SetInfo()
    {

    }


    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }
    public override void Start() { }
    public virtual void FixedUpdate()
    {
        Vector3 vel = m_rigid.velocity;
        Vector3 dir = m_moveSpeed * MoveDir;
        vel.x = dir.x; vel.z = dir.z;
        m_rigid.velocity = vel;
    }
}
