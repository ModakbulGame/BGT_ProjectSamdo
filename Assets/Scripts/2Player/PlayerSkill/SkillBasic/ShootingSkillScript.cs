using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSkillScript : PlayerSkillScript
{
    private float MoveSpeed { get { return 5;/*m_scriptable.MoveSpeed;*/ } }

    public Vector2 Direction { get; private set; }
    public void SetSkill(PlayerController _player, float _attack, float _magic, Vector2 _dir)
    {
        SetSkill(_player, _attack, _magic);
        Direction = _dir;
    }





    private void Update()
    {
        transform.position += Time.deltaTime * MoveSpeed * transform.forward;
    }
}
