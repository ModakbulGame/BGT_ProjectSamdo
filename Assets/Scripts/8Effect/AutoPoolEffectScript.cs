using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPoolEffectScript : EffectScript
{
    [SerializeField]
    private float m_destroyTime = 1;

    public void OnEnable()
    {
        SetDestroyTime(m_destroyTime);
    }
}
