using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGuardianSkill2Script : ObjectAttackScript
{
    private float SkillRadius { get; set; }
    private readonly float DamageGap = 0.5f;

    private float GapCount { get; set; }

    public override void AttackOn()
    {
        gameObject.SetActive(true);
        base.AttackOn();
    }

    public override void AttackOff()
    {
        base.AttackOff();
        gameObject.SetActive(false);
    }

    private void CheckNAttackTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, SkillRadius, ValueDefine.HITTABLE_LAYER);
        foreach (Collider col in hits)
        {
            ObjectScript script = col.GetComponentInParent<ObjectScript>();
            if(script == null || script.IsDead || !script.IsPlayer) { continue; }
            script.GetDamage(Damage, Attacker);
            Vector3 dir = 5 * (transform.position - script.Position).normalized;
            script.AddForce(dir);
        }
    }

    public override void GiveDamage(IHittable _hittable, Vector3 _point)
    {
        if (CheckHit(_hittable)) { return; }
        HitData hit = new(Attacker, Damage, _point, CCList);
        _hittable.GetHit(hit);
        AddHitObject(_hittable);
    }

    private void Awake()
    {
        SkillRadius = transform.localScale.x * 0.5f;
    }

    private void Update()
    {
        if (GapCount > 0)
        {
            GapCount -= Time.deltaTime;
            if (GapCount <= 0)
            {
                GapCount = DamageGap;
                m_hitObjects.Clear();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!IsAttacking) { return; }
        CheckNAttackTarget();
    }
}
