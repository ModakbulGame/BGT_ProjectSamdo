using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public enum EMonsterDeathType
{
    PURIFY,
    BY_PLAYER,
    BY_MONSTER,
    ETC
}

[Serializable]
public class MonsterCombatInfo : ObjectCombatInfo
{
    public float ApproachSpeed;     // 접근 속도
    public float ViewAngle;         // 시야각
    public float ViewRange;         // 시야 범위
    public float EngageRange;       // 시야 제외 감지 범위
    public float ReturnRange;       // 접근 종료 범위
    public float AttackRange;       // 공격 범위
    public float ApproachDelay;     // 감지 후 접근 딜레이
    public float FenceRange;        // 활동 범위
    public override void SetInfo(MonsterScriptable _monster)
    {
        base.SetInfo(_monster);
        ApproachSpeed = _monster.ApproachSpeed;
        ViewAngle = _monster.ViewAngle;
        ViewRange = _monster.ViewRange;
        EngageRange = _monster.EngageRange;
        ReturnRange = _monster.ReturnRange;
        AttackRange = _monster.AttackRange;
        ApproachDelay = _monster.ApproachDelay;
        FenceRange = _monster.FenceRange;
    }
}

public abstract partial class MonsterScript
{
    // 몬스터 전투 정보
    [SerializeField]
    private MonsterCombatInfo m_combatInfo;
    public override ObjectCombatInfo CombatInfo { get { return m_combatInfo; } }    // 전투 관련 정보
    public float ApproachSpeed { get { return m_combatInfo.ApproachSpeed; } }           // 접근 속도
    public float ViewAngle { get { return m_combatInfo.ViewAngle; } }                   // 시야각
    public float ViewRange { get { return m_combatInfo.ViewRange; } }                   // 시야 범위
    public float EngageRange {get { return m_combatInfo.EngageRange; } }                // 시야 제외 감지 범위
    public float ReturnRange {get { return m_combatInfo.ReturnRange; } }                // 접근 종료 범위
    public float AttackRange {get { return m_combatInfo.AttackRange; } }                // 공격 범위
    public float ApproachDelay {get { return m_combatInfo.ApproachDelay; } }            // 감지 후 접근 딜레이
    public float FenceRange {get { return m_combatInfo.FenceRange; } }                  // 활동 범위


    [SerializeField]
    protected GameObject m_normalAttackPrefab;                                      // 기본 공격 프리펍


    // 전투 관련 변수 (추후 값 연결 필요)
    public float PathingAngle;
    public float PathingTime;
    public float AttackDuration;                // 공격 지속 시간
    public readonly float MissTargetDelay = 5f;
    public readonly float StunDelay = 1f;
    private float HitGuardEndTime = 3;


    // 전투 관련 프로퍼티
    public ObjectScript CurTarget { get; protected set; }                                               // 현재 타겟
    public bool HasTarget { get { return (CurTarget != null && !CurTarget.IsDead); } }                  // 타겟을 가지고 있는지
    public float TargetDistance { get { if (HasTarget) return Vector3.Distance(Position, CurTarget.Position); return -1; } }    // 목표와의 거리
    public bool TargetInAttackRange { get { return TargetDistance <= AttackRange; } }                   // 공격범위 내인지
    public float AttackTimeCount { get; set; } = 0;                                                     // 공격 쿨타임
    private bool HitGuarding { get; set; }                                                              // 플레이어 가드 중 때림
    public override bool IsUnstoppable { get { return InCombat && !HitGuarding; } }                     // 공격 모션 캔슬 불가인지


    [SerializeField]
    private GameObject m_tempHitEffect;
    [SerializeField]
    private GameObject m_bloodEffect;

    public virtual void ApproachTarget()            // 타겟에게 접근
    {
        if(!HasTarget) { return; }
        Vector2 dir = (CurTarget.Position2 - Position2);
        if (AttackTimeCount > 0 && TargetInAttackRange) { RotateTo(dir); return; }
        m_aiPath.destination = CurTarget.Position;
    }

    public override void CreateAttack() 
    {
        if (!PlayManager.IsPlayerGuarding) m_cameraShake.CameraShaking(m_magnitude);
    }
    public virtual void CreateNormalAttack(Vector3 _offset)
    {
        CreateNormalAttack(_offset, Attack);
    }
    public virtual void CreateNormalAttack(Vector3 _offset, float _damage)
    {
        GameObject attack = Instantiate(m_normalAttackPrefab, transform);
        attack.transform.localPosition = _offset;
        ObjectAttackScript script = attack.GetComponent<ObjectAttackScript>();
        script.SetAttack(this, _damage);
    }

    public override void AttackDone()
    {
        if (CurTarget != null)
            ChangeState(EMonsterState.APPROACH);
        else
            ChangeState(EMonsterState.IDLE);
        AttackTimeCount = 1 / AttackSpeed;
    }


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


    private void CreateBloodEffect(Vector3 _pos)
    {
        Instantiate(m_tempHitEffect, _pos, Quaternion.identity);
        Vector2 bloodDir = Position2 - new Vector2(_pos.x, _pos.z);
        float bloodAngle = FunctionDefine.VecToDeg(bloodDir);
        GameObject blood = Instantiate(m_bloodEffect, transform);
        blood.transform.eulerAngles = new(0, bloodAngle, 0);
        blood.transform.parent = null;
    }


    public override void GetHit(HitData _hit)    // 맞음
    {
        base.GetHit(_hit);
        if (IsDead) { SetDeathType(_hit.Attacker); }
        else if (CurTarget == null) { CurTarget = _hit.Attacker; }
        if(PlayManager.CheckIsPlayer(_hit.Attacker)) { CreateBloodEffect(_hit.Point); }
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

    private void ProceedCooltime()
    {
        if(AttackTimeCount > 0) { AttackTimeCount -= Time.deltaTime; }
    }
}
