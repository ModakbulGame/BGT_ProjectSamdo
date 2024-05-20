using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeScript : ObjectAttackScript
{
    private Transform ReturnTransform;

    [SerializeField]
    private float m_explosionRadius = 1.75f;

    public override float Damage => 8;

    public override void GiveDamage(IHittable _hittable, Vector3 _point)
    {
        if (CheckHit(_hittable)) { return; }
        HitData hit = new(Attacker, Damage, _point, CCList);
        _hittable.GetHit(hit);
        AddHitObject(_hittable);
    }

    public override void AttackOff()
    {
        base.AttackOff();
        if (ReturnTransform != null)
        {
            transform.SetParent(ReturnTransform);
            ReturnTransform = null;
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
