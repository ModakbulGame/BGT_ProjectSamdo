using System;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
public class PlayerCombatInfo : ObjectCombatInfo
{
    public float MaxStamina;        // �ִ� ���׹̳�
    public float Magic;             // ���� ���ݷ�
    public float Overdrive;          // ġ��Ÿ (���� ����)
    public float Tolerance;         // ����

    public void SetCombatInfo(PlayerStatInfo _stat)
    {
        MaxHP = FunctionDefine.RoundF2(28.85f + 15 * Mathf.Sqrt(1.6f*_stat.m_health + 0.65f * _stat.m_strength));
        MaxStamina = FunctionDefine.RoundF2(64.6f + (1.5f * _stat.m_endure + _stat.m_health + 0.8f * _stat.m_intellect + 0.24f * _stat.m_rapid));
        Defense = FunctionDefine.RoundF2((0.5f * (_stat.m_endure + 0.3f * _stat.m_mental) - 6.5f)* 0.01f);
        Attack = FunctionDefine.RoundF2(1.5f * _stat.m_strength);
        Magic = FunctionDefine.RoundF2(1.5f * _stat.m_intellect);
        Overdrive = FunctionDefine.RoundF2(0.73f + (15 * Mathf.Sqrt(_stat.m_rapid) * 0.01f));
        Tolerance = FunctionDefine.RoundF2(1.2f * _stat.m_mental * 0.01f - 0.12f);
    }
    public override float GetStat(ECombatInfoName _name)
    {
        return _name switch
        {
            ECombatInfoName.MAX_HP => MaxHP,
            ECombatInfoName.MAX_STAMINA => MaxStamina,
            ECombatInfoName.DEFENSE => Defense,
            ECombatInfoName.ATTACK => Attack,
            ECombatInfoName.MAGIC => Magic,
            ECombatInfoName.OVERDRIVE => Overdrive,
            ECombatInfoName.TOLERANCE => Tolerance,
            _ => -1,
        };
    }
    public PlayerCombatInfo(PlayerStatInfo _stat)
    {
        SetCombatInfo(_stat);
    }
}

public partial class PlayerController
{
    // ���� ���� ��ġ
    public const int MAX_ATTACK = 3;                         // ���� ���� ��

    // ���� ���� ����
    public override bool IsUnstoppable { get { return false; } }        // ��Ʈ
    private bool IsInvincible { get; set; }                             // ���� ���� ����
    public void StartInvincible() { IsInvincible = true; }              // ���� ����
    public void StopInvincible() { IsInvincible = false; }              // ���� �ߴ�


    // �⺻ ����
    public override void GetHit(HitData _hit)
    {
        if (IsInvincible) { return; }           // ������ ���
        base.GetHit(_hit);
    }
    public override void PlayHitAnim()
    {
        if (!IsGuarding) 
        {
            StopMove();
            ChangeState(EPlayerState.HIT);
        }
    }
    public override void SetHP(float _hp)
    {
        base.SetHP(_hp);
        PlayManager.SetPlayerCurHP(_hp);
    }
    public override void SetDead()
    {
        base.SetDead();
        ChangeState(EPlayerState.DIE);
    }


    // ���� ����
    public int AttackStack { get; private set; }
    public bool CanAttack { get { return AttackTrigger && CurStamina >= AttackStaminaUse && AttackStack < 3; } }
    public bool AttackCreated { get; private set; }             // ���� ������Ʈ ���� (���� ��� ���� �������� �Ѿ �� ����)
    public bool AttackFinished { get; private set; }
    private bool AttackProcing { get; set; }

    private const float AttackStaminaUse = 0;
    public void StartAttack()
    {
        StopMove();
        AttackCreated = false;
        AttackProcing = false;
        AttackFinished = false;
        AttackStack = 1;
        SetAttackAnim(AttackStack);
    }
    public void ToNextAttack()
    {
        if(AttackProcing) { return; }
        AttackStack++;
        SetAttackAnim(AttackStack);
        AttackProcing = true;
    }
    public override void AttackTriggerOn()          // ���� ������ on
    {
        base.AttackTriggerOn();
        CurWeapon.AttackOn();
        AttackCreated = true;
        UseStamina(AttackStaminaUse);
    }
    public override void AttackTriggerOff()         // ���� ������ off
    {
        base.AttackTriggerOff();
        CurWeapon.AttackOff();
        AttackFinished = true;
    }

