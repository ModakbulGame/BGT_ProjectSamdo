using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileSkillScript : PlayerSkillScript
{
    protected Rigidbody m_rigid;
    private float MoveSpeed { get { return 5;/*m_scriptable.MoveSpeed;*/ } }

    private Vector3 MoveDir;

    public override void ReleaseTopool()
    {
        m_rigid.velocity = Vector3.zero;
        base.ReleaseTopool();
    }

    public void SetSkill(PlayerController _player, float _attack, float _magic, Vector2 _dir)
    {
        SetSkill(_player, _attack, _magic);
        MoveDir = new(_dir.x,0,_dir.y);
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

