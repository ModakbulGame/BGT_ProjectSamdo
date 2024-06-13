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

    [SerializeField]
    private VisualEffect m_poisonVFX;                               // �� VFX

    private const int MAX_SKURRABY = 3;                             // �ִ� ��ȯ ����
    private readonly float SkillDelay = 6;                          // ��ų �ѹ� ���� ���� ��ų����
    public readonly float EvadeRange = 3;                           // ȸ�� �Ÿ�

    private readonly float SpitRange = 7.5f;                        // �� �Ѹ��� �Ÿ� ����
    private readonly float SpitAngle = 60;                          // �� �Ѹ��� ���� ����
    private readonly float SpitDelay = 0.5f;                        // �� �Ѹ��� ����

    private int CurSkurraby { get; set; } = 0;                      // ��ȯ�� ���� ��
    private Vector3 SkurrabyOffset = new(0, 1.5f, 2.75f);

    public EQueenSkillName SkillIdx { get; private set; }           // ���� ��ų

    private readonly float[] m_skillCooltime = new float[] { 20, 12 };

    private readonly float[] SkillCoolCount = new float[(int)EQueenSkillName.LAST];
    public bool CanUseSkill { get { return CanCreateSkurraby || CanSpitPoison; } }  // ��ų ��� ����
    private bool CanCreateSkurraby { get { return SkillCoolCount[(int)EQueenSkillName.CREATE_SKURRABY] <= 0 && CurSkurraby < MAX_SKURRABY; } }
    private bool CanSpitPoison { get { return SkillCoolCount[(int)EQueenSkillName.SPIT_POISON] <= 0; } }

    public bool IsSpitting { get; private set; }                    // �� �մ� ��

    public float AngleToPlayer
    {
        get
        {
            Vector2 dir = (Position2 - PlayManager.PlayerPos2).normalized;
            float rot = FunctionDefine.VecToDeg(dir);
            Vector2 forward = new(transform.forward.x, transform.forward.z);
            float fRot = FunctionDefine.VecToDeg(forward);
            float gap = rot - fRot;
            if (gap <= -360) { gap += 360; } else if (gap >= 360) { gap -= 360; }
            return gap;
        }
    }


    public void EvadeQueen()                                        // ȸ�� �⵿
    {
        StopMove();
        LookTarget();
        Vector3 dir = (Position - CurTarget.Position).normalized * 3;
        m_rigid.velocity = dir;
    }

    public override void StartAttack()                              // ���� ����
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
        if (SkillCoolCount[1-skill] <= SkillDelay) { SkillCoolCount[1-skill] = SkillDelay; }
    }
    public override void CreateAttack()                             // ���� �����
    {
        CreateSkurraby();
    }
    public override void AttackTriggerOn()                          // �� �ձ� ����
    {
        IsSpitting = true;
        m_poisonVFX.Play();
        StartCoroutine(SpittingCoroutine());
    }
    public override void AttackTriggerOff()                         // �� �ձ� �ߴ�
    {
        IsSpitting = false;
        m_poisonVFX.Stop();
    }
    public void CreateSkurraby()                                    // ���� ���� �����
    {
        if (CurSkurraby >= MAX_SKURRABY) { return; }
        GameObject skurraby = m_skurrabyPool.Get();
        skurraby.transform.localPosition = SkurrabyOffset;
        Vector2 dir = FunctionDefine.DegToVec(Rotation);
        SkurrabyScript script = skurraby.GetComponent<SkurrabyScript>();
        script.SkurrabySpawned(dir, CurTarget);
        skurraby.transform.SetParent(null);
    }
    private IEnumerator SpittingCoroutine()                         // �� �ձ� �ڷ�ƾ
    {
        yield return new WaitForSeconds(SpitDelay);
        while (IsSpitting)
        {
            SpitPoison();
            yield return new WaitForSeconds(SpitDelay);
        }
    }
    private void SpitPoison()                                       // �� ���� �ձ�
    {
        Collider[] cols = Physics.OverlapSphere(Position, SpitRange, ValueDefine.HITTABLE_LAYER);
        foreach (Collider col in cols)
        {
            PlayerController player = col.GetComponentInParent<PlayerController>();
            if(player == null || AngleToPlayer > SpitAngle) { continue; }
            player.GetBlind();
            break;
        }
    }
    public override void AttackDone()                               // ���� �Ϸ�
    {
        base.AttackDone();
    }


    // �ʱ� ����
    private void InitPool()
    {
        m_skurrabyPool = new(OnPoolCreate, OnPoolGet, OnPoolRelease, OnPoolDestroy, true, MAX_SKURRABY, MAX_SKURRABY);
        for (int i = 0; i<MAX_SKURRABY; i++) { GameObject skurraby = OnPoolCreate(); skurraby.GetComponent<SkurrabyScript>().ReleaseToPool(); }
    }
    private GameObject OnPoolCreate()
    {
        GameObject skurraby = Instantiate(m_skurrabyPrefab, transform);
        skurraby.GetComponent<SkurrabyScript>().SetPool(m_skurrabyPool);
        return skurraby;
    }
    private void OnPoolGet(GameObject _skurraby) 
    {
        _skurraby.SetActive(true);
        _skurraby.GetComponent<SkurrabyScript>().OnSpawned();
    }
    private void OnPoolRelease(GameObject _skurraby)
    {
        _skurraby.GetComponent<SkurrabyScript>().ResetSkurraby();
        _skurraby.transform.SetParent(transform);
        _skurraby.SetActive(false);
    }
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