    public void ChkAttackDone()
    {
        if (!AttackProcing) { AttackDone(); }
        else { AttackProcing = false; AttackFinished = false; }
    }
    public override void AttackDone()               // ���� �ִϸ��̼� ����
    {
        AttackOffAnim();
        ChangeState(EPlayerState.IDLE);
    }
    public void BreakAttack()
    {
        AttackCreated = false;
        AttackProcing = false;
        AttackFinished = false;
        AttackDone();
        ResetAnim();
    }


    // ��ų ����
    private ESkillName[] SkillSlot { get { return GameManager.SkillSlot; } }                // ��ų ����
    public int SkillIdx { get { for (int i = 0; i<ValueDefine.MAX_SKILL_SLOT; i++) { if (SkillTriggers[i]) return i; } return -1; } }       // ���� ��ų
    public bool CanUseSkill { get { return SkillIdx != -1 && SkillSlot[SkillIdx] != ESkillName.LAST && SkillCooltime[SkillIdx] <= 0; } }    // ��ų ��� Ȯ��

    private readonly float[] SkillCooltime = new float[ValueDefine.MAX_SKILL_SLOT];         // ��ų ��Ÿ��
    public ESkillName SkillInHand { get; private set; } = ESkillName.LAST;                  // ��� ���� ��ų
    private SkillInfo SkillInfoInHand { get { if (SkillInHand == ESkillName.LAST) return null;
            return GameManager.GetSkillInfo(SkillInHand); } }                               // �� ����
    public int UsingSkillIdx { get; private set; } = -1;                                    // �� ���� ��ȣ
    public void ReadySkill()    // ��ų �غ�
    {
        UsingSkillIdx = SkillIdx;
        SkillInHand = SkillSlot[UsingSkillIdx];
        SkillStartAnim();
        
        if (SkillInfoInHand.SkillType == ESkillType.SUMMON)
        {
            ShowSkillAim(SkillInfoInHand.SkillRadius, SkillInfoInHand.SkillCastRange);
        }
    }
    public void FireSkill()             // ��ų �߻�
    {
        float coolTime = SkillInfoInHand.SkillCooltime;
        SkillCooltime[UsingSkillIdx] = coolTime;
        PlayManager.UseSkillSlot(UsingSkillIdx, coolTime);
        SkillFireAnim();
        HideSkillAim();
    }
    public void CreateSkill()           // ��ų ������Ʈ ����
    {
        GameObject prefab = GameManager.GetSkillPrefab(SkillInHand);
        GameObject skill = Instantiate(prefab, transform);
        ESkillType type = SkillInfoInHand.SkillType;
        if(type == ESkillType.RANGED || type == ESkillType.RANGED_CC || type == ESkillType.SUMMON) { skill.transform.parent = null; }
        if(type == ESkillType.SUMMON) { skill.transform.position = PlayManager.TraceSkillAim(Position, SkillInfoInHand.SkillCastRange); }
        switch (type) {
            case ESkillType.RANGED:
            case ESkillType.RANGED_CC:
                ProjectileSkillScript projectile = skill.GetComponentInChildren<ProjectileSkillScript>();
                projectile.SetSkill(this, Attack, Magic, PlayerAimDirection);
                break;
            default:
                PlayerSkillScript script = skill.GetComponentInChildren<PlayerSkillScript>();
                script.SetSkill(this, Attack, Magic);
                break;
        };

        SkillAnimDone();
    }
    public void CancelSkill()           // ��ų ���
    {
        SkillAnimDone();
    }
    public void SkillDone()             // ��ų �Ϸ�
    {
        SkillInHand = ESkillName.LAST;
        UsingSkillIdx = -1;
        HideSkillAim();
        ShowWeapon();
        ChangeState(EPlayerState.IDLE);
    }




