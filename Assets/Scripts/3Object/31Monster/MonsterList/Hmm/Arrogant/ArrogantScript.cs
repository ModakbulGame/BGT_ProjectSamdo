using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum EArrogantAttack
{
    RIGHT_SWING,
    LEFT_SWING,

    LAST
}

public enum EArrogantSkill
{
    SMASH,

    LAST
}

public class ArrogantScript : HmmScript
{
    public override bool CanPurify => IsFatigure;


    [Tooltip("내려찍기 반경")]
    [SerializeField]
    public float m_smashRadius = 4;

    public override float AttackRange => SkillTimeCheck ? m_smashRadius-2 : base.AttackRange;

    public override void StartAttack()
    {
        SetAttackIdx(Random.Range(0, (int)EArrogantAttack.LAST));
        base.StartAttack();
    }

    public override void AttackTriggerOn()
    {
        base.AttackTriggerOn(AttackIdx);
        AttackObject.SetAttack(this, NormalDamage(AttackIdx));
        AttackObject.AttackOn();
    }
    public override void AttackTriggerOff()
    {
        base.AttackTriggerOff();
        AttackObject.AttackOff();
    }

    public override void StartSkill()
    {
        base.StartSkill();
        m_smashList.Clear();
    }
    public override void CreateSkill()
    {
        ((ArrogantSmashScript)SkillList[0]).PlayEffect();
        CheckNSmash();
        base.CreateSkill();
    }

    [SerializeField]
    private VisualEffect m_smash;

    private readonly List<ObjectScript> m_smashList = new();
    public void CheckNSmash()
    {
        Collider[] targets = Physics.OverlapSphere(m_smash.transform.position, m_smashRadius, ValueDefine.HITTABLE_PLAYER_LAYER);
        for (int i = 0; i<targets.Length; i++)
        {
            ObjectScript obj = targets[i].GetComponentInParent<ObjectScript>();
            if (obj == null || obj == this || m_smashList.Contains(obj)) { continue; }
            HitData air = new(this, Attack, Position, ECCType.AIRBORNE);
            if (obj.GetHit(air))
            {
                m_smashList.Add(obj);
            }
        }
    }
}
