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
        transform.SetParent(null);
        ReturnTrans = _returnTrans;
        if(m_vfx != null) { m_vfx.Play(); }
        else if(m_particle != null) { m_particle.SetActive(true); }
        StartCoroutine(WaitDone(m_lastTime));
    }
    private IEnumerator WaitDone(float _time)
    {
        yield return new WaitForSeconds(_time);
        EffectDone();
    }
    public void LeaveEffect(Transform _transform, float _delay)
    {
        transform.SetParent(null);
        ReturnTrans = _transform;
        StartCoroutine(WaitDone(_delay));
    }
    public void EffectDone()
    {
        if (m_vfx != null) { m_vfx.Stop(); }
        else if (m_particle != null) { m_particle.SetActive(false); }
        transform.SetParent(ReturnTrans);
    }
}
