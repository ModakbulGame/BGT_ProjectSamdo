using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ExplodeScript : ObjectAttackScript
{
    protected ObjectAttackScript m_attack;
    public ObjectAttackScript Attack { get { return m_attack; } }
    private Transform ReturnTransform { get; set; }

    public void SetAttack(ObjectAttackScript _attacker,float _damge)
    {
        m_attack= _attacker; SetDamage(_damge);
    }
    public void SetDamage(ObjectAttackScript _attacker, float _damage, float _time)
    {
        SetAttack(_attacker, _damage);
        StartCoroutine(LoseDamage(_time));
    }

    public void SetReturnTransform(Transform _transform)
    {
        ReturnTransform = _transform;
    }

    private IEnumerator LoseDamage(float _time)
    {
        yield return new WaitForSeconds(_time);
        AttackOff();
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

    public override void Start() { }
}
