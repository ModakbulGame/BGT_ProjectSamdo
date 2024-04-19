using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideGimicScript : MonoBehaviour, IHidable
{
    [SerializeField]
    private Collider[] m_colliders;

    public virtual void GetLight()
    {
        SetObjectHide(false);
    }

    public virtual void LooseLight()
    {
        SetObjectHide(true);
    }

    private void SetObjectHide(bool _hide)
    {
        foreach (Collider col in m_colliders)
        {
            col.enabled = !_hide;
        }
    }


    private void Awake()
    {
        m_colliders = GetComponentsInChildren<Collider>();
    }
}
