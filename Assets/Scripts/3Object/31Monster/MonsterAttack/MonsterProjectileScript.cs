using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MonsterProjectileScript : ObjectAttackScript
{
    protected Rigidbody m_rigid;

    [SerializeField]
    private float m_moveSpeed = 6;
    [SerializeField]
    private float m_attackRange = 4;

    private Vector3 MoveDir { get; set; }

    public void SetAttack(ObjectScript _attacker, Vector3 _dir, float _damage) { SetAttack(_attacker, _damage); MoveDir = new(_dir.x, 0, _dir.z); }


    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag(ValueDefine.PLAYER_TAG))
        {
            DestroyAttack();
        }
    }

    public virtual void DestroyAttack()
    {
        Destroy(gameObject);
    }



    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    public override void Start()
    {
        AttackOn();
        float time = m_attackRange / m_moveSpeed;
        Destroy(gameObject, time);
    }
    public virtual void FixedUpdate()
    {
        Vector3 vel = m_rigid.velocity;
        Vector3 dir = m_moveSpeed * MoveDir;
        vel.x = dir.x; vel.z = dir.z;
        m_rigid.velocity = vel;
    }
}
