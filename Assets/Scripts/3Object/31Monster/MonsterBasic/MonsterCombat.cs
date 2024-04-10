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

public abstract partial class MonsterScript
{
    // ���� ���� ����
    [SerializeField]
    private MonsterCombatInfo m_combatInfo;
    public override ObjectCombatInfo CombatInfo { get { return m_combatInfo; } }    // ���� ���� ����
    public float ApproachSpeed { get { return m_combatInfo.ApproachSpeed; } }           // ���� �ӵ�
    public float ViewAngle { get { return m_combatInfo.ViewAngle; } }                   // �þ߰�
    public float ViewRange { get { return m_combatInfo.ViewRange; } }                   // �þ� ����
    public float EngageRange {get { return m_combatInfo.EngageRange; } }                // �þ� ���� ���� ����
    public float ReturnRange {get { return m_combatInfo.ReturnRange; } }                // ���� ���� ����
    public float AttackRange {get { return m_combatInfo.AttackRange; } }                // ���� ����
    public float ApproachDelay {get { return m_combatInfo.ApproachDelay; } }            // ���� �� ���� ������
    public float FenceRange {get { return m_combatInfo.FenceRange; } }                  // Ȱ�� ����


    [SerializeField]
    protected GameObject m_normalAttackPrefab;                                      // �⺻ ���� ������


    // ���� ���� ���� (���� �� ���� �ʿ�)
    public float PathingAngle;
    public float PathingTime;
    public float AttackDuration;                // ���� ���� �ð�
    public readonly float MissTargetDelay = 5f;
    public readonly float StunDelay = 1f;
    private float HitGuardEndTime = 3;


    // ���� ���� ������Ƽ
    public ObjectScript CurTarget { get; protected set; }                                               // ���� Ÿ��
    public bool HasTarget { get { return (CurTarget != null && !CurTarget.IsDead); } }                  // Ÿ���� ������ �ִ���
    public float TargetDistance { get { if (HasTarget) return Vector3.Distance(Position, CurTarget.Position); return -1; } }    // ��ǥ���� �Ÿ�
    public bool TargetInAttackRange { get { return TargetDistance <= AttackRange; } }                   // ���ݹ��� ������
    public float AttackTimeCount { get; set; } = 0;                                                     // ���� ��Ÿ��
    private bool HitGuarding { get; set; }                                                              // �÷��̾� ���� �� ����
    public override bool IsUnstoppable { get { return InCombat && !HitGuarding; } }                     // ���� ��� ĵ�� �Ұ�����


    [SerializeField]
    private GameObject m_tempHitEffect;
    [SerializeField]
    private GameObject m_bloodEffect;

    public virtual void ApproachTarget()            // Ÿ�ٿ��� ����
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


    public override void GetHit(HitData _hit)    // ����
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
