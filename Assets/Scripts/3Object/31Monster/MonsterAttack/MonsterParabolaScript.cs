using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParabolaScript : MonsterProjectileScript
{
    [SerializeField]
    private float m_upperForce = 5;






    public override void OnEnable()
    {
        base.OnEnable();
        m_rigid.velocity = Vector3.up * m_upperForce;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        m_rigid.AddForce(Vector3.down * ValueDefine.PARABOLA_GRAVITY);
    }
}
