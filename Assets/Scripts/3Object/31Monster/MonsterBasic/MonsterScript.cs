using Cinemachine;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.VFX;

public abstract partial class MonsterScript : ObjectScript, IHidable
{
    protected SkinnedMeshRenderer[] m_skinneds;           // �Ž�

    [SerializeField]
    private MonsterScriptable m_scriptable;             // ��ũ���ͺ�� ������ �����մϴ�. ��� ������ ���⿡..
    public bool IsScriptableSet { get { return m_scriptable != null; } }
    public void SetScriptable(MonsterScriptable _scriptable) { m_scriptable = _scriptable; SetInfo(); }

    public override bool IsMonster { get { return true; } }

    // �ɷ� ����
    protected HideScript m_hideScript;
    private ObjectHPBarScript m_hpBar;
    private bool IsHiding { get { if (m_hideScript == null) return false; return m_hideScript.IsHiding; } }


    // ���� ����
    protected MonsterStateManager m_stateManager;
    private IMonsterState CurState { get { return m_stateManager.CurMonsterState; } }
    protected readonly IMonsterState[] m_monsterStates = new IMonsterState[(int)EMonsterState.LAST];
    public void ChangeState(EMonsterState _state) { m_stateManager.ChangeState(m_monsterStates[(int)_state]); }

    // ���� ����
    public override float CurSpeed { get { return m_aiPath.maxSpeed; } protected set { m_aiPath.maxSpeed = value; } }

    public bool IsIdle { get { return CurState.CurMonsterState == EMonsterState.IDLE; } }
    public bool IsRoaming { get { return CurState.CurMonsterState == EMonsterState.ROAMING; } }
    public bool IsApproaching { get { return CurState.CurMonsterState == EMonsterState.APPROACH; } }
    public bool IsAttacking { get { return CurState.CurMonsterState == EMonsterState.ATTACK; } }


    // ���� ���� ������Ƽ
    public bool InCombat { get { return !IsIdle && !IsRoaming; } }                                      // ���� ������
    public bool CanAttack { get { return HasTarget && TargetInAttackRange && AttackTimeCount <= 0; } }  // ���� ���� ����
    public int NearRomaingMonsterNum { get; protected set; }                                            // �ֺ��� �ι����� ���� ��
    public bool ShouldRoam { get { return NearRomaingMonsterNum > 2; } }                                // �ι� �ؾ� ��
    public bool Purified { get; protected set; }                                                        // ���� ���� �Ϸ�
    public EMonsterDeathType DeathType { get; protected set; }                                          // ��� Ÿ��

    // �ǰ� ȿ�� ����
    private CameraShake m_cameraShake;
    [SerializeField]
    private float m_magnitude = 0.3f;   // �ǰ� �� ��鸲 ����

    // �⺻ �޼ҵ�
    private void SetUI()                // UI ����
    {
        m_hpBar = GetComponentInChildren<ObjectHPBarScript>();
        m_hpBar.SetMaxHP(MaxHP);
    }


    // Hidable �޼ҵ�
    public void GetLight()              // ���� �޾��� �� ����
    {
        if (m_hpBar)
            m_hpBar.gameObject.SetActive(true);
    }
    public void LooseLight()            // ���� �׸� ���� �� ����
    {
        if (m_hpBar)
            m_hpBar.gameObject.SetActive(false);
    }


    // ���� ���� �޼ҵ�
    public void StartIdle()             // IDLE ����
    {
        StopMove();
        IdleAnimation();
    }
    public virtual void StartRoaming()          // �ι� ����
    {
        CurSpeed = MoveSpeed;
    }
    public void StartHit()              // �ǰ� ����
    {
        StopMove();
        HitAnimation();
    }
    public void StartDie()              // ��� ����
    {
        StopMove();
        DieAnimation();                     // �ִϸ��̼�
        StartDissolve();                    // ������
        DropItems();                        // ������ ���
        m_hpBar.DestroyUI();                // HP��
        m_rigid.useGravity = false;         // �߷�
        GetComponentInChildren<CapsuleCollider>().isTrigger = true;         // Ʈ����
    }
    public virtual void StartApproach()
    {
        CurSpeed = ApproachSpeed;
        MoveAnimation();
    }
    public virtual void StartAttack()   // ���� ����
    {
        StopMove();
        AttackAnimation();
    }


    // ���� ����
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


