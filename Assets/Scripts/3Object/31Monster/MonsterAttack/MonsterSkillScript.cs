using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillScript : ObjectAttackScript
{
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




    public override void Start()
    {
        AttackOn();
    }
}
