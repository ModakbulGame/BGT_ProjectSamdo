using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AttackEffect : MonoBehaviour
{
    [SerializeField]
    private float m_lastTime;
    [SerializeField]
    private VisualEffect m_vfx;
    [SerializeField]
    private GameObject m_particle;

    private Transform ReturnTrans { get; set; }

    public void EffectOn(Transform _returnTrans)
    {
        ReturnTrans = _returnTrans;
        if(m_vfx != null) { m_vfx.Play(); }
        else if(m_particle != null) { m_particle.SetActive(true); }
        StartCoroutine(WaitDone());
    }
    private IEnumerator WaitDone()
    {
        yield return new WaitForSeconds(m_lastTime);
        EffectDone();
    }
    public void EffectDone()
    {
        if (m_vfx != null) { m_vfx.Stop(); }
        else if (m_particle != null) { m_particle.SetActive(false); }
        transform.SetParent(ReturnTrans);
    }
}
