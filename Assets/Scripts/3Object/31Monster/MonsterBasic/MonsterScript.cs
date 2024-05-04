using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;
using Cinemachine;

[RequireComponent(typeof(MonsterLighter))]
public abstract partial class MonsterScript : ObjectScript, IHidable, IPoolable
{
    // 풀
    public ObjectPool<GameObject> OriginalPool { get; set; }
    public void SetPool(ObjectPool<GameObject> _pool) { OriginalPool = _pool; }
    public virtual void OnPoolGet() { IsSpawned = true; }
    public virtual void ReleaseToPool() { m_aiPath.enabled = false; IsSpawned = false; OriginalPool.Release(gameObject); }


    // 스테이트 매니저
    protected MonsterStateManager m_stateManager;
    protected readonly IMonsterState[] m_monsterStates = new IMonsterState[(int)EMonsterState.LAST];

    private IMonsterState CurState { get { return m_stateManager.CurMonsterState; } }

    public void ChangeState(EMonsterState _state) { m_stateManager.ChangeState(m_monsterStates[(int)_state]); }


    // 몬스터 상태
    public bool InCombat { get { return !IsIdle; } }                                            // 전투 중인지
    public virtual bool IsSpawned { get; protected set; } = true;
    public bool IsIdle { get { return CurState.StateEnum == EMonsterState.IDLE; } }
    public bool IsApproaching { get { return CurState.StateEnum == EMonsterState.APPROACH; } }
    public bool IsAttacking { get { return CurState.StateEnum == EMonsterState.ATTACK; } }
    public bool IsHit { get { return CurState.StateEnum == EMonsterState.HIT; } }


    // 상태 관련 메소드
    public virtual void StartIdle()          // 로밍 시작
    {
        CurSpeed = MoveSpeed;
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
        m_hpBar.DestroyUI();                // HP바
        m_rigid.useGravity = false;         // 중력
        GetComponentInChildren<CapsuleCollider>().isTrigger = true;         // 트리거
    }


    // 사망 관련
    private float m_dissolveTime = 2;
    [SerializeField]
    private float m_dissolveDelay = 1;

    public EMonsterDeathType DeathType { get; protected set; }                                          // 사망 타입
    public virtual bool CanPurify { get; }                                                              // 성불 조건 완료
    public bool IsPurified { get { return IsGettingLight && CanPurify; } }                              // 성불 가능 상태에서 사망

    private void SetDeathType(ObjectScript _object)         // 사망 원인 설정
    {
        if(_object == null) { DeathType = EMonsterDeathType.ETC; return; }

        if (PlayManager.CheckIsPlayer(_object))
        {
            if (IsPurified) { DeathType = EMonsterDeathType.PURIFY; }
            else { DeathType = EMonsterDeathType.BY_PLAYER; }
        }
        else
        {
            DeathType = EMonsterDeathType.BY_MONSTER;
        }
    }
    public void DeathResult()           // 사망 원인에 따른 결과
    {
        if(DeathType == EMonsterDeathType.PURIFY || DeathType == EMonsterDeathType.BY_PLAYER)
        {
            DropItems();                        // 아이템 드랍
            if (!GameManager.CheckNClearMonster(MonsterEnum))
            {
                int stat = m_scriptable.DropInfo.StatPoint;
                PlayManager.AddStatPoint(stat);
            }
        }

        switch (DeathType)
        {
            case EMonsterDeathType.PURIFY:      // 성불
                PlayManager.AddPurified(1);
                break;
            case EMonsterDeathType.BY_PLAYER:   // 플레이어
                PlayManager.AddSoul(1);
                break;
            case EMonsterDeathType.BY_MONSTER:  // 몬스터

                break;
            case EMonsterDeathType.ETC:         // 기타

                break;
        }
    }
    public void DropItems()             // 아이템 드랍
    {
        List<SDropItem> drops = m_scriptable.DropInfo.Items;
        foreach(SDropItem drop in drops)
        {
            if (UnityEngine.Random.Range(0f, 1f) <= drop.Prob)
            {
                EItemType type = DataManager.IDToItemType(drop.ID);
                GameObject item = GameManager.GetDropItemPrefab(type);
                item.transform.position = Position;
                DropItemScript script = item.GetComponent<DropItemScript>();
                script.SetDropItem(drop.ID);
            }
        }
    }
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
        yield return new WaitForSeconds(m_dissolveDelay);
        float counter = 0;

        float change = 1 / m_dissolveTime;
        while (_mats[0].GetFloat("_DissolveAmount") < 1)
        {
            counter += Time.deltaTime * change;
            for (int i = 0; i<_mats.Length; i++)
            {
                _mats[i].SetFloat("_DissolveAmount", counter);
            }
            yield return null;
        }
        DestroyMonster();
    }
    protected void DestroyMonster()
    {
        if (OriginalPool == null)
        {
            Destroy(gameObject);
        }
        else
        {
            ReleaseToPool();
        }
    }


    // 플레이어 능력 관련
    protected MonsterLighter m_lightReciever;

    private bool IsGettingLight { get { if (m_lightReciever == null) return false; return m_lightReciever.GettingLight; } }
    private bool IsPurifyGlowing { get; set; }                  // 빛나고 있어

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
    public void HideMonster() { m_lightReciever.HideMonster(); }
    private void CheckPurify()
    {
        if (IsPurified && !IsPurifyGlowing)
        {
            ActivePurify();
        }
        else if (!IsPurified && IsPurifyGlowing)
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
            counter += Time.deltaTime * _on;
            for (int i = 0; i<_mats.Length; i++)
            {
                _mats[i].SetFloat("_Fresnelpower", counter);
            }
            yield return null;
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


    // UI 관련
    private ObjectHPBarScript m_hpBar;

    private void SetUI()                // UI 설정
    {
        m_hpBar = GetComponentInChildren<ObjectHPBarScript>();
        m_hpBar.SetMaxHP(MaxHP);
    }


    // 업데이트
    public override void ProcCooltime()
    {
        base.ProcCooltime();
        if (AttackTimeCount > 0) { AttackTimeCount -= Time.deltaTime; }
    }

    public override void Update()
    {
        if(!IsSpawned) { return; }
        base.Update();

        CurState.Proceed();

        CheckPurify();
    }
}
