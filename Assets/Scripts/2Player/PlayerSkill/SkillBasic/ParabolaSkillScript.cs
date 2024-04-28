using System.Collections;
using UnityEngine;

public class ParabolaSkillScript : ProjectileSkillScript
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
        m_rigid.AddForce(Vector3.down * ValueDefine.PARABOLA_GRAVITY);
        base.FixedUpdate();
    }
}
