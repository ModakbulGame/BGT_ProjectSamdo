using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EArrogantAttack
{
    NORMAL,
    JUMP,
    LAST
}

public class ArrogantScript : HmmScript
{
    public EArrogantAttack CurAttack { get; private set; } = EArrogantAttack.NORMAL;
    private readonly float JumpCooltime = 15;
    public readonly float JumpRange = 10;

    private bool CanJump { get { return JumpTimeCount <= 0; } }

    public override float AttackRange => CanJump ? JumpRange-2 : base.AttackRange;

    private float JumpTimeCount { get; set; } = 0;

    public override void StartAttack()
    {
        if (CanJump) { StartJump(); return; }
        CurAttack = EArrogantAttack.NORMAL;
        base.StartAttack();
    }

    public void StartJump()
    {
        CurAttack = EArrogantAttack.JUMP;
        JumpTimeCount = JumpCooltime;
        StopMove();
        m_anim.SetTrigger("SKILL");
        m_jumpList.Clear();
    }

    public override void CreateAttack()
    {
        if (CurAttack == EArrogantAttack.JUMP)
        {
            CheckNJump();
        }
    }

    private readonly List<ObjectScript> m_jumpList = new();
    public void CheckNJump()
    {
        Collider[] targets = Physics.OverlapSphere(Position, JumpRange, ValueDefine.HITTABLE_LAYER);
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
        if (JumpTimeCount > 0) { JumpTimeCount -= Time.deltaTime; }
    }
}