    // ���� ����
    public void GuardUpdate()                                   // ���� ���� ������Ʈ
    {
        if (!IsIdle && !IsMoving && !IsThrowing)                // IDLE, MOVE, THROW�� �ƴϸ�
        {
            if (IsGuarding) IsGuarding = false;              // ���� ����
            if (!IsUpperIdleAnim) { QuitGuardAnim(); }       // �ִϸ��̼� �ߴ�
            if (!IsThrowing && m_anim.GetLayerWeight(UpperLayerIdx) == 1) { m_anim.SetLayerWeight(UpperLayerIdx, 0); }
            return;
        }
        if (IsGuarding) { RotateTo(PlayerAimDirection); }   // ���� ���� ���� ����
        if (!IsGuarding && GuardPressing)
        {
            GuardStart();       // ���� ����
        }
        else if (IsGuarding && !IsUpperAnimOn)
        {
            UpperAnimStart();
        }
        else if (IsGuarding && !GuardPressing)
        {
            GuardStop();        // ���� �ߴ�
        }
    }
    public void GuardStart()                                    // ���� ����
    {
        IsGuarding = true;
        GuardAnimStart();
    }
    public void GuardStop()                                     // ���� �ߴ�
    {
        IsGuarding = false;
        GuardAnimStop();
    }


    // ������ ����
    private readonly Vector3 TempThrowOffset = new(0.371f, 1.628f, 0.664f);
    private readonly Vector3 TempThrowRotation = new(-64.449f, -30.487f, 40.266f);

    public readonly int ThrowPower = 60;                        // ������ ��
    private const float ThrowDelay = 1.5f;
    private EThrowItemName ItemInHand { get; set; } = EThrowItemName.LAST;  // ������ �غ� ���� ������
    private GameObject InHandPrefab { get; set; }
    public Vector3 ThrowOffset { get {
            Vector3 pos = transform.position;
            Vector2 dir = PlayerAimDirection;
            Vector2 right = new(dir.y, -dir.x);
            Vector2 offset = dir*TempThrowOffset.z + right*TempThrowOffset.x;
            pos += new Vector3(offset.x, TempThrowOffset.y, offset.y); 
            return pos; } }                              // ������ ������
    public void ReadyThrow()                                    // ������ �غ�
    {
        EThrowItemName item = PlayManager.CurThrowItem;
        if (item == EThrowItemName.LAST) { return; }

        HideWeapon();
        SetThrowItem(item);
        ChangeState(EPlayerState.THROW);
    }
    public void SetThrowItem(EThrowItemName _item)              // ������ ���
    {
        ItemInHand = _item;

        GameObject item = PlayManager.GetThorwItemPrefab(_item);
        InHandPrefab = Instantiate(item, m_throwItemTransform);
        Destroy(InHandPrefab.GetComponent<CapsuleCollider>());
        Destroy(InHandPrefab.GetComponent<Rigidbody>());
        Destroy(InHandPrefab.GetComponent<ThrowItemScript>());
    }
    public void CancelThrow()                                   // ������ ���
    {
        PlayManager.HideThrowLine();
        DestroyInHand();
        DoneThrow();
    }
    private void DestroyInHand()
    {
        if(InHandPrefab == null) { return; }
        Destroy(InHandPrefab);
    }
    public void ThrowItem()                                     // ������ ������
    {
        PlayManager.HideThrowLine();
        ThrowAnim();
    }
    public void CreateThrowItem()                               // ������ ������ Ÿ�̹�
    {
        Vector3 force = PlayerAimVector * ThrowPower;
        GameObject item = Instantiate(PlayManager.GetThorwItemPrefab(ItemInHand), ThrowOffset, Quaternion.Euler(TempThrowRotation));
        ThrowItemScript script = item.GetComponent<ThrowItemScript>();
        script.SetFlying(force);
        DestroyInHand();
        PlayManager.UseThrowItem();
    }
    public void CheckThrowDone()                                // ������ �Ϸ� Ȯ��
    {
        if(IsThrowAnim && UpperAnimState >= 1)
        {
            DoneThrow();
        }
    }
    public void DoneThrow()                                     // ������ �Ϸ�
    {
        ShowWeapon();
        CancelThrowAnim();
        ItemInHand = EThrowItemName.LAST;
        ChangeState(EPlayerState.IDLE);
    }
}
