using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController
{
    // ���� �⺻
    public override bool IsUnstoppable { get { return false; } }        // ���� ��� �Ȳ���

    public override void GetHit(HitData _hit)                           // ���� ����
    {
        if (IsInvincible) { return; }           // ������ ���
        base.GetHit(_hit);
    }
    public override void PlayHitAnim()                                  // �ǰ� �ִϸ��̼�
    {
        if (IsHealing) { CancelHeal(); }
        StopMove();
        HitAnimation();
        ChangeState(EPlayerState.HIT);
    }
    public override void SetHP(float _hp)                               // HP ����
    {
        base.SetHP(_hp);
        PlayManager.SetPlayerCurHP(_hp);
    }
    public override void SetDead()                                      // ���� ����
    {
        base.SetDead();
        if (IsHealing) { CancelHeal(); }
        ChangeState(EPlayerState.DIE);
    }


    // ���� ����
    private bool IsInvincible { get; set; }                             // ���� ���� ����

    public void StartInvincible() { IsInvincible = true; }              // ���� ����
    public void StopInvincible() { IsInvincible = false; }              // ���� �ߴ�



    // ���� ����
    public const int MAX_ATTACK = 3;                                    // ���� ���� ��
    private readonly float AttackStaminaUse = 0;                        // �⺻���� ���׹̳� �Ҹ�
    private readonly float[,] ForwardDist = new float[3, 3] {           // ����, ���� �� ���� �ӵ�
        { 1.5f, 1.5f, 2 },
        { 3, 1, 3 },
        { 1.5f, 1.5f, 2 }
    };

    public int AttackStack { get; private set; }                        // ���� ���� ���� (1, 2, 3)
    private float CurForwardDist { get {                                // ������ ���� ���� �ӵ�
            return ForwardDist[(int)CurWeapon.WeaponType, AttackStack-1]; } }
    public bool CanAttack { get {                                       // ���� ����
            return AttackTrigger && CurStamina >= AttackStaminaUse && AttackStack < 3; } }    
    public bool AttackCreated { get; private set; }                     // ���� ������Ʈ ���� (���� ���� ���� ����)
    public bool AttackFinished { get; private set; }                    // ���� �Ϸ� (���� ���� ����)
    private bool AttackProcing { get; set; }                            // ���� �̾��

    public void StartAttack()                                           // ���� ���� ����
    {
        StopMove();
        AttackCreated = false;
        AttackProcing = false;
        AttackFinished = false;
        AttackStack = 1;
        SetAttackAnim(AttackStack);
        if (IsHealing) { CancelHeal(); }
        GuardDelayStart();
    }
    public void ToNextAttack()                                          // ���� �������� ����
    {
        if(AttackProcing) { return; }
        AttackStack++;
        SetAttackAnim(AttackStack);
        AttackProcing = true;
        GuardDelayStart();
    }
    public override void AttackTriggerOn()                              // ���� ��Ʈ ���� on
    {
        base.AttackTriggerOn();
        CurWeapon.AttackOn();
        AttackCreated = true;
        UseStamina(AttackStaminaUse);
    }
    public override void AttackTriggerOff()                             // ���� ��Ʈ ���� off
    {
        base.AttackTriggerOff();
        CurWeapon.AttackOff();
        AttackFinished = true;
    }
    public void ChkAttackDone()                                         // ���� ���� �� ���� or IDLE �Ǵ�
    {
        if (!AttackProcing) { AttackDone(); }
        else { AttackProcing = false; AttackFinished = false; }
    }
    public override void AttackDone()                                   // ���� ����
    {
        AttackOffAnim();
        ChangeState(EPlayerState.IDLE);
    }
    public void BreakAttack()                                           // ���� �ߴ�
    {
        AttackCreated = false;
        AttackProcing = false;
        AttackFinished = false;
        AttackDone();
        ResetAnim();
    }
    public void AttackForward()                                         // ���� �� ����
    {
        Vector3 forward = transform.forward * CurForwardDist;
        Vector2 forward2 = new(forward.x, forward.z);
        ForceMove(forward2);
    }


    // ��ų ����
    private ESkillName[] SkillSlot { get { return GameManager.SkillSlot; } }                // ��ų ����
    public int SkillIdx { get { for (int i = 0; i<ValueDefine.MAX_SKILL_SLOT; i++)          // ���� ��ų
            { if (SkillTriggers[i]) return i; } return -1; } }
    public bool CanUseSkill { get { return SkillIdx != -1                                   // ��ų ��� ���� ����
                && SkillSlot[SkillIdx] != ESkillName.LAST && SkillCooltime[SkillIdx] <= 0; } }
    public ESkillName SkillInHand { get; private set; } = ESkillName.LAST;                  // ��� ���� ��ų
    private SkillInfo SkillInfoInHand { get {                                               // ���� ����
            if (SkillInHand == ESkillName.LAST) return null;
            return GameManager.GetSkillInfo(SkillInHand); } }                               
    public int UsingSkillIdx { get; private set; } = -1;                                    // ���� ���� ��ȣ

    public void ReadySkill()                                                                // ��ų ��� �غ�
    {
        UsingSkillIdx = SkillIdx;
        SkillInHand = SkillSlot[UsingSkillIdx];
        if (SkillInHand != ESkillName.BUFF)
            SkillStartAnim();
        else if(SkillInHand == ESkillName.BUFF)
            BuffStartAnim();


        if (IsHealing) { CancelHeal(); }

        if (SkillInfoInHand.SkillType == ESkillType.SUMMON)
        {
            ShowSkillAim(SkillInfoInHand.SkillRadius, SkillInfoInHand.SkillCastRange);
        }
        GuardDelayStart();
    }
    public void FireSkill()                                                                 // ��ų ���
    {
        float coolTime = SkillInfoInHand.SkillCooltime;
        SkillCooltime[UsingSkillIdx] = coolTime;
        PlayManager.UseSkillSlot(UsingSkillIdx, coolTime);
        SkillFireAnim();
        HideSkillAim();
    }
    public void CreateSkill()                                                               // ��ų ������Ʈ ����
    {
        GameObject skill = GameManager.GetSkillObj(SkillInHand);
        ESkillType type = SkillInfoInHand.SkillType;
        skill.transform.SetParent(transform); 
        
        switch (type) {
            case ESkillType.MELEE:
            case ESkillType.MELEE_CC:
                skill.transform.localPosition = Vector3.zero;
                skill.transform.localEulerAngles = Vector3.zero;
                break;
            case ESkillType.RANGED:
            case ESkillType.RANGED_CC:
                skill.transform.localPosition = Vector3.up;
                skill.transform.SetParent(null);
                ProjectileSkillScript projectile = skill.GetComponentInChildren<ProjectileSkillScript>();
                projectile.SetSkill(this, Attack, Magic, PlayerAimDirection);
                break;
            case ESkillType.SUMMON:
                skill.transform.SetParent(null);
                skill.transform.position = PlayManager.TraceSkillAim(Position, SkillInfoInHand.SkillCastRange);
                break;
            case ESkillType.AROUND:
            case ESkillType.AROUND_CC:
                skill.transform.localPosition = Vector3.zero;
                skill.transform.SetParent(null);
                AroundSkillScript around=skill.GetComponentInChildren<AroundSkillScript>();
                around.SetSkill(this, Attack, Magic);
                break;
            case ESkillType.BUFF:
                StatAdjust adjust = SkillInfoInHand.SkillData.StatAdjust;
                GetStatAdjust(adjust);
                BuffSkillScript buff=skill.GetComponentInChildren<BuffSkillScript>();
                buff.SetSkill(this,Attack, Magic);
                skill.transform.localPosition = Vector3.zero;
                break;
            default:
                PlayerSkillScript script = skill.GetComponentInChildren<PlayerSkillScript>();
                script.SetSkill(this, Attack, Magic);
                break;
        };

        SkillAnimDone();
    }
    public void CancelSkill()                                                               // ��ų ���
    {
        SkillAnimDone();
    }
    public void SkillDone()                                                                 // ��ų ��� ����
    {
        SkillInHand = ESkillName.LAST;
        UsingSkillIdx = -1;
        HideSkillAim();
        ShowWeapon();
        ChangeState(EPlayerState.IDLE);
    }

    // ���� ����
    private readonly float GuardDelay = 0.8f;

    public bool CanGaurd { get { return GuardPressing && GuardCooltime <= 0; } }
    public void GuardStart()                                                                // ���� ����
    {

    }
    public void GuardStop()                                                                 // ���� �ߴ�
    {

    }
    public void GuardDone()
    {
        ActionDone();
        GuardDelayStart();
    }
    public void GuardDelayStart()
    {
        GuardCooltime = GuardDelay;
    }


    // ȸ�� ����
    private readonly float HealDelay = 5;                                                                           // ȸ�� ������

    private bool CanHeal { get { return HealInHand != EPatternName.LAST && HealItemTrigger && !IsHealing &&         // ȸ�� ����
                (IsIdle || IsMoving) && HealCooltime <= 0; } }
    public bool IsHealing { get; private set; }                                                                     // ȸ�� ��    
    private EPatternName HealInHand { get { return PlayManager.CurHealPattern; } }                                  // ������ ȸ�� ������
    private float HealAmountInHand { get { if (HealInHand == EPatternName.LAST) return -1;                          // ���� ȸ����
            ItemInfo info = GameManager.GetItemInfo(new SItem(EItemType.PATTERN, (int)HealInHand));
            return ((PatternScriptable)info.ItemData).HealAmount; } }

    public void HealUpdate()                                                                                        // ȸ�� ���� Ȯ��
    {
        if (CanHeal)
        {
            HealAnimStart();
            HealCooltime = HealDelay;
            IsHealing = true;
        }
    }
    public void UseHealItem()
    {
        if(!IsHealing) { return; }
        HealObj(HealAmountInHand);
        PlayManager.UseHealPattern();
        CreateHealEffect();
    }
    private void CreateHealEffect()
    {
        GameObject heal = GameManager.GetEffectObj(EEffectName.HEAL);
        heal.transform.SetParent(transform);
        heal.transform.localPosition = Vector3.zero;
    }
    public void CancelHeal()
    {
        CancelHealAnim();
        IsHealing = false;
    }
    public void HealDone()                                                                                          // ȸ�� �Ϸ�(�ִϸ��̼�)
    {
        HealAnimDone();
        IsHealing = false;
    }


    // ������ ����
    public readonly int ThrowPower = 60;                                                    // ������ ��
    private readonly float ThrowDelay = 1.5f;                                               // ������ ������

    private readonly Vector3 TempThrowOffset = new(0.371f, 1.628f, 0.664f);                 // �ӽ� ������ ���� ������
    private readonly Vector3 TempThrowRotation = new(-64.449f, -30.487f, 40.266f);          // �ӽ� ������ ������Ʈ ȸ��

    public bool CanThrow { get { return IsUpperIdleAnim && ThrowItemTrigger; } }            // ��ô ���� ����
    private EThrowItemName ItemInHand { get; set; } = EThrowItemName.LAST;                  // ������ �غ� ���� ������ Enum
    private GameObject InHandPrefab { get; set; }                                           // �տ� �� ������ ������Ʈ
    public Vector3 ThrowOffset { get {
            Vector3 pos = transform.position;
            Vector2 dir = PlayerAimDirection;
            Vector2 right = new(dir.y, -dir.x);
            Vector2 offset = dir*TempThrowOffset.z + right*TempThrowOffset.x;
            pos += new Vector3(offset.x, TempThrowOffset.y, offset.y); 
            return pos; } }                                                          // ������ ������

    public void ReadyThrow()                                                                // ������ �غ�
    {
        EThrowItemName item = PlayManager.CurThrowItem;
        if (item == EThrowItemName.LAST) { return; }

        HideWeapon();
        SetThrowItem(item);
        ChangeState(EPlayerState.THROW);
    }
    public void SetThrowItem(EThrowItemName _item)                                          // ������ ���
    {
        ItemInHand = _item;

        GameObject item = GameManager.GetThorwItemPrefab(_item);
        item.transform.SetParent(m_throwItemTransform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = TempThrowRotation;
        InHandPrefab = item;
        Destroy(InHandPrefab.GetComponent<CapsuleCollider>());
        Destroy(InHandPrefab.GetComponent<Rigidbody>());
        Destroy(InHandPrefab.GetComponent<ThrowItemScript>());
    }
    public void CancelThrow()                                                               // ������ ���
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
    public void ThrowItem()                                                                 // ������ ������
    {
        PlayManager.HideThrowLine();
        ThrowAnim();
    }
    public void CreateThrowItem()                                                           // ���� ������ ����
    {
        Vector3 force = PlayerAimVector * ThrowPower;
        GameObject item = GameManager.GetThorwItemPrefab(ItemInHand);
        item.transform.SetParent(transform);
        item.transform.localPosition = TempThrowOffset;
        item.transform.localEulerAngles = TempThrowOffset;
        item.transform.SetParent(null);

        ThrowItemScript script = item.GetComponent<ThrowItemScript>();
        script.SetFlying(force);
        DestroyInHand();
        PlayManager.UseThrowItem();
    }
    public void DoneThrow()                                                                 // ������ �Ϸ�
    {
        ShowWeapon();
        CancelThrowAnim();
        ItemInHand = EThrowItemName.LAST;
        ActionDone();
    }


    // CC ����
    public override void GetBlind(HitData _hit)
    {
        if (IsBlind) { m_ccCount[(int)ECCType.BLIND] = 10; return; }
        PlayManager.ShowBlindMark();
        StartCoroutine(BlindCotouine());
    }
    private IEnumerator BlindCotouine()
    {
        while (!IsDead && m_ccCount[(int)ECCType.BLIND] > 0)
        {
            m_ccCount[(int)ECCType.BLIND] -= Time.deltaTime;
            yield return null;
        }
        PlayManager.HideBlindMark();
        m_ccCount[(int)ECCType.BLIND] = 0;
    }
}
