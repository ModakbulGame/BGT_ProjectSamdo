using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloScript : MonsterScript
{
    [SerializeField]
    private float m_rushSpeed = 6;
    [SerializeField]
    private float m_rushCooltime = 10;
    private float RushCount { get; set; } = 0;

    private BloRushState m_rushState;

    public bool IsRushing { get; private set; }

    public override void ApproachTarget()   // 공격 시작
    {
        if (RushCount <= 0) { RushBlo(); return; }
        base.ApproachTarget();
    }


    public override void AttackDone()
    {
        if (IsRushing) { RushCount = m_rushCooltime; IsRushing = false; ChangeState(EMonsterState.APPROACH); return; }
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
        StartCoroutine(RushTimeCooldown());
    }
    private IEnumerator RushTimeCooldown()
    {
        while(RushCount > 0) { RushCount -= Time.deltaTime; yield return null; }
    }
    public void RushToTarget()
    {
        Vector2 dir = (CurTarget.Position2 - Position2).normalized;
        Vector3 dir3 = new Vector3(dir.x, 0, dir.y) * m_rushSpeed;
        LookTarget();
        m_rigid.velocity = dir3;
    }


    public override void SetStates()
    {
        base.SetStates();
        m_rushState = gameObject.AddComponent<BloRushState>();
    }
}
