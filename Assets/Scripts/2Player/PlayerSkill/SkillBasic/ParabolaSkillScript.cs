using System.Collections;
using System.Data;
using UnityEngine;

public class ParabolaSkillScript : ProjectileSkillScript
{
    [SerializeField]
    private float m_upperForce = 5;

    [SerializeField]
    private float ForceMultiplier { get; set; } = 1;
    private float UpperForce { get { return m_upperForce * ForceMultiplier; } }
    public override void OnEnable()
    {
        base.OnEnable();
        ForceMultiplier = m_scriptable.MoveSpeed;
        m_rigid.velocity = Vector3.up * UpperForce;
    }


    public override void FixedUpdate()
    {
        m_rigid.AddForce(Vector3.down * ValueDefine.PARABOLA_GRAVITY); // rigidbody mass Use it or not?? if yes, m_rigid.mass
        base.FixedUpdate();
    }
}
