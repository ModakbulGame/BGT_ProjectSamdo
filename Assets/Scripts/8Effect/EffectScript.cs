using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    [SerializeField]
    private float m_destroyTime = 1;

    public virtual void Awake()
    {
        Destroy(gameObject, m_destroyTime);
    }
}
