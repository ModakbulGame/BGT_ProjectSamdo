using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController
{
    // 전투 기본
    public override bool IsUnstoppable { get { return false; } }        // 공격 모션 안끊김

    public override void GetHit(HitData _hit)                           // 공격 맞음
    {
        if (IsInvincible && !_hit.CCList.Contains(ECCType.AIRBORNE)) { return; }    // 구르는 중이고 에어본이 아니면
        if (!IsDead && IsGuarding && _hit.Attacker.IsMonster) { ((MonsterScript)_hit.Attacker).HitGuardingPlayer(); }
        base.GetHit(_hit);
    }
    public override void PlayHitAnim(HitData _hit)                      // 피격 애니메이션
    {
        if (IsHealing) { CancelHeal(); }
        if (!_hit.CCList.Contains(ECCType.AIRBORNE) && !_hit.CCList.Contains(ECCType.KNOCKBACK))
        {
            StopMove();
        }
        HitAnimation();
        ChangeState(EPlayerState.HIT);
    }
    public override void SetHP(float _hp)                               // HP 설정
    {
        base.SetHP(_hp);
        PlayManager.SetPlayerCurHP(_hp);
    }
    public override void SetDead()                                      // 죽음 설정
    {
        base.SetDead();
        if (IsHealing) { CancelHeal(); }
        ChangeState(EPlayerState.DIE);
    }


    // 무적 관련
    private bool IsInvincible { get; set; }                             // 무적 상태 여부

    public void StartInvincible() { IsInvincible = true; }              // 무적 시작
    public void StopInvincible() { IsInvincible = false; }              // 무적 중단



    // 공격 관련
    public const int MAX_ATTACK = 3;                                    // 공격 연격 수
    private readonly float AttackStaminaUse = 0;                        // 기본공격 스테미나 소모
    private readonly float[,] ForwardDist = new float[3, 3] {           // 무기, 순서 별 전진 속도
        { 1.5f, 1.5f, 2 },
        { 3, 1, 3 },
        { 1.5f, 1.5f, 2 }
    };

    public int AttackStack { get; private set; }                        // 현재 공격 순서 (1, 2, 3)
    private float CurForwardDist
    {
        get
        {                                // 순서에 따른 전진 속도
            return ForwardDist[(int)CurWeapon.WeaponType, AttackStack-1];
        }
    }
    public bool CanAttack
    {
        get
        {                                       // 공격 가능
            return AttackTrigger && CurStamina >= AttackStaminaUse && AttackStack < 3;
        }
    }
    public bool AttackCreated { get; private set; }                     // 공격 오브젝트 생성 (다음 공격 가능 기준)
    public bool AttackFinished { get; private set; }                    // 공격 완료 (가드 이행 기준)
    private bool AttackProcing { get; set; }                            // 연격 이어갈지

    public void StartAttack()                                           // 공격 상태 시작
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
    public void ToNextAttack()                                          // 다음 공격으로 이행
    {
        if (AttackProcing) { return; }
        AttackStack++;
        SetAttackAnim(AttackStack);
        AttackProcing = true;
        GuardDelayStart();
    }
    public override void AttackTriggerOn()                              // 무기 히트 판정 on
    {
        base.AttackTriggerOn();
        CurWeapon.AttackOn();
        AttackCreated = true;
        UseStamina(AttackStaminaUse);
    }
    public override void AttackTriggerOff()                             // 무기 히트 판정 off
    {
        base.AttackTriggerOff();
        CurWeapon.AttackOff();
        AttackFinished = true;
    }
    public void ChkAttackDone()                                         // 공격 종료 후 연격 or IDLE 판단
    {
        if (!AttackProcing) { AttackDone(); }
        else { AttackProcing = false; AttackFinished = false; }
    }
    public override void AttackDone()                                   // 공격 종료
    {
        AttackOffAnim();
        ChangeState(EPlayerState.IDLE);
    }
    public void BreakAttack()                                           // 공격 중단
    {
        AttackCreated = false;
        AttackProcing = false;
        AttackFinished = false;
        AttackDone();
        ResetAnim();
    }
    public void AttackForward()                                         // 공격 시 전진
    {
        Vector3 forward = transform.forward * CurForwardDist;
        Vector2 forward2 = new(forward.x, forward.z);
        ForceMove(forward2);
    }

    public void HitTarget()                                             // 몬스터 때림
    {
        if (!IsOverload) { return; }
        OverloadRestoreLight();
    }


    // 스킬 관련
    private ESkillName[] SkillSlot { get { return GameManager.SkillSlot; } }                // 스킬 슬롯
    public int SkillIdx
    {
        get
        {
            for (int i = 0; i<ValueDefine.MAX_SKILL_SLOT; i++)          // 눌린 스킬
            { if (SkillTriggers[i]) return i; }
            return -1;
        }
    }
    public bool CanUseSkill
    {
        get
        {
            return !IsOverload && !IsOblivion && SkillIdx != -1     // 스킬 사용 가능 여부
                && SkillSlot[SkillIdx] != ESkillName.LAST && SkillCooltime[SkillIdx] <= 0;
        }
    }
    public ESkillName SkillInHand { get; private set; } = ESkillName.LAST;                  // 사용 중인 스킬
    private SkillInfo SkillInfoInHand
    {
        get
        {                                               // ㄴ의 정보
            if (SkillInHand == ESkillName.LAST) return null;
            return GameManager.GetSkillInfo(SkillInHand);
        }
    }
    public int UsingSkillIdx { get; private set; } = -1;                                    // ㄴ의 슬롯 번호

    public bool IsRaycastSkill { get { return SkillInHand == ESkillName.RANGED_KNOCKBACK1; } }

    public void ReadySkill()                                                                // 스킬 사용 준비
    {
        UsingSkillIdx = SkillIdx;
        SkillInHand = SkillSlot[UsingSkillIdx];

        SkillStartAnim();
        if (SkillInfoInHand.HideWeapon) { HideWeapon(); }

        if (IsHealing) { CancelHeal(); }

        if (SkillInfoInHand.CastType == ECastType.SUMMON)
        {
            ShowSkillAim(SkillInfoInHand.SkillRadius, SkillInfoInHand.SkillCastRange);
        }
        if (IsRaycastSkill)
        {
            PlayManager.ShowRaycastAim();
        }
        GuardDelayStart();
    }
    public void FireSkill()                                                                 // 스킬 사용
    {
        float coolTime = SkillInfoInHand.SkillCooltime;
        SkillCooltime[UsingSkillIdx] = coolTime;
        PlayManager.UseSkillSlot(UsingSkillIdx, coolTime);
        SkillFireAnim();
        HideSkillAim();
    }
    public void CreateSkill()                                                               // 스킬 오브젝트 생성
    {
        GameObject skill = GameManager.GetSkillObj(SkillInHand);
        ECastType type = SkillInfoInHand.CastType;
        skill.transform.SetParent(transform);

        if (IsRaycastSkill)
        {
            RaycastDone(skill);
            SkillAnimDone();
            return;
        }

        switch (type)
        {
            case ECastType.MELEE:
            case ECastType.MELEE_CC:
                skill.transform.localPosition = Vector3.zero;
                skill.transform.localEulerAngles = Vector3.zero;
                break;
            case ECastType.RANGED:
            case ECastType.RANGED_CC:
                skill.transform.localPosition = Vector3.up;
                skill.transform.SetParent(null);
                ProjectileSkillScript projectile = skill.GetComponentInChildren<ProjectileSkillScript>();
                projectile.SetSkill(this, Attack, Magic, PlayerAimDirection);
                break;
            case ECastType.SUMMON:
                skill.transform.SetParent(null);
                skill.transform.position = PlayManager.TraceSkillAim(Position, SkillInfoInHand.SkillCastRange);
                break;
            case ECastType.AROUND:
            case ECastType.AROUND_CC:
                skill.transform.localPosition = Vector3.zero;
                skill.transform.SetParent(null);
                AroundSkillScript around = skill.GetComponentInChildren<AroundSkillScript>();
                around.SetSkill(this, Attack, Magic);
                break;
            case ECastType.BUFF:
                TempAdjust adjust = SkillInfoInHand.SkillData.StatAdjust;
                GetAdj(adjust);
                skill.transform.localPosition = Vector3.zero;
                break;
            default:
                PlayerSkillScript script = skill.GetComponentInChildren<PlayerSkillScript>();
                script.SetSkill(this, Attack, Magic);
                break;
        };

        SkillAnimDone();
    }
    public void CancelSkill()                                                               // 스킬 취소
    {
        SkillAnimDone();
    }
    public void SkillDone()                                                                 // 스킬 사용 종료
    {
        HideSkillAim();
        SkillInHand = ESkillName.LAST;
        UsingSkillIdx = -1;
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
    private void RaycastDone(GameObject _skill)
    {
        RaycastTargetInfo info = CheckRaycast();
        if (info.IsNull) { return; }
        _skill.transform.position = info.Point;
        _skill.GetComponent<EffectScript>().SetDestroyTime(2); ;
        RaycastTarget.GetInstantHit(SkillInfoInHand, info.Target);
    }

    // 무기 CC
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


    // 가드 관련
    private readonly float GuardDelay = 0.8f;

    public bool CanGaurd { get { return GuardPressing && GuardCooltime <= 0; } }
    public void GuardStart()                                                                // 가드 시작
    {

    }
    public void GuardStop()                                                                 // 가드 중단
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


    // 회복 관련
    private readonly float HealDelay = 5;                                                                           // 회복 딜레이

    private bool CanHeal
    {
        get
        {
            return HealInHand != EPatternName.LAST && HealItemTrigger && !IsHealing &&         // 회복 가능
                (IsIdle || IsMoving) && HealCooltime <= 0;
        }
    }
    public bool IsHealing { get; private set; }                                                                     // 회복 중    
    private EPatternName HealInHand { get { return PlayManager.CurHealPattern; } }                                  // 장착된 회복 아이템
    private float HealAmountInHand
    {
        get
        {
            if (HealInHand == EPatternName.LAST) return -1;                          // ㄴ의 회복량
            ItemInfo info = GameManager.GetItemInfo(new SItem(EItemType.PATTERN, (int)HealInHand));
            return ((PatternScriptable)info.ItemData).HealAmount;
        }
    }

    public void HealUpdate()                                                                                        // 회복 여부 확인
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
    public void HealDone()                                                                                          // 회복 완료(애니메이션)
    {
        HealAnimDone();
        IsHealing = false;
    }


    // 던지기 관련
    public readonly int ThrowPower = 60;                                                    // 던지기 힘
    private readonly float ThrowDelay = 1.5f;                                               // 던지기 딜레이

    private readonly Vector3 TempThrowOffset = new(0.371f, 1.628f, 0.664f);                 // 임시 던지기 생성 오프셋
    private readonly Vector3 TempThrowRotation = new(-64.449f, -30.487f, 40.266f);          // 임시 던지기 오브젝트 회전

    public bool CanThrow { get { return IsUpperIdleAnim && ThrowItemTrigger; } }            // 투척 가능 여부
    private EThrowItemName ItemInHand { get; set; } = EThrowItemName.LAST;                  // 던지기 준비 중인 아이템 Enum
    private GameObject InHandPrefab { get; set; }                                           // 손에 든 아이템 오브젝트
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
    }                                                          // 던지기 오프셋

    public void ReadyThrow()                                                                // 던지기 준비
    {
        EThrowItemName item = PlayManager.CurThrowItem;
        if (item == EThrowItemName.LAST) { return; }

        HideWeapon();
        SetThrowItem(item);
        ChangeState(EPlayerState.THROW);
    }
    public void SetThrowItem(EThrowItemName _item)                                          // 아이템 들기
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
    public void CancelThrow()                                                               // 던지기 취소
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
    public void ThrowItem()                                                                 // 아이템 던지기
    {
        PlayManager.HideThrowLine();
        ThrowAnim();
    }
    public void CreateThrowItem()                                                           // 던질 아이템 생성
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
    public void DoneThrow()                                                                 // 던지기 완료
    {
        ShowWeapon();
        CancelThrowAnim();
        ItemInHand = EThrowItemName.LAST;
        ActionDone();
    }


    // CC 관련
    private IEnumerator OblivionCoroutine;
    public bool IsOblivion { get { return m_ccCount[(int)ECCType.OBLIVION] > 0; } }
    public override void GetOblivion()
    {
        m_ccCount[(int)ECCType.OBLIVION] = 10;
        // 스킬 사용 불가 표시
        if (OblivionCoroutine == null) { OblivionCoroutine = ResetOblivion(); StartCoroutine(OblivionCoroutine); }
    }
    private IEnumerator ResetOblivion()
    {
        while (!IsDead && IsOblivion)
        {
            yield return null;
        }
        // 스킬 사용 가능 표시
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
