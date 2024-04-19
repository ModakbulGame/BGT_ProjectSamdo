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
    // 컴포넌트
    protected SkinnedMeshRenderer[] m_skinneds;           // 매쉬

    // 풀
    public ObjectPool<GameObject> OriginalPool { get; set; }
    public void SetPool(ObjectPool<GameObject> _pool) { OriginalPool = _pool; }
    public virtual void OnPoolGet() { IsSpawned = true; }
    public void ReleaseTopool() { m_aiPath.enabled = false; IsSpawned = false; OriginalPool.Release(gameObject); }


    // 기본 정보
    [SerializeField]
    private MonsterScriptable m_scriptable;             // 스크립터블로 정보를 저장합니다. 모든 정보는 여기에..
    public bool IsScriptableSet { get { return m_scriptable != null; } }
    public void SetScriptable(MonsterScriptable _scriptable) { m_scriptable = _scriptable; SetInfo(); }

    public override bool IsMonster { get { return true; } }


    // 능력 관련
    protected MonsterLighter m_lightReciever;
    private ObjectHPBarScript m_hpBar;
    private bool IsGettingLight { get { if (m_lightReciever == null) return false; return m_lightReciever.GettingLight; } }
    private bool IsPurifyGlowing { get; set; }                  // 빛나고 있어
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


    // 상태 관리
    protected MonsterStateManager m_stateManager;
    private IMonsterState CurState { get { return m_stateManager.CurMonsterState; } }
    protected readonly IMonsterState[] m_monsterStates = new IMonsterState[(int)EMonsterState.LAST];
    public void ChangeState(EMonsterState _state) { m_stateManager.ChangeState(m_monsterStates[(int)_state]); }


    // 몬스터 상태
    public virtual bool IsSpawned { get; protected set; } = true;
    public override float CurSpeed { get { return m_aiPath.maxSpeed; } protected set { m_aiPath.maxSpeed = value; } }
    public bool IsIdle { get { return CurState.StateEnum == EMonsterState.IDLE; } }
    public bool IsRoaming { get { return CurState.StateEnum == EMonsterState.ROAMING; } }
    public bool IsApproaching { get { return CurState.StateEnum == EMonsterState.APPROACH; } }
    public bool IsAttacking { get { return CurState.StateEnum == EMonsterState.ATTACK; } }
    public bool IsHit { get { return CurState.StateEnum == EMonsterState.HIT; } }


    // 상태 관련 프로퍼티
    public bool InCombat { get { return !IsIdle && !IsRoaming; } }                                      // 전투 중인지
    public bool CanAttack { get { return HasTarget && TargetInAttackRange && AttackTimeCount <= 0; } }  // 공격 가능 여부
    public int NearRomaingMonsterNum { get; protected set; }                                            // 주변에 로밍중인 몬스터 수
    public bool ShouldRoam { get { return NearRomaingMonsterNum > 2; } }                                // 로밍 해야 함
    public virtual bool CanPurify { get; }                                                              // 성불 조건 완료
    public bool IsPurified { get; protected set; }                                                      // 성불 가능 상태에서 사망
    public EMonsterDeathType DeathType { get; protected set; }                                          // 사망 타입

    // 피격 효과 구현
    private CameraShake m_cameraShake;
    private CinemachineImpulseSource m_impulseSource;

    // 기본 메소드
    private void SetUI()                // UI 설정
    {
        m_hpBar = GetComponentInChildren<ObjectHPBarScript>();
        m_hpBar.SetMaxHP(MaxHP);
    }


    // Hidable 메소드
    public void GetLight()              // 빛을 받았을 때 실행
    {
        if (m_hpBar)
            m_hpBar.gameObject.SetActive(true);
    }
    public void LoseLight()            // 빛을 그만 받을 때 실행
    {
        if (m_hpBar)
            m_hpBar.gameObject.SetActive(false);
    }


    // 상태 관련 메소드
    public void StartIdle()             // IDLE 시작
    {
        StopMove();
        IdleAnimation();
    }
    public virtual void StartRoaming()          // 로밍 시작
    {
        CurSpeed = MoveSpeed;
    }
    public void StartHit()              // 피격 시작
    {
        StopMove();
        HitAnimation();
    }
    public void StartDie()              // 사망 시작
    {
        StopMove();
        DieAnimation();                     // 애니메이션
        StartDissolve();                    // 디졸브
        DropItems();                        // 아이템 드랍
        m_hpBar.DestroyUI();                // HP바
        m_rigid.useGravity = false;         // 중력
        GetComponentInChildren<CapsuleCollider>().isTrigger = true;         // 트리거
    }
    public virtual void StartApproach()
    {
        CurSpeed = ApproachSpeed;
        MoveAnimation();
    }
    public virtual void StartAttack()   // 공격 시작
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


    // 감지 관련
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


    // 사망 관련
    private void SetDeathType(ObjectScript _object)         // 사망 원인 설정
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

    public void DeathResult()           // 사망 원인에 따른 결과
    {
        switch (DeathType)
        {
            case EMonsterDeathType.PURIFY:      // 성불
                PlayManager.AddPurified(1);
                break;
            case EMonsterDeathType.BY_PLAYER:   // 플레이어
                PlayManager.AddSoul(1);
                DropItems();
                break;
            case EMonsterDeathType.BY_MONSTER:  // 몬스터

                break;
            case EMonsterDeathType.ETC:         // 기타

                break;
        }
    }

    public void DropItems()             // 아이템 드랍
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
    public void StartDissolve()             // dissolve vfx 효과 재생
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


    // 몬스터 세부 상태 설정 (기본 상태 클래스 -> 몬스터별 상태 클래스)
    protected void ReplaceState(EMonsterState _enum, IMonsterState _state)
    {
        IMonsterState origin = m_monsterStates[(int)_enum];
        Destroy((UnityEngine.Object)origin);
        m_monsterStates[(int)_enum] = _state;
    }


    // 초기 설정
    public override void SetComps()
    {
        base.SetComps();
        m_skinneds = GetComponentsInChildren<SkinnedMeshRenderer>();
        m_lightReciever = GetComponent<MonsterLighter>();
        m_aiPath = GetComponent<AIPath>();

        m_cameraShake = GetComponent<CameraShake>();  // 몬스터 프리팹에 cinemachine impulse source와 CameraShake 컴포넌트를 달아야 함
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
