using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S305 : ParabolaExplodeScript
{
    public override void FixedUpdate()
    {
        m_rigid.AddForce(Vector3.down * m_rigid.mass / 2 * ValueDefine.PARABOLA_GRAVITY);
        base.FixedUpdate();
    }
}
