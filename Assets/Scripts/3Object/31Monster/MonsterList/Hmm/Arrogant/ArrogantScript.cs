using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EArrogantAttack
{
    NORMAL,
    SMASH,
    LAST
}

public class ArrogantScript : HmmScript
{
    public EArrogantAttack CurAttack { get; private set; } = EArrogantAttack.NORMAL;
    private readonly float SmashCooltime = 15;
    public readonly float SmashRange = 10;

    private bool CanSmash { get { return SmashTimeCount <= 0; } }

    public override float AttackRange => CanSmash ? SmashRange-2 : base.AttackRange;

    private float SmashTimeCount { get; set; } = 0;

    public override void StartAttack()
    {
        if (CanSmash) { StartSmash(); return; }
        CurAttack = EArrogantAttack.NORMAL;
        base.StartAttack();
    }

    public void StartSmash()
    {
        CurAttack = EArrogantAttack.SMASH;
        SmashTimeCount = SmashCooltime;
        StopMove();
        m_anim.SetTrigger("SKILL");
        m_jumpList.Clear();
    }

    public override void CreateAttack()
    {
        if (CurAttack == EArrogantAttack.SMASH)
        {
            CheckNSmash();
        }
    }

    private readonly List<ObjectScript> m_jumpList = new();
    public void CheckNSmash()
    {
        Collider[] targets = Physics.OverlapSphere(Position, SmashRange, ValueDefine.HITTABLE_LAYER);
        for (int i = 0; i<targets.Length; i++)
        {
            ObjectScript obj = targets[i].GetComponentInParent<ObjectScript>();
            if (obj == null || obj == this || m_jumpList.Contains(obj)) { continue; }
            HitData air = new(this, Attack, Position, ECCType.AIRBORNE);
            obj.GetHit(air);
            m_jumpList.Add(obj);
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
