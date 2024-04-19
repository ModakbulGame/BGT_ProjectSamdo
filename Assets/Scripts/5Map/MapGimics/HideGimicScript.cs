using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideGimicScript : MonoBehaviour, IHidable
{
    private Collider[] m_colliders;


    public virtual void GetLight()
    {
        SetColliderState(true);
    }

    public virtual void LooseLight()
    {
        SetColliderState(false);
    }

    private void SetColliderState(bool _active)
    {
        foreach (Collider col in m_colliders)
        {
            col.enabled = _active;
        }
    }


    private void Awake()
    {
        m_colliders = GetComponentsInChildren<Collider>();
    }
}
