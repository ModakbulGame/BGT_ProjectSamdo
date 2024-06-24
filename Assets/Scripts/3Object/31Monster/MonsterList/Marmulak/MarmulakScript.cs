using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum EMarmulakAttack
{
    NORMAL,
    THROW,
    ROAR,
    LAST
}

public class MarmulakScript : RangedAttackMonster
{
    public override bool CanPurify => RoarTimeCount >= (m_roarCooltime - PurifyTime);

    public EMarmulakAttack CurAttack { get; private set; } = EMarmulakAttack.NORMAL;

    private readonly float ThrowRange = 25;

    private bool CanRoar { get { return TargetDistance <= m_roarRange && RoarTimeCount <= 0; } }

    public override float AttackRange => CanRoar ? m_roarRange : (TargetDistance <= m_roarRange) ? base.AttackRange : ThrowRange;

    private readonly float PurifyTime = 8;
    private float RoarTimeCount { get; set; } = 0;

    public override void StartAttack()
    {
        if (CanRoar) { StartRoar(); return; }
        if (TargetDistance <= m_roarRange)
        {
            CurAttack = EMarmulakAttack.NORMAL;
            m_anim.SetInteger("ATTACK_IDX", Random.Range(0, 2));
        }
        else
        {
            CurAttack = EMarmulakAttack.THROW;
            m_anim.SetInteger("ATTACK_IDX", 2);
        }
        base.StartAttack();
    }

    public override Vector3 AttackOffset => new(0.63f, 1.391f, 1.321f);
    public void ThrowBall()
    {
        base.CreateAttack();
    }
    public void StartRoar()
    {
        CurAttack = EMarmulakAttack.ROAR;
        RoarTimeCount = m_roarCooltime;
        StopMove();
        m_anim.SetTrigger("SKILL");
    }

    public override void CreateAttack()
    {
        if (CurAttack == EMarmulakAttack.THROW)
        {
            ThrowBall();
        }
        else if(CurAttack == EMarmulakAttack.ROAR)
        {
            CheckNRoar();
        }
    }
    public override void AttackTriggerOn()
    {
        if(CurAttack == EMarmulakAttack.NORMAL)
        {
            base.AttackTriggerOn();
            AttackObject.SetDamage(Attack);
        }
        else if (CurAttack == EMarmulakAttack.ROAR)
        {
            m_roarEffect.Play();
        }
    }
    public override void AttackTriggerOff()
    {
        if(CurAttack == EMarmulakAttack.NORMAL)
        {
            base.AttackTriggerOff();
        }
        else if (CurAttack == EMarmulakAttack.ROAR)
        {
            m_roarEffect.Stop();
        }
    }


    [SerializeField]
    private MarmulakRoarEffect m_roarEffect;

    [SerializeField]
    private float m_roarCooltime = 15;
    [SerializeField]
    public float m_roarRange = 10;


    private readonly List<ObjectScript> m_roarList = new();
    public void CheckNRoar()
    {
        m_roarList.Clear();
        Collider[] targets = Physics.OverlapSphere(m_roarEffect.transform.position, m_roarRange, ValueDefine.HITTABLE_PLAYER_LAYER);
        for (int i = 0; i<targets.Length; i++)
        {
            ObjectScript obj = targets[i].GetComponentInParent<ObjectScript>();
            if (obj == null || obj == this || m_roarList.Contains(obj)) { continue; }
            Debug.Log($"{obj.ObjectName} 은(는) 공포에 떨고 있다!");
            m_roarList.Add(obj);
        }
    }


    public override void SetStates()
    {
        base.SetStates();
        ReplaceState(EMonsterState.ATTACK, gameObject.AddComponent<MarmulakAttackScript>());
    }

    public override void ProcCooltime()
    {
        base.ProcCooltime();
        if (RoarTimeCount > 0) { RoarTimeCount -= Time.deltaTime; }
    }
}
