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
    public float ApproachSpeed;     // ���� �ӵ�
    public float ViewAngle;         // �þ߰�
    public float ViewRange;         // �þ� ����
    public float EngageRange;       // �þ� ���� ���� ����
    public float ReturnRange;       // ���� ���� ����
    public float AttackRange;       // ���� ����
    public float ApproachDelay;     // ���� �� ���� ������
    public float FenceRange;        // Ȱ�� ����
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
    // ������Ʈ
    protected SkinnedMeshRenderer[] m_skinneds;           // �Ž�


    // �⺻ ����
    [SerializeField]
    private MonsterScriptable m_scriptable;             // ��ũ���ͺ�� ������ �����մϴ�. ��� ������ ���⿡..
    public bool IsScriptableSet { get { return m_scriptable != null; } }
    public void SetScriptable(MonsterScriptable _scriptable) { m_scriptable = _scriptable; SetInfo(); }     // ��ũ���ͺ� �Է�
    public EMonsterName MonsterEnum { get { return m_scriptable.MonsterEnum; } }                            // enum
    public EMonsterType MonsterType { get { return m_scriptable.MonsterType; } }                            // Ÿ��

    public override void ApplyHPUI()
    {
        m_hpBar.SetMaxHP(MaxHP);
        m_hpBar.SetCurHP(CurHP);
    }

    public override void SetMoveMultiplier(float _multiplier)           // �̵� �ӵ� ���� ����
    {
        base.SetMoveMultiplier(_multiplier);
        m_aiPath.maxSpeed = MoveSpeed;
    }


    // ���� ����
    [SerializeField]
    private MonsterCombatInfo m_combatInfo;
    public override ObjectCombatInfo CombatInfo { get { return m_combatInfo; } }    // ���� ���� ����
    public float ApproachSpeed { get { return m_combatInfo.ApproachSpeed; } }           // ���� �ӵ�
    public float ViewAngle { get { return m_combatInfo.ViewAngle; } }                   // �þ߰�
    public float ViewRange { get { return m_combatInfo.ViewRange; } }                   // �þ� ����
    public float EngageRange { get { return m_combatInfo.EngageRange; } }                // �þ� ���� ���� ����
    public float ReturnRange { get { return m_combatInfo.ReturnRange; } }                // ���� ���� ����
    public virtual float AttackRange { get { return m_combatInfo.AttackRange; } }        // ���� ����
    public float ApproachDelay { get { return m_combatInfo.ApproachDelay; } }            // ���� �� ���� ������
    public float FenceRange { get { return m_combatInfo.FenceRange; } }                  // Ȱ�� ����


    // ��� ����
    public override bool IsMonster { get { return true; } }


    // �ʱ� ����
    protected void ReplaceState(EMonsterState _enum, IMonsterState _state)  // ���� ���� ���� ���� (�⺻ ���� Ŭ���� -> ���ͺ� ���� Ŭ����)
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