using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ExplodeScript : ObjectAttackScript
{
    [SerializeField]
    private VisualEffect m_explosionEffect;
    [SerializeField]
    public override float Damage => base.Damage;

    public void SetDamage(ObjectScript _attacker, float _damage, float _time)
    {
        SetAttack(_attacker, _damage);
        StartCoroutine(Explosion());
    }

    private IEnumerator Explosion()
    {
        m_explosionEffect.Play();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void CheckExplosionTrigger(Collider _other)
    {
        IHittable hittable = _other.GetComponentInParent<IHittable>();
        hittable ??= _other.GetComponentInChildren<IHittable>();
        if (hittable == null) { return; }
        if (hittable.IsPlayer) { return; }
        Vector3 point = _other.ClosestPoint(transform.position);
        GiveDamage(hittable, point);
    }
}
