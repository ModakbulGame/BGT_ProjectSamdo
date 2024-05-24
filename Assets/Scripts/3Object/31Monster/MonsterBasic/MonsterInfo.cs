using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
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

public partial class MonsterScript
{
    // 컴포넌트
    protected SkinnedMeshRenderer[] m_skinneds;           // 매쉬


    // 기본 정보
    [SerializeField]
    private MonsterScriptable m_scriptable;             // 스크립터블로 정보를 저장합니다. 모든 정보는 여기에..
    public bool IsScriptableSet { get { return m_scriptable != null; } }
    public void SetScriptable(MonsterScriptable _scriptable) { m_scriptable = _scriptable; SetInfo(); }     // 스크립터블 입력
    public EMonsterName MonsterEnum { get { return m_scriptable.MonsterEnum; } }                            // enum
    public EMonsterType MonsterType { get { return m_scriptable.MonsterType; } }                            // 타입

    public override void ApplyHPUI()
    {
        m_hpBar.SetMaxHP(MaxHP);
        m_hpBar.SetCurHP(CurHP);
    }

    public override void SetMoveMultiplier(float _multiplier)           // 이동 속도 배율 설정
    {
        base.SetMoveMultiplier(_multiplier);
        m_aiPath.maxSpeed = MoveSpeed;
    }


    // 전투 정보
    [SerializeField]
    private MonsterCombatInfo m_combatInfo;
    public override ObjectCombatInfo CombatInfo { get { return m_combatInfo; } }    // 전투 관련 정보
    public float ApproachSpeed { get { return m_combatInfo.ApproachSpeed; } }           // 접근 속도
    public float ViewAngle { get { return m_combatInfo.ViewAngle; } }                   // 시야각
    public float ViewRange { get { return m_combatInfo.ViewRange; } }                   // 시야 범위
    public float EngageRange { get { return m_combatInfo.EngageRange; } }                // 시야 제외 감지 범위
    public float ReturnRange { get { return m_combatInfo.ReturnRange; } }                // 접근 종료 범위
    public virtual float AttackRange { get { return m_combatInfo.AttackRange; } }        // 공격 범위
    public float ApproachDelay { get { return m_combatInfo.ApproachDelay; } }            // 감지 후 접근 딜레이
    public float FenceRange { get { return m_combatInfo.FenceRange; } }                  // 활동 범위


    // 상속 정보
    public override bool IsMonster { get { return true; } }


    // 초기 설정
    protected void ReplaceState(EMonsterState _enum, IMonsterState _state)  // 몬스터 세부 상태 설정 (기본 상태 클래스 -> 몬스터별 상태 클래스)
    {
        IMonsterState origin = m_monsterStates[(int)_enum];
        Destroy((UnityEngine.Object)origin);
        m_monsterStates[(int)_enum] = _state;
    }

    public override void SetComps()
    {
        base.SetComps();
        m_skinneds = GetComponentsInChildren<SkinnedMeshRenderer>();
        m_lightReciever = GetComponent<MonsterLighter>();
        m_battleManager = GetComponent<MonsterBattler>();
        m_aiPath = GetComponent<AIPath>();
        DissolveColor = m_skinneds[0].materials[0].GetColor("_Dissolvecolor");
    }

    public virtual void SetStates()
    {
        m_stateManager = new(this);
        m_monsterStates[(int)EMonsterState.IDLE] = gameObject.AddComponent<MonsterIdleState>();
        m_monsterStates[(int)EMonsterState.APPROACH] = gameObject.AddComponent<MonsterApproachState>();
        m_monsterStates[(int)EMonsterState.ATTACK] = gameObject.AddComponent<MonsterAttackState>();
        m_monsterStates[(int)EMonsterState.HIT] = gameObject.AddComponent<MonsterHitState>();
        m_monsterStates[(int)EMonsterState.DIE] = gameObject.AddComponent<MonsterDieState>();
    }

    public override void SetInfo()
    {
        m_baseInfo.SetInfo(m_scriptable);
        m_combatInfo = new MonsterCombatInfo();
        m_combatInfo.SetInfo(m_scriptable);
    }

    public virtual void OnEnable()
    {
        base.Start();
        SetUI();
        if (m_spawnPoint != null) { m_spawnPoint.AddMonster(this); }
        if (AttackObject == null) { SetAttackObject(); }
        StartCoroutine(WaitSpawned());
    }
    public virtual IEnumerator WaitSpawned()
    {
        while (!IsSpawned)
        {
            yield return null;
        }
        m_aiPath.enabled = true;
        ChangeState(EMonsterState.IDLE);
    }

    public override void Awake()
    {
        base.Awake();
        SetStates();
    }
}