using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ExplodeScript : ObjectAttackScript
{ 
    private Transform ReturnTransform { get; set; }
    private Vector3 LocalOffset { get; set; }  //  얘가 필요한가?

    public void SetDamage(ObjectScript _attacker, float _damage, float _time)
    {
        SetAttack(_attacker, _damage);
        StartCoroutine(LoseDamage(_time));
    }

    private void CheckExplosion(Collider _other)
    {
        IHittable hittable=_other.GetComponentInParent<IHittable>();
        hittable ??=_other.GetComponentInChildren<IHittable>();
        if (hittable == null) { return; }
        if (hittable.IsPlayer) { return; } // 이 부분 삭제하면 전 hittable 공격 가능한 script로 변경
        Vector3 point = _other.ClosestPoint(transform.position);
        GiveDamage(hittable, point);
    }

    private void OnTriggerEnter(Collider _other)
    {
        CheckExplosion(_other);
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
            transform.position = ReturnTransform.position;
            transform.localPosition = LocalOffset;
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
