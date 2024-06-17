using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectilePowerScript : PlayerPowerScript
{
    protected Rigidbody m_rigid;
    public float HitRadius { get { return m_scriptable.HitRadius; } }
    public float MoveSpeed { get { return m_scriptable.MoveSpeed; } }
    public Vector3 MoveDir;
    private Vector3 StartPosition;

    private void DeleteTrailOnHit()
    {
        TrailRenderer[] trailRenderers = GetComponentsInChildren<TrailRenderer>();
        foreach(TrailRenderer trailRenderer in trailRenderers)
        {
            trailRenderer.enabled = false;
        }
    }

    public override void ReleaseToPool()
    {
        m_rigid.velocity = Vector3.zero;
        base.ReleaseToPool();
        //DeleteTrailOnHit();
    }

    public virtual void OnDestroyObject()
    {
        Destroy(gameObject);
    }

    public virtual bool DistanceCheck(float _distanceMoved)
    {
        return _distanceMoved > m_scriptable.CastingRange;
    }

    public virtual void SetPower(PlayerController _player, float _attack, float _magic, Vector2 _dir)
    {
        SetPower(_player, _attack, _magic);
        MoveDir = new(_dir.x,0,_dir.y);
        transform.localEulerAngles = new(0, FunctionDefine.VecToDeg(_dir), 0);
    }

    public override void CollideTarget()
    {
        ReleaseToPool();
    }

    public override void Start()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if(sphereCollider!= null)
        {
            sphereCollider.radius = HitRadius;
        }
        StartPosition = transform.position;
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
        float distanceMoved = Vector3.Distance(StartPosition, transform.position);
        if (DistanceCheck(distanceMoved))
        {
            OnDestroyObject();
        }
    }
}

