using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class UumScript : MonsterScript
{
    [SerializeField]
    private VisualEffect m_headFire;


    public override void StartDissolve()
    {
        base.StartDissolve();
        m_headFire.Stop();
    }
}
