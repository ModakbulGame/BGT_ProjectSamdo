using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public abstract partial class MonsterScript
{
    // �⺻ ����
    public override void GetHit(HitData _hit)    // ����
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


    // ���� ����
    [SerializeField]
    protected GameObject[] m_normalAttacks;                                      // �⺻ ���� ������

    public bool CanAttack { get { return HasTarget && TargetInAttackRange && AttackTimeCount <= 0; } }  // ���� ���� ����
    public float AttackTimeCount { get; set; } = 0;                                                     // ���� ��Ÿ��

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


    // ���� ��� ����
    public readonly float MissTargetDelay = 5f;

    public ObjectScript CurTarget { get; protected set; }                                               // ���� Ÿ��
    public bool HasTarget { get { return (CurTarget != null && !CurTarget.IsDead); } }                  // Ÿ���� ������ �ִ���
    public float TargetDistance { get { if (HasTarget) return Vector3.Distance(Position, CurTarget.Position); return -1; } }    // ��ǥ���� �Ÿ�
    public bool TargetInAttackRange { get { return TargetDistance <= AttackRange; } }                   // ���ݹ��� ������

    public void FindTarget()            // Ÿ�� Ž�� (Ÿ�� X)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, ViewRange);

        if (targets.Length == 0) return;
        foreach (Collider col in targets)
        {
            PlayerController player = col.GetComponentInParent<PlayerController>();         // �ϴ� �÷��̾ üũ
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
    public bool CheckTarget()           // Ÿ�� Ȯ�� (Ÿ�� O)
    {
        if (CurTarget == null) { return false; }
        if (Vector3.Distance(CurTarget.Position, Position) > ReturnRange) { return false; }
        return true;
    }
    public void MissTarget()            // Ÿ�� �ұ�
    {
        CurTarget = null;
    }
    public virtual void ApproachTarget()            // Ÿ�ٿ��� ����
    {
        if (!HasTarget) { return; }
        Vector2 dir = (CurTarget.Position2 - Position2);
        if (AttackTimeCount > 0 && TargetInAttackRange) { RotateTo(dir); return; }
        m_aiPath.destination = CurTarget.Position;
    }


    // �ǰ� ����
    public readonly float StunDelay = 1f;                       // �ǰ� �� ����
    private readonly float HitGuardEndTime = 3;                 // ���� ���� �÷��̾� Ÿ�� �� �ǰ� �ִϸ��̼� ��� �Ⱓ

    private bool HitGuarding { get; set; }                                                              // �÷��̾� ���� �� ����
    public override bool IsUnstoppable { get { return InCombat && !HitGuarding; } }                     // ���� ��� ĵ�� �Ұ�����

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
