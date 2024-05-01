using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public abstract partial class MonsterScript
{
    // 기본 전투
    public override void GetHit(HitData _hit)    // 맞음
    {
        if (CurTarget == null) { CurTarget = _hit.Attacker; }
        SetDeathType(_hit.Attacker);
        base.GetHit(_hit);
        m_hpBar.SetCurHP(CurHP);
    }
    public override void PlayHitAnim()
    {
        base.PlayHitAnim();
        ChangeState(EMonsterState.HIT);
    }
    public override void SetDead()
    {
        base.SetDead();
        ChangeState(EMonsterState.DIE);
    }


    // 공격 관련
    [SerializeField]
    protected GameObject[] m_normalAttacks;                                      // 기본 공격 프리펍

    public bool CanAttack { get { return HasTarget && TargetInAttackRange && AttackTimeCount <= 0; } }  // 공격 가능 여부
    public float AttackTimeCount { get; set; } = 0;                                                     // 공격 쿨타임

    public override void AttackTriggerOn()
    {
        AttackTriggerOn(0);
    }
    public virtual void AttackTriggerOn(int _idx)
    {
        m_normalAttacks[_idx].SetActive(true);
        if (AttackObject == null)
        {
            AttackObject = m_normalAttacks[_idx].GetComponent<ObjectAttackScript>();
            AttackObject.SetAttack(this, Attack);
        }
        AttackObject.SetDamage(Attack);
        base.AttackTriggerOn();
    }
    public override void AttackTriggerOff()
    {
        base.AttackTriggerOff();
        m_normalAttacks[0].SetActive(false);
    }
    public override void CreateAttack()
    {
        AttackTriggerOn();
    }

    public override void AttackDone()
    {
        if (CurTarget != null)
            ChangeState(EMonsterState.APPROACH);
        else
            ChangeState(EMonsterState.IDLE);
        AttackTimeCount = 1 / AttackSpeed;
    }


    // 전투 대상 관련
    public readonly float MissTargetDelay = 5f;

    public ObjectScript CurTarget { get; protected set; }                                               // 현재 타겟
    public bool HasTarget { get { return (CurTarget != null && !CurTarget.IsDead); } }                  // 타겟을 가지고 있는지
    public float TargetDistance { get { if (HasTarget) return Vector3.Distance(Position, CurTarget.Position); return -1; } }    // 목표와의 거리
    public bool TargetInAttackRange { get { return TargetDistance <= AttackRange; } }                   // 공격범위 내인지

    public void FindTarget()            // 타겟 탐색 (타겟 X)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, ViewRange);

        if (targets.Length == 0) return;
        foreach (Collider col in targets)
        {
            PlayerController player = col.GetComponentInParent<PlayerController>();         // 일단 플레이어만 체크
            if (player == null) { continue; }
            Vector3 targetPos = col.transform.position;
            Vector3 targetDir = (targetPos - transform.position).normalized;
            Vector3 look = FunctionDefine.AngleToDir(Rotation);
            float targetAngle = Mathf.Acos(Vector3.Dot(look, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= ViewAngle * 0.5f && !Physics.Raycast(transform.position, targetDir, ViewRange))
            {
                CurTarget = player;
            }
        }
    }
    public bool CheckTarget()           // 타겟 확인 (타겟 O)
    {
        if (CurTarget == null) { return false; }
        if (Vector3.Distance(CurTarget.Position, Position) > ReturnRange) { return false; }
        return true;
    }
    public void MissTarget()            // 타겟 잃기
    {
        CurTarget = null;
    }
    public virtual void ApproachTarget()            // 타겟에게 접근
    {
        if (!HasTarget) { return; }
        Vector2 dir = (CurTarget.Position2 - Position2);
        if (AttackTimeCount > 0 && TargetInAttackRange) { RotateTo(dir); return; }
        m_aiPath.destination = CurTarget.Position;
    }


    // 피격 관련
    public readonly float StunDelay = 1f;                       // 피격 시 경직
    private readonly float HitGuardEndTime = 3;                 // 가드 중인 플레이어 타격 후 피격 애니메이션 재생 기간

    private bool HitGuarding { get; set; }                                                              // 플레이어 가드 중 때림
    public override bool IsUnstoppable { get { return InCombat && !HitGuarding; } }                     // 공격 모션 캔슬 불가인지

    public void HitGuardingPlayer()
    {
        HitGuarding = true;
        StartCoroutine(HitGuardEnd());
    }
    private IEnumerator HitGuardEnd()
    {
        yield return new WaitForSeconds(HitGuardEndTime);
        HitGuarding = false;
    }

}
