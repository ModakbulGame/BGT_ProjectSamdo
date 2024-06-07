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
        Vector3 dir = transform.forward;
        Vector3 rot = (dir + (CurTarget.Position - Position).normalized * 0.1f).normalized;
        Vector2 rot2 = new(rot.x, rot.z);
        RotateTo(rot2);
        m_rigid.velocity = m_rushSpeed * dir.normalized;
    }


    public override void OnSpawned()
    {
        base.OnSpawned();
        RushDone = false;
    }

    public override void SetStates()
    {
        base.SetStates();
        m_rushState = gameObject.AddComponent<BloRushState>();
    }
}
