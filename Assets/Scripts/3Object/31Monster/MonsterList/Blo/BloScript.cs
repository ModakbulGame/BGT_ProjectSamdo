using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloScript : MonsterScript
{
    public override bool CanPurify => true;


    [SerializeField]
    private float m_rushSpeed = 6;

    private bool RushDone { get; set; }

    private BloRushState m_rushState;

    public bool IsRushing { get; private set; }

    public override void ApproachTarget()   // 공격 시작
    {
        if (!RushDone) { RushBlo(); return; }
        base.ApproachTarget();
    }
    public override void AttackDone()
    {
        if (IsRushing) { RushDone = true; ChangeState(EMonsterState.APPROACH); return; }
        base.AttackDone();
    }

    private void RushBlo()
    {
        StopMove();
        m_stateManager.ChangeState(m_rushState);
    }
    public void StartRush()
    {
        m_anim.SetTrigger("RUSH");
        IsRushing = true;
    }
    public void RushToTarget()
    {
        Vector3 dir = transform.forward * m_rushSpeed;
        Vector2 adj = (CurTarget.Position2 - Position2).normalized;
        Vector3 adj3 = new Vector3(adj.x, 0, adj.y) * 0.1f;
        dir += adj3;
        m_rigid.velocity = dir;
    }


    public override void OnEnable()
    {
        base.OnEnable();
        RushDone = false;
    }

    public override void SetStates()
    {
        base.SetStates();
        m_rushState = gameObject.AddComponent<BloRushState>();
    }
}