    // ��� ����
    private void SetDeathType(ObjectScript _object)         // ��� ���� ����
    {
        if(_object == null) { DeathType = EMonsterDeathType.ETC; return; }

        if (PlayManager.CheckIsPlayer(_object))
        {
            if(Purified) { DeathType = EMonsterDeathType.PURIFY; }
            else { DeathType = EMonsterDeathType.BY_PLAYER; }
        }
        else
        {
            DeathType = EMonsterDeathType.BY_MONSTER;
        }
    }

    public void DeathResult()           // ��� ���ο� ���� ���
    {
        switch (DeathType)
        {
            case EMonsterDeathType.PURIFY:      // ����
                PlayManager.AddPurified(1);
                break;
            case EMonsterDeathType.BY_PLAYER:   // �÷��̾�
                PlayManager.AddSoul(1);
                DropItems();
                break;
            case EMonsterDeathType.BY_MONSTER:  // ����

                break;
            case EMonsterDeathType.ETC:         // ��Ÿ

                break;
        }
    }

    public void DropItems()             // ������ ���
    {
        List<SDropItem> drops = m_scriptable.DropItemInfo;
        foreach(SDropItem drop in drops)
        {
            if (UnityEngine.Random.Range(0f, 1f) <= drop.Prob)
            {
                EItemType type = DataManager.IDToItemType(drop.ID);
                GameObject prefab = PlayManager.GetDropItemPrefab(type);
                GameObject item = Instantiate(prefab, transform.position, Quaternion.Euler(transform.eulerAngles));
                DropItemScript script = item.GetComponent<DropItemScript>();
                script.SetDropItem(drop.ID);
            }
        }
    }

    private const float m_dissolveRate = 0.0125f;
    private const float m_refreshRate = 0.025f;
    public void StartDissolve()             // dissolve vfx ȿ�� ���
    {
        GameObject effect = GameManager.GetEffect(EEffectName.MONSTER_DISSOLVE);
        effect.transform.position = Position;
        VisualEffect vfx = effect.GetComponent<VisualEffect>();
        foreach (SkinnedMeshRenderer smr in m_skinneds)
        {
            /*vfx.SetSkinnedMeshRenderer("SkinnedMeshRenderer", smr);*/
            StartCoroutine(DissolveCoroutine(effect, smr.materials));
        }
        vfx.Play();
    }
    private IEnumerator DissolveCoroutine(GameObject _effect, Material[] _mats)
    {
        float counter = 0;

        while (_mats[0].GetFloat("_DissolveAmount") < 1)
        {
            counter += m_dissolveRate;
            for (int i = 0; i<_mats.Length; i++)
            {
                _mats[i].SetFloat("_DissolveAmount", counter);
            }
            yield return new WaitForSeconds(m_refreshRate);
        }
        Destroy(gameObject);
    }


    // ���� ���� ���� ���� (�⺻ ���� Ŭ���� -> ���ͺ� ���� Ŭ����)
    protected void ReplaceState(EMonsterState _enum, IMonsterState _state)
    {
        IMonsterState origin = m_monsterStates[(int)_enum];
        Destroy((UnityEngine.Object)origin);
        m_monsterStates[(int)_enum] = _state;
    }


    // �ʱ� ����
    public override void SetComps()
    {
        base.SetComps();
        m_skinneds = GetComponentsInChildren<SkinnedMeshRenderer>();
        m_hideScript = GetComponent<HideScript>();
        m_aiPath = GetComponent<AIPath>();
    }

    public virtual void SetStates()
    {
        m_stateManager = new(this);
        m_monsterStates[(int)EMonsterState.IDLE] = gameObject.AddComponent<MonsterIdleState>();
        m_monsterStates[(int)EMonsterState.ROAMING] = gameObject.AddComponent<MonsterRoamingState>();
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

    public override void Awake()
    {
        base.Awake();
        SetStates();
    }
    public override void Start()
    {
        base.Start();
        m_cameraShake = GetComponent<CameraShake>();  // ���� �����տ� cinemachine impulse source�� CameraShake ������Ʈ�� �޾ƾ� ��
        SetUI();
        if (m_spawnPoint != null) { m_spawnPoint.AddMonster(this); }
        ChangeState(EMonsterState.ROAMING);
    }
    private void Update()
    {
        CurState.Proceed();
        //m_aiPath.SearchPath();

        ProceedCooltime();
    }
}
