using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMarmulakAttack
{
    NORMAL,
    ROAR,
    LAST
}

public class MarmulakScript : AnimatedAttackMonster
{
    public EMarmulakAttack CurAttack { get; private set; } = EMarmulakAttack.NORMAL;
    private readonly float RoarCooltime = 15;
    public readonly float RoarRange = 10;

    private bool CanRoar { get { return RoarTimeCount <= 0; } }

    public override float AttackRange => CanRoar ? RoarRange-2 : base.AttackRange;

    private float RoarTimeCount { get; set; } = 0;

    public override void StartAttack()
    {
        if (CanRoar) { StartRoar(); return; }
        CurAttack = EMarmulakAttack.NORMAL;
        base.StartAttack();
    }

    public void StartRoar()
    {
        CurAttack = EMarmulakAttack.ROAR;
        RoarTimeCount = RoarCooltime;
        StopMove();
        m_anim.SetTrigger("SKILL");
        m_roarList.Clear();
    }

    public override void CreateAttack()
    {
        if (CurAttack == EMarmulakAttack.ROAR)
        {
            CheckNRoar();
        }
    }

    private readonly List<ObjectScript> m_roarList = new();
    public void CheckNRoar()
    {
        Collider[] targets = Physics.OverlapSphere(Position, RoarRange, ValueDefine.HITTABLE_LAYER);
        for (int i = 0; i<targets.Length; i++)
        {
            ObjectScript obj = targets[i].GetComponentInParent<ObjectScript>();
            if (obj == null || m_roarList.Contains(obj)) { continue; }
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
        if(RoarTimeCount > 0) { RoarTimeCount -= Time.deltaTime; }
    }
}
