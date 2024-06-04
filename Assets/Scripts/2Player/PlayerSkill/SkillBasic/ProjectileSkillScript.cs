using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileSkillScript : PlayerSkillScript
{
    protected Rigidbody m_rigid;
    private float HitRadius { get { return m_scriptable.HitRadius; } }
    private float MoveSpeed { get { return m_scriptable.MoveSpeed; } }
    private Vector3 MoveDir;

    private void DeleteTrailOnHit()
    {
        TrailRenderer[] trailRenderers = GetComponentsInChildren<TrailRenderer>();
        foreach(TrailRenderer trailRenderer in trailRenderers)
        {
            trailRenderer.enabled = false;
        }
    }
    public override void CheckSkillTrigger(Collider _other)
    {
        base.CheckSkillTrigger(_other);
        DeleteTrailOnHit();
    }

    public override void ReleaseToPool()
    {
        m_rigid.velocity = Vector3.zero;
        base.ReleaseToPool();
    }

    public void SetSkill(PlayerController _player, float _attack, float _magic, Vector2 _dir)
    {
        SetSkill(_player, _attack, _magic);
        MoveDir = new(_dir.x,0,_dir.y);
    }

    public override void CollideTarget()
    {
        ReleaseToPool();
    }

    public override void Start()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = HitRadius;
    }

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    public virtual void FixedUpdate()
    {
        Vector3 vel = m_rigid.velocity;
        Vector3 dir = MoveSpeed * MoveDir;
        vel.x = dir.x; vel.z = dir.z;
        m_rigid.velocity = vel;
    }
}

