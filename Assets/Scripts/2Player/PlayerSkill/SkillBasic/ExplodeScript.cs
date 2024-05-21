using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ExplodeScript : ObjectAttackScript
{
    [SerializeField]
    private VisualEffect m_explosionEffect;

    private Transform ReturnTransform;

    public void SetDamage(ObjectScript _attacker, float _damage, float _time)
    {
        SetAttack(_attacker, _damage);
        StartCoroutine(LoseDamage(_time));
    }

    private IEnumerator LoseDamage(float _time)
    {
        yield return new WaitForSeconds(_time);
        AttackOff();
    }
}
