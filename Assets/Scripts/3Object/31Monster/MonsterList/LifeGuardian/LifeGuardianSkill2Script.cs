using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGuardianSkill2Script : ObjectAttackScript
{
    private float SkillRadius { get; set; }
    
    private readonly float DamageGap = 0.2f;
    private readonly float DrainForce = 9;

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
        Collider[] hits = Physics.OverlapSphere(transform.position, SkillRadius, ValueDefine.HITTABLE_PLAYER_LAYER);
        foreach (Collider col in hits)
        {
            ObjectScript script = col.GetComponentInParent<ObjectScript>();
            if(script == null ||  script.IsDead || !script.IsPlayer) { continue; }
            Vector3 dir = DrainForce * (transform.position - script.Position);
            script.AddForce(dir);
            if (m_hitObjects.Contains(script)) { return; }
            HitData hit = new(Attacker, Damage, script.Position);
            script.GetDamage(hit);
            m_hitObjects.Add(script);
        }
    }

    public override void GiveDamage(IHittable _hittable, Vector3 _point)
    {
        if (CheckHit(_hittable)) { return; }
        HitData hit = new(Attacker, Damage, _point, m_impulseAmount, CCList);
        if (_hittable.GetHit(hit))
        {
            AddHitObject(_hittable);
        }
    }

    private void Awake()
    {
        SkillRadius = transform.localScale.x * 0.5f;
    }
    public override void Start() { }

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
