using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public abstract partial class MonsterScript
{
    // 전투 매니저
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
        if (!AgainstMonster && !monster.AgainstMonster) { NotTargetDamage(_hit); return true; }
        AgainstMonster = true;
        return false;
    }
    private void NotTargetDamage(HitData _hit)
    {
        float damage = _hit.Damage * (100-Defense) * 0.01f;
        GetDamage(damage, _hit.Attacker);
        Debug.Log($"{_hit.Attacker.ObjectName} => {ObjectName} {damage} 데미지");
    }


    // 기본 전투
    public override void SetHP(float _hp)
    {
        base.SetHP(_hp);
        m_hpBar.SetCurHP(CurHP);
    }                   // HP 설정
    public override void GetHit(HitData _hit)    // 맞음
    {
        if (CheckMonsterBattle(_hit)) { return; }
        SetBattleTarget(_hit.Attacker);
        SetDeathType(_hit.Attacker);
        base.GetHit(_hit);
        CheckPowerProp(_hit.Property);
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
    public void GetInstantHit(PowerInfo _skill, GameObject _part, ObjectScript _attacker)
    {
        PowerScriptable data = _skill.PowerData;
        GetDamage(100, _attacker);
    }
    public virtual void AttackedPlayer(HitData _hit) { }

    private void CheckPowerProp(EPowerProperty _prop)
    {
        if (_prop == EPowerProperty.LAST) { return; }
        GetPropHit(_prop);
    }
    public virtual void GetPropHit(EPowerProperty _prop) { }


    // 공격 관련
    [SerializeField]
    protected GameObject[] m_normalAttacks;                                      // 기본 공격 프리펍

    protected int AttackIdx { get; set; }

    public virtual bool CanAttack { get { return HasTarget && TargetInAttackRange && AttackTimeCount <= 0; } }      // 공격 가능 여부
    public float AttackTimeCount { get; set; } = 0;                                                                 // 공격 쿨타임

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


    // 스킬
    [SerializeField]
    protected ObjectAttackScript[] SkillList;
    public virtual int SkillNum { get { return 0; } }
    public float[] SkillCooltime { get; protected set; }
    public float[] SkillTimeCount { get; protected set; }
    public int CheckCurSkill
    {
        get
        {
            List<int> list = new();
            for (int i = 0; i<SkillCooltime.Length; i++) { if (SkillTimeCount[i] <= 0) list.Add(i); }
            if (list.Count > 0) { return list[UnityEngine.Random.Range(0, list.Count)]; }
            return -1;
        }
    }

    public virtual bool CanSkill => false;
    public int CurSkillIdx { get; protected set; }

    public virtual void InitSkillInfo()
    {
        SkillCooltime = new float[SkillNum];
        SkillTimeCount = new float[SkillNum];
    }
    public virtual void StartSkill()
    {
        StopMove();
    }
    public virtual void SkillOn() { }
    public virtual void SkillOff() { }
    public virtual void CreateSkill() { }
    public virtual void SkillDone()
    {
        if (CurTarget != null)
            ChangeState(EMonsterState.APPROACH);
        else
            ChangeState(EMonsterState.IDLE);
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
            Vector3 look = FunctionDefine.AngleToDir(Direction);
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

    public bool HasPath { get; set; } = true;
    public virtual void ApproachTarget()            // 타겟에게 접근
    {
        if (!HasTarget) { HasPath = false; return; }
        Vector2 dir = (CurTarget.Position2 - Position2);
        if (AttackTimeCount > 0 && TargetInAttackRange) { RotateToDir(dir, ERotateSpeed.SLOW); HasPath = false; return; }

        if (!CanAttack && dir.magnitude < FunctionDefine.Max(0.5f, AttackRange - 2)) { m_aiPath.destination = transform.position - 0.1f * new Vector3(dir.x, 0, dir.y); HasPath = true; }
        else if (dir.magnitude < AttackRange - 0.5f) { StopMove(); HasPath = false; }
        else { m_aiPath.destination = CurTarget.Position; HasPath = true; }
    }


    // 피격 관련
    public readonly float StunDelay = 1f;                       // 피격 시 경직
    private readonly float HitGuardEndTime = 1.5f;              // 가드 중인 플레이어 타격 후 피격 애니메이션 재생 기간

    private float HitGuardCooltime { get; set; }

    private bool HitGuarding { get; set; }                                                              // 플레이어 가드 중 때림
    public override bool IsUnstoppable { get { return InCombat && !HitGuarding; } }                     // 공격 모션 캔슬 불가인지

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
            if (monster == null || monster.IsDead || monster.IsMelancholy || targets.Contains(monster)) { continue; }
            targets.Add(monster);
        }
        foreach (MonsterScript monster in targets) { monster.GetMelancholy(); }
    }
}
