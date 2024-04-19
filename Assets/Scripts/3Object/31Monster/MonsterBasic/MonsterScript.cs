using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms;
using UnityEngine.VFX;
using Cinemachine;
using Unity.VisualScripting;

[RequireComponent(typeof(MonsterLighter))]
public abstract partial class MonsterScript : ObjectScript, IHidable, IPoolable
{
    // ������Ʈ
    protected SkinnedMeshRenderer[] m_skinneds;           // �Ž�

    // Ǯ
    public ObjectPool<GameObject> OriginalPool { get; set; }
    public void SetPool(ObjectPool<GameObject> _pool) { OriginalPool = _pool; }
    public virtual void OnPoolGet() { IsSpawned = true; }
    public void ReleaseTopool() { m_aiPath.enabled = false; IsSpawned = false; OriginalPool.Release(gameObject); }


    // �⺻ ����
    [SerializeField]
    private MonsterScriptable m_scriptable;             // ��ũ���ͺ�� ������ �����մϴ�. ��� ������ ���⿡..
    public bool IsScriptableSet { get { return m_scriptable != null; } }
    public void SetScriptable(MonsterScriptable _scriptable) { m_scriptable = _scriptable; SetInfo(); }

    public override bool IsMonster { get { return true; } }


    // �ɷ� ����
    protected MonsterLighter m_lightReciever;
    private ObjectHPBarScript m_hpBar;
    private bool IsGettingLight { get { if (m_lightReciever == null) return false; return m_lightReciever.GettingLight; } }
    private bool IsPurifyGlowing { get; set; }                  // ������ �־�
    public void HideMonster() { m_lightReciever.HideMonster(); }
    private void CheckPurify()
    {
        if (IsGettingLight && CanPurify && !IsPurifyGlowing) 
        {
            ActivePurify();
        }
        else if ((!IsGettingLight || !CanPurify) && IsPurifyGlowing) 
        {
            InactivePurify();
        }
    }
    public virtual void ActivePurify()
    {
        foreach (SkinnedMeshRenderer smr in m_skinneds)
        {
            Material[] mats = smr.materials;
            StartCoroutine(PurifyEffect(mats, 1));
            for (int i = 0; i<mats.Length; i++)
            {
                mats[i].SetInt("_FresnelOnOff", 1);
            }
        }
        IsPurifyGlowing = true;
    }
    private IEnumerator PurifyEffect(Material[] _mats, int _on)
    {
        float counter = 0;
        while (_mats[0].GetFloat("_Fresnelpower") < 1 && 0 < _mats[0].GetFloat("_Fresnelpower"))
        {
            counter += m_dissolveRate * _on;
            for (int i = 0; i<_mats.Length; i++)
            {
                _mats[i].SetFloat("_Fresnelpower", counter);
            }
            yield return new WaitForSeconds(m_refreshRate);
        }
    }
    public virtual void InactivePurify()
    {
        foreach (SkinnedMeshRenderer smr in m_skinneds)
        {
            Material[] mats = smr.materials;
            StartCoroutine(PurifyEffect(mats, -1));
            for (int i = 0; i<mats.Length; i++)
            {
                mats[i].SetInt("_FresnelOnOff", 0);
            }
        }
        IsPurifyGlowing = false;
    }


    // ���� ����
    protected MonsterStateManager m_stateManager;
    private IMonsterState CurState { get { return m_stateManager.CurMonsterState; } }
    protected readonly IMonsterState[] m_monsterStates = new IMonsterState[(int)EMonsterState.LAST];
    public void ChangeState(EMonsterState _state) { m_stateManager.ChangeState(m_monsterStates[(int)_state]); }


    // ���� ����
    public virtual bool IsSpawned { get; protected set; } = true;
    public override float CurSpeed { get { return m_aiPath.maxSpeed; } protected set { m_aiPath.maxSpeed = value; } }
    public bool IsIdle { get { return CurState.StateEnum == EMonsterState.IDLE; } }
    public bool IsRoaming { get { return CurState.StateEnum == EMonsterState.ROAMING; } }
    public bool IsApproaching { get { return CurState.StateEnum == EMonsterState.APPROACH; } }
    public bool IsAttacking { get { return CurState.StateEnum == EMonsterState.ATTACK; } }
    public bool IsHit { get { return CurState.StateEnum == EMonsterState.HIT; } }


    // ���� ���� ������Ƽ
    public bool InCombat { get { return !IsIdle && !IsRoaming; } }                                      // ���� ������
    public bool CanAttack { get { return HasTarget && TargetInAttackRange && AttackTimeCount <= 0; } }  // ���� ���� ����
    public int NearRomaingMonsterNum { get; protected set; }                                            // �ֺ��� �ι����� ���� ��
    public bool ShouldRoam { get { return NearRomaingMonsterNum > 2; } }                                // �ι� �ؾ� ��
    public virtual bool CanPurify { get; }                                                              // ���� ���� �Ϸ�
    public bool IsPurified { get; protected set; }                                                      // ���� ���� ���¿��� ���
    public EMonsterDeathType DeathType { get; protected set; }                                          // ��� Ÿ��

    // �ǰ� ȿ�� ����
    private CameraShake m_cameraShake;
    private CinemachineImpulseSource m_impulseSource;

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
    public void LoseLight()            // ���� �׸� ���� �� ����
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
    protected void DestroyMonster()
    {
        if(OriginalPool == null)
        {
            Destroy(gameObject);
        }
        else
        {
            ReleaseTopool();
        }
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
            if(IsPurified) { DeathType = EMonsterDeathType.PURIFY; }
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
        GameObject effect = GameManager.GetEffectObj(EEffectName.MONSTER_DISSOLVE);
        effect.transform.position = Position;
        VisualEffect vfx = effect.GetComponent<VisualEffect>();
        foreach (SkinnedMeshRenderer smr in m_skinneds)
        {
            /*vfx.SetSkinnedMeshRenderer("SkinnedMeshRenderer", smr);*/
            StartCoroutine(DissolveCoroutine(smr.materials));
        }
        vfx.Play();
    }
    private IEnumerator DissolveCoroutine(Material[] _mats)
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
        DestroyMonster();
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
        m_lightReciever = GetComponent<MonsterLighter>();
        m_aiPath = GetComponent<AIPath>();

        m_cameraShake = GetComponent<CameraShake>();  // ���� �����տ� cinemachine impulse source�� CameraShake ������Ʈ�� �޾ƾ� ��
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
        ChangeState(EMonsterState.ROAMING);
    }

    public override void Awake()
    {
        base.Awake();
        SetStates();
    }
    public virtual void Update()
    {
        if(!IsSpawned) { return; }

        CurState.Proceed();

        CheckPurify();
        ProceedCooltime();
    }
}
