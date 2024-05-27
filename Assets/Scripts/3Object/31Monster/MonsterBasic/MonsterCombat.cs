using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public abstract partial class MonsterScript
{
    // ���� �Ŵ���
    private MonsterBattler m_battleManager;
    public bool AgainstMonster { get { return m_battleManager.AgainstMonster; } set { m_battleManager.AgainstMonster = value; } }
    private void SetBattleTarget(ObjectScript _obj)
    {
        if (_obj == CurTarget) { return; }
        if (_obj.IsPlayer) { CurTarget = _obj; }
        else if (_obj.IsMonster && AgainstMonster) { CurTarget = _obj; }
    }
    private bool CheckMonsterBattle(HitData _hit)
    {
        if (_hit.Attacker.IsPlayer) { return false; }
        MonsterScript monster = (MonsterScript)_hit.Attacker;
        if(!AgainstMonster && !monster.AgainstMonster) { NotTargetDamage(_hit); return true; }
        AgainstMonster = true;
        return false;
    }
    private void NotTargetDamage(HitData _hit)
    {
        float damage = _hit.Damage * (100-Defense) * 0.01f;
        GetDamage(damage);
        Debug.Log($"{_hit.Attacker.ObjectName} => {ObjectName} {damage} ������");
    }
    

    // �⺻ ����
    public override void GetHit(HitData _hit)    // ����
    {
        if (CheckMonsterBattle(_hit)) { return; }
        SetBattleTarget(_hit.Attacker);
        SetDeathType(_hit.Attacker);
        base.GetHit(_hit);
    }
    public override void GetDamage(float _damage)
    {
        base.GetDamage(_damage);
        m_hpBar.SetCurHP(CurHP);
    }
    public override void PlayHitAnim(HitData _hit)
    {
        base.PlayHitAnim(_hit);
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

    protected int AttackIdx { get; set; }

    public bool CanAttack { get { return HasTarget && TargetInAttackRange && AttackTimeCount <= 0; } }  // ���� ���� ����
    public float AttackTimeCount { get; set; } = 0;                                                     // ���� ��Ÿ��

    public override void AttackTriggerOn()
    {
        AttackTriggerOn(0);
    }
    public virtual void AttackTriggerOn(int _idx)
    {
        m_normalAttacks[_idx].SetActive(true);

        AttackObject = m_normalAttacks[_idx].GetComponent<ObjectAttackScript>();
        AttackObject.SetAttack(this, Attack);
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


    public void GetInstantHit(SkillInfo _skill, GameObject _part)
    {
        SkillScriptable data = _skill.SkillData;
        GetDamage(100);
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
    private readonly float HitGuardEndTime = 1.5f;              // ���� ���� �÷��̾� Ÿ�� �� �ǰ� �ִϸ��̼� ��� �Ⱓ

    private float HitGuardCooltime { get; set; }

    private bool HitGuarding { get; set; }                                                              // �÷��̾� ���� �� ����
    public override bool IsUnstoppable { get { return InCombat && !HitGuarding; } }                     // ���� ��� ĵ�� �Ұ�����

    public void HitGuardingPlayer()
    {
        HitGuarding = true;
        if (HitGuardCooltime > 0) { HitGuardCooltime = HitGuardEndTime; return; }
        else
        {
            HitGuardCooltime = HitGuardEndTime;
            StartCoroutine(HitGuardEnd());
        }
    }
    private IEnumerator HitGuardEnd()
    {
        while (HitGuardCooltime > 0)
        {
            HitGuardCooltime -= Time.deltaTime;
            yield return null;
        }
        HitGuarding = false;
    }


    private readonly float InfectRadius = 1.5f;
    public bool IsMelancholySource { get; private set; }
    public override void GetMelancholy(HitData _hit)
    {
        IsMelancholySource = true;
        StartCoroutine(InfectMelancholy());
        base.GetMelancholy(_hit);
    }
    public void GetMelancholy()
    {
        base.GetMelancholy(HitData.Null);
    }
    private IEnumerator InfectMelancholy()
    {
        yield return new WaitForSeconds(0.1f);
        while (!IsDead && IsMelancholy)
        {
            FindInfectTarget();
            yield return new WaitForSeconds(0.1f);
        }
        IsMelancholySource = false;
    }
    private void FindInfectTarget()
    {
        Collider[] cols = Physics.OverlapSphere(Position, InfectRadius, ValueDefine.HITTABLE_LAYER);
        List<MonsterScript> targets = new();
        foreach (Collider col in cols)
        {
            if (col.CompareTag(ValueDefine.MONSTER_TAG)) { continue; }
            MonsterScript monster = col.GetComponent<MonsterScript>();
            if(monster == null || monster.IsDead || monster.IsMelancholy || targets.Contains(monster)) { continue; }
            targets.Add(monster);
        }
        foreach(MonsterScript monster in targets) { monster.GetMelancholy(); }
    }
}
