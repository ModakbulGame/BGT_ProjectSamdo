using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ArrogantSmashScript : ObjectAttackScript
{
    [SerializeField]
    private VisualEffect m_effect;

    public override void PlayEffect()
    {
        m_effect.Play();
    }
}
