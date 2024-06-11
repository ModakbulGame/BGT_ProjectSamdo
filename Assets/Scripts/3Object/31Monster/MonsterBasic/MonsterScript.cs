using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

[RequireComponent(typeof(MonsterLighter), typeof(MonsterBattler))]
public abstract partial class MonsterScript : ObjectScript, IHidable, IPoolable
{
    // Ǯ
    public ObjectPool<GameObject> OriginalPool { get; set; }
    public void SetPool(ObjectPool<GameObject> _pool) { OriginalPool = _pool; }
    public virtual void OnPoolGet() { OnSpawned(); }
    public virtual void ReleaseToPool() { OriginalPool.Release(gameObject); }


    // �ִ�
    public void StartMoveAnim() { m_anim.SetBool("IS_MOVING", true); }
    public void StopMoveAnim() { m_anim.SetBool("IS_MOVING", false); }


    // ������Ʈ �Ŵ���
    protected MonsterStateManager m_stateManager;
    protected readonly IMonsterState[] m_monsterStates = new IMonsterState[(int)EMonsterState.LAST];

    private IMonsterState CurState { get { return m_stateManager.CurMonsterState; } }

    public void ChangeState(EMonsterState _state) { m_stateManager.ChangeState(m_monsterStates[(int)_state]); }


    // ���� ����
    public bool InCombat { get { return !IsIdle; } }                                            // ���� ������
    public virtual bool IsSpawned { get; protected set; } = false;
    public bool IsIdle { get { if (!IsSpawned) { return true; } return CurState.StateEnum == EMonsterState.IDLE; } }
    public bool IsApproaching { get { return CurState.StateEnum == EMonsterState.APPROACH; } }
    public bool IsAttacking { get { return CurState.StateEnum == EMonsterState.ATTACK; } }
    public bool IsSkilling { get { return CurState.StateEnum == EMonsterState.SKILL; } }
    public bool IsHit { get { return CurState.StateEnum == EMonsterState.HIT; } }

    public override bool IsVoid => MonsterType == EMonsterType.NORMAL && base.IsVoid;


    // ���� ���� �޼ҵ�
    [SerializeField]
    private bool HasSpeedAttack = true;

    public virtual void StartIdle()          // �ι� ����
    {
        CurSpeed = MoveSpeed;
    }
    public virtual void StartApproach()
    {
        CurSpeed = ApproachSpeed;
    }
    public virtual void StartAttack()   // ���� ����
    {
        StopMove();
        AttackAnimation();
        if (HasSpeedAttack)
        {
            bool random = (UnityEngine.Random.Range(0, 2) == 0) ? true : false;
            m_anim.SetBool("SPEED_ATTACK", random);
        }
        m_impulseSource.GenerateImpulse(3.0f);
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
        m_hpBar.DestroyUI();                // HP��
        m_rigid.useGravity = false;         // �߷�
        GetComponentInChildren<CapsuleCollider>().isTrigger = true;         // Ʈ����

        if (PlayManager.CheckRequiredQuestObject(ObjectName))
            PlayManager.DoObjectQuest(ObjectName, 1);
    }


    // ��� ����
    private float m_dissolveTime = 2;
    [SerializeField]
    private float m_dissolveDelay = 1;

    public EMonsterDeathType DeathType { get; protected set; }                                          // ��� Ÿ��
    public virtual bool CanPurify { get; }                                                              // ���� ���� �Ϸ�
    public bool IsPurified { get { return IsGettingLight && CanPurify; } }                              // ���� ���� ���¿��� ���

    private void SetDeathType(ObjectScript _object)         // ��� ���� ����
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
    public void DeathResult()           // ��� ���ο� ���� ���
    {
        if(DeathType == EMonsterDeathType.PURIFY || DeathType == EMonsterDeathType.BY_PLAYER)
        {
            DropItems();                        // ������ ���
            if (!GameManager.CheckNClearMonster(MonsterEnum))
            {
                int stat = m_scriptable.DropInfo.StatPoint;
                PlayManager.AddStatPoint(stat);
            }
        }

        switch (DeathType)
        {
            case EMonsterDeathType.PURIFY:      // ����
                PlayManager.AddPurified(1);
                break;
            case EMonsterDeathType.BY_PLAYER:   // �÷��̾�
                PlayManager.AddSoul(1);
                break;
            case EMonsterDeathType.BY_MONSTER:  // ����

                break;
            case EMonsterDeathType.ETC:         // ��Ÿ

                break;
        }
    }
    public void DropItems()             // ������ ���
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
    public virtual void StartDissolve()             // dissolve vfx ȿ�� ���
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


    // �÷��̾� �ɷ� ����
    protected MonsterLighter m_lightReciever;

    protected Color DissolveColor { get; private set; }

    private bool IsGettingLight { get { if (m_lightReciever == null) return false; return m_lightReciever.GettingLight; } }
    private bool IsPurifyGlowing { get; set; }                  // ������ �־�

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


    // UI ����
    private ObjectHPBarScript m_hpBar;

    protected void SetUI()                // UI ����
    {
        m_hpBar = GetComponentInChildren<ObjectHPBarScript>();
        m_hpBar.SetMaxHP(MaxHP);
    }

    protected void ShowHPBar()
    {
        m_hpBar.gameObject.SetActive(true);
        ApplyHPUI();
    }
    protected void HideHPBar()
    {
        m_hpBar.gameObject.SetActive(false);
    }


    // ������Ʈ
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
