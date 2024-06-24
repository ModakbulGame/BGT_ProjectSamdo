using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum EArrogantAttack
{
    RIGHT_SWING,
    LEFT_SWING,
    SMASH,

    LAST
}

public class ArrogantScript : HmmScript
{
    public override bool CanPurify => IsFatigure;

    public EArrogantAttack CurAttack { get; private set; } = EArrogantAttack.RIGHT_SWING;
    
    
    [Tooltip("기본 공격 별 데미지 배율")]
    [SerializeField]
    private float[] m_normalDamageMultiplier = new float[(int)EArrogantAttack.LAST]
    { 1, 1, 1 };

    [Tooltip("내려찍기 데미지")]
    [SerializeField]
    private float m_smashDamage = 10;
    [Tooltip("내려찍기 쿨타임")]
    [SerializeField]
    private float m_smashCooltime = 15;
    [Tooltip("내려찍기 반경")]
    [SerializeField]
    public float m_smashRadius = 4;

    private bool CanSmash { get { return SmashTimeCount <= 0; } }

    public override float AttackRange => CanSmash ? m_smashRadius-2 : base.AttackRange;

    private float SmashTimeCount { get; set; } = 0;

    public override void StartAttack()
    {
        if (CanSmash) { StartSmash(); return; }
        CurAttack = EArrogantAttack.RIGHT_SWING;

        AttackIdx = Random.Range(0, 2/*(int)EArrogantAttack.LAST*/);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        base.StartAttack();
    }

    public override void AttackTriggerOn()
    {
        AttackTriggerOn(AttackIdx);
        float damage = Attack * m_normalDamageMultiplier[AttackIdx];
        AttackObject.SetAttack(this, damage);
    }

    public void StartSmash()
    {
        CurAttack = EArrogantAttack.SMASH;
        SmashTimeCount = m_smashCooltime;
        StopMove();
        m_anim.SetTrigger("SKILL");
        m_smashList.Clear();
    }

    public override void CreateAttack()
    {
        if (CurAttack == EArrogantAttack.SMASH)
        {
            m_smash.Play();
            CheckNSmash();
        }
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


    public override void SetStates()
    {
        base.SetStates();
        ReplaceState(EMonsterState.ATTACK, gameObject.AddComponent<ArrogantAttackScript>());
    }

    public override void ProcCooltime()
    {
        base.ProcCooltime();
        if (SmashTimeCount > 0) { SmashTimeCount -= Time.deltaTime; }
    }
}
