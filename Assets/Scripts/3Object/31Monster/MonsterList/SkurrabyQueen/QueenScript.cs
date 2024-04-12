using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public enum EQueenSkillName
{
    CREATE_SKURRABY,
    SPIT_POISON,
    LAST
}

public class QueenScript : MonsterScript
{
    [SerializeField]
    private GameObject m_skurrabyPrefab;

    private ObjectPool<GameObject> m_skurrabyPool;
    private List<SkurrabyScript> m_createdSkurraby = new();

    [SerializeField]
    private VisualEffect m_poisonVFX;

    private const int MAX_SKURRABY = 3;
    private const float SPIT_ROTATION = 120;
    private float SKILL_DELAY = 10;
    public readonly float EvadeRange = 3;

    private int CurSkurraby { get; set; } = 0;
    private Vector3 SkurrabyOffset = new(0, 1.5f, 2.75f);

    public EQueenSkillName SkillIdx { get; private set; }

    private readonly float[] m_skillCooltime = new float[] { 30, 20 };

    private readonly float[] SkillCoolCount = new float[(int)EQueenSkillName.LAST];
    public bool CanUseSkill { get { return CanCreateSkurraby || CanSpitPoison; } }
    private bool CanCreateSkurraby { get { return SkillCoolCount[(int)EQueenSkillName.CREATE_SKURRABY] <= 0 && CurSkurraby < MAX_SKURRABY; } }
    private bool CanSpitPoison { get { return SkillCoolCount[(int)EQueenSkillName.SPIT_POISON] <= 0; } }

    public bool IsSpitting { get; private set; }

    public void EvadeQueen()
    {
        StopMove();
        LookTarget();
        Vector3 dir = (Position - CurTarget.Position).normalized * 3;
        m_rigid.velocity = dir;
    }

    public override void StartAttack()
    {
        StopMove();
        if (CanCreateSkurraby)
        {
            SkillIdx = EQueenSkillName.CREATE_SKURRABY;
            m_anim.SetTrigger("SKILL1");
        }
        else if (CanSpitPoison)
        {
            SkillIdx = EQueenSkillName.SPIT_POISON;
            m_anim.SetTrigger("SKILL2");
        }
        else return;
        int skill = (int)SkillIdx;
        SkillCoolCount[skill] = m_skillCooltime[skill];
        if (SkillCoolCount[1-skill] <= SKILL_DELAY / 2) { SkillCoolCount[1-skill] = SKILL_DELAY; }
    }
    public override void CreateAttack()
    {
        CreateSkurraby();
    }
    public override void AttackTriggerOn() 
    {
        IsSpitting = true;
        m_poisonVFX.Play();
    }
    public override void AttackTriggerOff() 
    {
        IsSpitting = false;
        m_poisonVFX.Stop();
    }
    public void CreateSkurraby()
    {
        if(CurSkurraby >= MAX_SKURRABY) { return; }
        GameObject skurraby = m_skurrabyPool.Get();
        skurraby.transform.localPosition = SkurrabyOffset;
        Vector2 dir = FunctionDefine.DegToVec(Rotation);
        SkurrabyScript script = skurraby.GetComponent<SkurrabyScript>();
        script.SkurrabySpawned(dir, CurTarget);
        m_createdSkurraby.Add(script);
        skurraby.transform.SetParent(null);
    }
    public void SkillSpinQueen()
    {
        float rot = Rotation + Time.deltaTime * SPIT_ROTATION / 3;
        RotateTo(rot);
    }
    public override void AttackDone()
    {
        base.AttackDone();
    }

    // 초기 설정
    private void InitPool()
    {
        m_skurrabyPool = new(OnPoolCreate, OnPoolGet, OnPoolRelease, OnPoolDestroy, true, MAX_SKURRABY, MAX_SKURRABY);
        for(int i = 0; i<MAX_SKURRABY; i++) { GameObject skurraby = OnPoolCreate(); skurraby.GetComponent<SkurrabyScript>().OnPoolRelease(); }
    }
    private GameObject OnPoolCreate()
    {
        GameObject skurraby = Instantiate(m_skurrabyPrefab, transform);
        skurraby.GetComponent<SkurrabyScript>().SetPool(m_skurrabyPool);
        return skurraby;
    }
    private void OnPoolGet(GameObject _skurraby) { _skurraby.SetActive(true); }
    private void OnPoolRelease(GameObject _skurraby) { _skurraby.transform.SetParent(transform); _skurraby.SetActive(false); }
    private void OnPoolDestroy(GameObject _skurraby) { Destroy(_skurraby); }

    public override void SetStates()
    {
        base.SetStates();
        ReplaceState(EMonsterState.APPROACH, gameObject.AddComponent<QueenApproachState>());
        ReplaceState(EMonsterState.ATTACK, gameObject.AddComponent<QueenAttackState>());
    }
    public override void Awake()
    {
        base.Awake();
        InitPool();
    }

    public override void Update()
    {
        base.Update();
        for (int i = 0; i<(int)EQueenSkillName.LAST; i++)
        {
            if (SkillCoolCount[i] > 0) { SkillCoolCount[i] -= Time.deltaTime; }
            if (SkillCoolCount[i] < 0) { SkillCoolCount[i] = 0; }
        }
    }
}
