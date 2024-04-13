using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkillScript : PlayerSkillScript
{
    private float MoveSpeed { get { return 5;/*m_scriptable.MoveSpeed;*/ } }

    public Vector2 Direction { get; private set; }
    public void SetSkill(PlayerController _player, float _attack, float _magic, Vector2 _dir)
    {
        SetSkill(_player, _attack, _magic);
        Direction = _dir;
    }




    /*
    public virtual void FixedUpdate()
    {
        Vector3 vel = m_rigid.velocity;
        Vector3 dir = m_moveSpeed * MoveDir;
        vel.x = dir.x; vel.z = dir.z;
        m_rigid.velocity = vel;
    }
    */
}

