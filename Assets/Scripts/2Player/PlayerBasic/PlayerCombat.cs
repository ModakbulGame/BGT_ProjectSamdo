using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController
{
    // ���� �⺻
    public override bool IsUnstoppable { get { return false; } }        // ���� ��� �Ȳ���

    public override void GetHit(HitData _hit)                           // ���� ����
    {
        if (IsInvincible && !_hit.CCList.Contains(ECCType.AIRBORNE)) { return; }    // ������ ���̰� ����� �ƴϸ�
        if (!IsDead && IsGuarding && _hit.Attacker.IsMonster) { ((MonsterScript)_hit.Attacker).HitGuardingPlayer(); }
        base.GetHit(_hit);
    }
    public override void PlayHitAnim(HitData _hit)                      // �ǰ� �ִϸ��̼�
    {
        if (IsHealing) { CancelHeal(); }
        if (!_hit.CCList.Contains(ECCType.AIRBORNE) && !_hit.CCList.Contains(ECCType.KNOCKBACK))
        {
            StopMove();
        }
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
        if (PlayManager.IsPlaying) { StartCoroutine(RestartCoroutine()); }
    }
    private IEnumerator RestartCoroutine()
    {
        yield return new WaitForSeconds(3);
        PlayManager.PlayerDead();
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
    private float CurForwardDist
    {
        get
        {                                // ������ ���� ���� �ӵ�
            return ForwardDist[(int)CurWeapon.WeaponType, AttackStack-1];
        }
    }
    public bool CanAttack
    {
        get
        {                                       // ���� ����
            return AttackTrigger && CurStamina >= AttackStaminaUse && AttackStack < 3;
        }
    }
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
        if (AttackProcing) { return; }
        AttackStack++;
        SetAttackAnim(AttackStack);
        AttackProcing = true;
        GuardDelayStart();
    }
    public override void AttackTriggerOn()                              // ���� ��Ʈ ���� on
    {
        CurWeapon.AttackOn();
        AttackCreated = true;
        UseStamina(AttackStaminaUse);
    }
    public override void AttackTriggerOff()                             // ���� ��Ʈ ���� off
    {
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

    public void HitTarget()                                             // ���� ����
    {
        if (!IsOverload) { return; }
        OverloadRestoreLight();
    }


    // ��ų ����
    private EPowerName[] PowerSlot { get { return PlayManager.PowerSlot; } }                // ��ų ����
    public int PowerIdx
    {
        get
        {
            for (int i = 0; i<ValueDefine.MAX_POWER_SLOT; i++)          // ���� ��ų
            { if (PowerTriggers[i]) return i; }
            return -1;
        }
    }
    public bool CanUsePower
    {
        get
        {
            return !IsOverload && !IsOblivion && PowerIdx != -1     // ��ų ��� ���� ����
                && PowerSlot[PowerIdx] != EPowerName.LAST && PowerCooltime[PowerIdx] <= 0;
        }
    }
    public EPowerName PowerInHand { get; private set; } = EPowerName.LAST;                  // ��� ���� ��ų
    private PowerInfo PowerInfoInHand
    {
        get
        {                                               // ���� ����
            if (PowerInHand == EPowerName.LAST) return null;
            return GameManager.GetPowerInfo(PowerInHand);
        }
    }
    public int UsingPowerIdx { get; private set; } = -1;                                    // ���� ���� ��ȣ

    public bool IsRaycastPower { get { return PowerInHand == EPowerName.RANGED_KNOCKBACK1; } }

    public void ReadyPower()                                                                // ��ų ��� �غ�
    {
        UsingPowerIdx = PowerIdx;
        PowerInHand = PowerSlot[UsingPowerIdx];

        PowerStartAnim();
        if (PowerInfoInHand.HideWeapon) { HideWeapon(); }
        if (PowerInfoInHand.ShowCastingEffect) { CastingEffectOn(); }

        if (IsHealing) { CancelHeal(); }

        if (PowerInfoInHand.CastType == ECastType.SUMMON)
        {
            ShowPowerAim(PowerInfoInHand.PowerRadius, PowerInfoInHand.PowerCastRange);
        }
        if (IsRaycastPower)
        {
            PlayManager.ShowRaycastAim();
        }
        GuardDelayStart();
    }
    public void FirePower()                                                                 // ��ų ���
    {
        float coolTime = PowerInfoInHand.PowerCooltime;
        PowerCooltime[UsingPowerIdx] = coolTime;
        PlayManager.UsePowerSlot(UsingPowerIdx, coolTime);
        PowerFireAnim();
        HidePowerAim();
    }
    public void CreatePower()                                                               // ��ų ������Ʈ ����
    {
        GameObject power = GameManager.GetPowerObj(PowerInHand);
        ECastType type = PowerInfoInHand.CastType;
        power.transform.SetParent(transform);

        if (IsRaycastPower)
        {
            RaycastDone(power);
            PowerAnimDone();
            return;
        }

        switch (type)
        {
            case ECastType.MELEE:
            case ECastType.MELEE_CC:
                power.transform.localPosition = Vector3.zero;
                power.transform.localEulerAngles = Vector3.zero;
                break;
            case ECastType.RANGED:
            case ECastType.RANGED_CC:
                power.transform.localPosition = PowerInfoInHand.PowerData.PowerPrefab.transform.localPosition;
                power.transform.SetParent(null);
                ProjectilePowerScript projectile = power.GetComponentInChildren<ProjectilePowerScript>();
                projectile.SetPower(this, Attack, Magic, PlayerAimDirection);
                break;
            case ECastType.SUMMON:
                power.transform.SetParent(null);
                power.transform.position = PlayManager.TracePowerAim(Position, PowerInfoInHand.PowerCastRange);
                break;
            case ECastType.AROUND:
            case ECastType.AROUND_CC:
                power.transform.localPosition = Vector3.zero;
                power.transform.SetParent(null);
                AroundPowerScript around = power.GetComponentInChildren<AroundPowerScript>();
                around.SetPower(this, Attack, Magic);
                break;
            case ECastType.BUFF:
                TempAdjust adjust = PowerInfoInHand.PowerData.StatAdjust;
                GetAdj(adjust);
                power.transform.localPosition = Vector3.zero;
                break;
            default:
                PlayerPowerScript script = power.GetComponentInChildren<PlayerPowerScript>();
                script.SetPower(this, Attack, Magic);
                break;
        };

        PowerAnimDone();
    }
    public void CancelPower()                                                               // ��ų ���
    {
        PowerAnimDone();
    }
    public void PowerDone()                                                                 // ��ų ��� ����
    {
        HidePowerAim();
        if (PowerInfoInHand.ShowCastingEffect) { CastingEffectOff(); }
        PowerInHand = EPowerName.LAST;
        UsingPowerIdx = -1;
        ShowWeapon();
        ChangeState(EPlayerState.IDLE);
    }

    public struct RaycastTargetInfo
    {
        public GameObject Target;
        public Vector3 Point;
        public bool IsNull { get { return Target == null; } }
        public static RaycastTargetInfo Null { get { return new(null, Vector3.zero); } }
        public RaycastTargetInfo(GameObject _target, Vector3 _point) { Target = _target; Point = _point; }
    }

    private MonsterScript RaycastTarget { get; set; }
    public RaycastTargetInfo CheckRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit[] hits = Physics.RaycastAll(ray, 50, ValueDefine.HITTABLE_LAYER, QueryTriggerInteraction.Collide);
        List<RaycastHit> hitList = hits.ToList();
        hitList.Sort((elm1, elm2) => (Vector3.Distance(Position, elm1.point) < Vector3.Distance(Position, elm2.point) ? -1 : 1));

        foreach (RaycastHit hit in hitList)
        {
            if (!hit.collider.isTrigger) { continue; }
            MonsterScript monster = hit.collider.GetComponentInParent<MonsterScript>();
            if (monster == null) { continue; }
            RaycastTarget = monster;
            PlayManager.SetRaycastAimState(true);
            Debug.Log(monster);
            return new(hit.collider.gameObject, hit.point);
        }
        PlayManager.SetRaycastAimState(false);
        return RaycastTargetInfo.Null;
    }
    private void RaycastDone(GameObject _power)
    {
        RaycastTargetInfo info = CheckRaycast();
        if (info.IsNull) { return; }
        _power.transform.position = info.Point;
        _power.GetComponent<EffectScript>().SetDestroyTime(2); ;
        RaycastTarget.GetInstantHit(PowerInfoInHand, info.Target, this);
    }

    // ���� CC
    public override void GetAdj(TempAdjust _adjust)
    {
        if (_adjust.Type == EAdjType.WEAPON_CC)
        {
            GetWeaponAdj(_adjust);
            return;
        }

        base.GetAdj(_adjust);
    }

    private void GetWeaponAdj(TempAdjust _adjust)
    {
        SetWeaponCCType((ECCType)_adjust.Amount);
        for (int i = 0; i< m_buffNDebuff.Count; i++)
        {
            if (m_buffNDebuff[i].AdjType == EAdjType.WEAPON_CC)
            {
                m_buffNDebuff[i].SetAmount(_adjust.Amount);
                m_buffNDebuff[i].SetTimeCount(_adjust.Time);
                return;
            }
        }
        BuffNDebuff info = new(_adjust, ResetWeaponCC);
        m_buffNDebuff.Add(info);
    }
    private void ResetWeaponCC() { SetWeaponCCType(ECCType.NONE); }


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

    private bool CanHeal
    {
        get
        {
            return HealInHand != EPatternName.LAST && HealItemTrigger && !IsHealing &&         // ȸ�� ����
                (IsIdle || IsMoving) && HealCooltime <= 0;
        }
    }
    public bool IsHealing { get; private set; }                                                                     // ȸ�� ��    
    private EPatternName HealInHand { get { return PlayManager.CurHealPattern; } }                                  // ������ ȸ�� ������
    private float HealAmountInHand
    {
        get
        {
            if (HealInHand == EPatternName.LAST) return -1;                          // ���� ȸ����
            ItemInfo info = GameManager.GetItemInfo(new SItem(EItemType.PATTERN, (int)HealInHand));
            return ((PatternScriptable)info.ItemData).HealAmount;
        }
    }

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
        if (!IsHealing) { return; }
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

    public bool CanThrow { get { return IsUpperIdleAnim && ThrowCooltime <= 0 && ThrowItemTrigger; } }     // ��ô ���� ����
    private EThrowItemName ItemInHand { get; set; } = EThrowItemName.LAST;                  // ������ �غ� ���� ������ Enum
    private GameObject InHandPrefab { get; set; }                                           // �տ� �� ������ ������Ʈ
    public Vector3 ThrowOffset
    {
        get
        {
            Vector3 pos = transform.position;
            Vector2 dir = PlayerAimDirection;
            Vector2 right = new(dir.y, -dir.x);
            Vector2 offset = dir*TempThrowOffset.z + right*TempThrowOffset.x;
            pos += new Vector3(offset.x, TempThrowOffset.y, offset.y);
            return pos;
        }
    }                                                          // ������ ������

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
        if (InHandPrefab == null) { return; }
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

        ThrowCooltime = ThrowDelay;
    }
    public void DoneThrow()                                                                 // ������ �Ϸ�
    {
        ShowWeapon();
        CancelThrowAnim();
        ItemInHand = EThrowItemName.LAST;
        ActionDone();
    }


    // CC ����
    private IEnumerator OblivionCoroutine;
    public bool IsOblivion { get { return m_ccCount[(int)ECCType.OBLIVION] > 0; } }
    public override void GetOblivion()
    {
        m_ccCount[(int)ECCType.OBLIVION] = 10;
        // ��ų ��� �Ұ� ǥ��
        if (OblivionCoroutine == null) { OblivionCoroutine = ResetOblivion(); StartCoroutine(OblivionCoroutine); }
    }
    private IEnumerator ResetOblivion()
    {
        while (!IsDead && IsOblivion)
        {
            yield return null;
        }
        // ��ų ��� ���� ǥ��
    }

    private IEnumerator BlindCoroutine;
    public override void GetBlind()
    {
        m_ccCount[(int)ECCType.BLIND] = 10;
        PlayManager.ShowBlindMark();
        if (BlindCoroutine == null) { BlindCoroutine = ResetBlind(); StartCoroutine(BlindCoroutine); }
    }
    private IEnumerator ResetBlind()
    {
        while (!IsDead && m_ccCount[(int)ECCType.BLIND] > 0)
        {
            yield return null;
        }
        PlayManager.HideBlindMark();
    }
}
