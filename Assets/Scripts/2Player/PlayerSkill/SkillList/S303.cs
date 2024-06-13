using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S303 : ProjectileSkillScript
{
    [SerializeField]
    private GameObject m_skillExplosion;
    private PlayerController Player { get { return (PlayerController)m_attacker; } }
    Vector3 SkillOffset { get; set; }

    private void ExplodeSkill()
    {
            m_skillExplosion.SetActive(true);
            m_skillExplosion.transform.localPosition = SkillOffset;
            m_skillExplosion.transform.SetParent(null);
            ExplodeScript explode = m_skillExplosion.GetComponent<ExplodeScript>();
            explode.SetDamage(m_attacker, 10, 1);
            explode.SetReturnTransform(transform);
    }

    public override void GiveDamage(IHittable _hittable, Vector3 _point)
    {
        if (CheckHit(_hittable)) { return; }
        HitData hit = new(Player, ResultDamage, _point, CCList);
        _hittable.GetHit(hit);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        ExplodeSkill();
    }
}
