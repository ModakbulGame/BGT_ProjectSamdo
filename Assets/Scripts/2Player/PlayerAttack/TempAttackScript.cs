using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAttackScript : MonoBehaviour       // 임시 공격 스크립트
{
    [SerializeField]
    private float DestTime = 0.1f;
    [SerializeField]
    private GameObject m_tempHitEffect;

    public float Damage { get; private set; }

    public void SetDamage(float _damage) { Damage = _damage; }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag(ValueDefine.MONSTER_TAG) && !_other.GetComponent<MonsterScript>().IsDead)
        {
            CreateEffect();
            DestroyAttack(0);
        }
    }

    private void CreateEffect()
    {
        Instantiate(m_tempHitEffect, transform.position, Quaternion.identity);
    }

    private void DestroyAttack(float _time)
    {
        Destroy(gameObject, _time);
    }

    private void Start()
    {
        DestroyAttack(DestTime);
    }
}
