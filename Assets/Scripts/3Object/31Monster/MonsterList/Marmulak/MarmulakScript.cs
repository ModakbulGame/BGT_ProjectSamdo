using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMarmulakAttack
{
    NORMAL,
    THROW,
    ROAR,
    LAST
}

public class MarmulakScript : RangedAttackMonster
{
    public override bool CanPurify => RoarTimeCount >= (RoarCooltime - PurifyTime);

    public EMarmulakAttack CurAttack { get; private set; } = EMarmulakAttack.NORMAL;
    private readonly float NearAttackRange = 3;

    private bool CanRoar { get { return TargetDistance <= RoarRange && RoarTimeCount <= 0; } }

    public override float AttackRange => CanRoar ? RoarRange : (TargetDistance <= RoarRange * 2) ? NearAttackRange : base.AttackRange;

    private readonly float PurifyTime = 8;
    private float RoarTimeCount { get; set; } = 0;

    public override void StartAttack()
    {
        if (CanRoar) { StartRoar(); return; }
        if (TargetDistance <= RoarRange)
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
        RoarTimeCount = RoarCooltime;
        StopMove();
        m_anim.SetTrigger("SKILL");
    }

    public override void CreateAttack()
    {
        ThrowBall();
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
            IsRoaring = true;
            StartCoroutine(RoarCoroutine());
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
            IsRoaring = false;
        }
    }



    private readonly float RoarCooltime = 15;
    public readonly float RoarRange = 8;
    private readonly float RoarGap = 0.5f;

    [SerializeField]
    private Transform m_roarTransform;
    private bool IsRoaring { get; set; }

    private readonly List<ObjectScript> m_roarList = new();
    private IEnumerator RoarCoroutine()
    {
        while (IsRoaring && !IsDead)
        {
            CheckNRoar();
            yield return new WaitForSeconds(RoarGap);
        }
    }
    public void CheckNRoar()
    {
        m_roarList.Clear();
        Collider[] targets = Physics.OverlapSphere(m_roarTransform.position, RoarRange, ValueDefine.HITTABLE_LAYER);
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
