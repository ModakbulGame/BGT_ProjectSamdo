using System;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
public class PlayerCombatInfo : ObjectCombatInfo
{
    public float MaxStamina;        // 최대 스테미나
    public float Magic;             // 마법 공격력
    public float Overdrive;          // 치명타 (삭제 예정)
    public float Tolerance;         // 내성

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
    // 전투 관련 수치
    public const int MAX_ATTACK = 3;                         // 공격 연격 수

    // 전투 관련 상태
    public override bool IsUnstoppable { get { return false; } }        // 히트
    private bool IsInvincible { get; set; }                             // 무적 상태 여부
    public void StartInvincible() { IsInvincible = true; }              // 무적 시작
    public void StopInvincible() { IsInvincible = false; }              // 무적 중단


    // 기본 전투
    public override void GetHit(HitData _hit)
    {
        if (IsInvincible) { return; }           // 무적일 경우
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


    // 공격 관련
    public int AttackStack { get; private set; }
    public bool CanAttack { get { return AttackTrigger && CurStamina >= AttackStaminaUse && AttackStack < 3; } }
    public bool AttackCreated { get; private set; }             // 공격 오브젝트 생성 (참일 경우 다음 공격으로 넘어갈 수 있음)
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
    public override void AttackTriggerOn()          // 무기 데미지 on
    {
        base.AttackTriggerOn();
        CurWeapon.AttackOn();
        AttackCreated = true;
        UseStamina(AttackStaminaUse);
    }
    public override void AttackTriggerOff()         // 무기 데미지 off
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
    public override void AttackDone()               // 공격 애니메이션 종료
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


    // 스킬 관련
    private ESkillName[] SkillSlot { get { return GameManager.SkillSlot; } }                // 스킬 슬롯
    public int SkillIdx { get { for (int i = 0; i<ValueDefine.MAX_SKILL_SLOT; i++) { if (SkillTriggers[i]) return i; } return -1; } }       // 눌린 스킬
    public bool CanUseSkill { get { return SkillIdx != -1 && SkillSlot[SkillIdx] != ESkillName.LAST && SkillCooltime[SkillIdx] <= 0; } }    // 스킬 사용 확인

    private readonly float[] SkillCooltime = new float[ValueDefine.MAX_SKILL_SLOT];         // 스킬 쿨타임
    public ESkillName SkillInHand { get; private set; } = ESkillName.LAST;                  // 사용 중인 스킬
    private SkillInfo SkillInfoInHand { get { if (SkillInHand == ESkillName.LAST) return null;
            return GameManager.GetSkillInfo(SkillInHand); } }                               // 의 정보
    public int UsingSkillIdx { get; private set; } = -1;                                    // 의 슬롯 번호
    public void ReadySkill()    // 스킬 준비
    {
        UsingSkillIdx = SkillIdx;
        SkillInHand = SkillSlot[UsingSkillIdx];
        SkillStartAnim();
        
        if (SkillInfoInHand.SkillType == ESkillType.SUMMON)
        {
            ShowSkillAim(SkillInfoInHand.SkillRadius, SkillInfoInHand.SkillCastRange);
        }
    }
    public void FireSkill()             // 스킬 발사
    {
        float coolTime = SkillInfoInHand.SkillCooltime;
        SkillCooltime[UsingSkillIdx] = coolTime;
        PlayManager.UseSkillSlot(UsingSkillIdx, coolTime);
        SkillFireAnim();
        HideSkillAim();
    }
    public void CreateSkill()           // 스킬 오브젝트 생성
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
    public void CancelSkill()           // 스킬 취소
    {
        SkillAnimDone();
    }
    public void SkillDone()             // 스킬 완료
    {
        SkillInHand = ESkillName.LAST;
        UsingSkillIdx = -1;
        HideSkillAim();
        ShowWeapon();
        ChangeState(EPlayerState.IDLE);
    }




    // 가드 관련
    public void GuardUpdate()                                   // 가드 상태 업데이트
    {
        if (!IsIdle && !IsMoving && !IsThrowing)                // IDLE, MOVE, THROW가 아니면
        {
            if (IsGuarding) IsGuarding = false;              // 가드 해제
            if (!IsUpperIdleAnim) { QuitGuardAnim(); }       // 애니메이션 중단
            if (!IsThrowing && m_anim.GetLayerWeight(UpperLayerIdx) == 1) { m_anim.SetLayerWeight(UpperLayerIdx, 0); }
            return;
        }
        if (IsGuarding) { RotateTo(PlayerAimDirection); }   // 가드 동안 에임 조절
        if (!IsGuarding && GuardPressing)
        {
            GuardStart();       // 가드 시작
        }
        else if (IsGuarding && !IsUpperAnimOn)
        {
            UpperAnimStart();
        }
        else if (IsGuarding && !GuardPressing)
        {
            GuardStop();        // 가드 중단
        }
    }
    public void GuardStart()                                    // 가드 시작
    {
        IsGuarding = true;
        GuardAnimStart();
    }
    public void GuardStop()                                     // 가드 중단
    {
        IsGuarding = false;
        GuardAnimStop();
    }


    // 던지기 관련
    private readonly Vector3 TempThrowOffset = new(0.371f, 1.628f, 0.664f);
    private readonly Vector3 TempThrowRotation = new(-64.449f, -30.487f, 40.266f);

    public readonly int ThrowPower = 60;                        // 던지기 힘
    private const float ThrowDelay = 1.5f;
    private EThrowItemName ItemInHand { get; set; } = EThrowItemName.LAST;  // 던지기 준비 중인 아이템
    private GameObject InHandPrefab { get; set; }
    public Vector3 ThrowOffset { get {
            Vector3 pos = transform.position;
            Vector2 dir = PlayerAimDirection;
            Vector2 right = new(dir.y, -dir.x);
            Vector2 offset = dir*TempThrowOffset.z + right*TempThrowOffset.x;
            pos += new Vector3(offset.x, TempThrowOffset.y, offset.y); 
            return pos; } }                              // 던지기 오프셋
    public void ReadyThrow()                                    // 던지기 준비
    {
        EThrowItemName item = PlayManager.CurThrowItem;
        if (item == EThrowItemName.LAST) { return; }

        HideWeapon();
        SetThrowItem(item);
        ChangeState(EPlayerState.THROW);
    }
    public void SetThrowItem(EThrowItemName _item)              // 아이템 들기
    {
        ItemInHand = _item;

        GameObject item = PlayManager.GetThorwItemPrefab(_item);
        InHandPrefab = Instantiate(item, m_throwItemTransform);
        Destroy(InHandPrefab.GetComponent<CapsuleCollider>());
        Destroy(InHandPrefab.GetComponent<Rigidbody>());
        Destroy(InHandPrefab.GetComponent<ThrowItemScript>());
    }
    public void CancelThrow()                                   // 던지기 취소
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
    public void ThrowItem()                                     // 아이템 던지기
    {
        PlayManager.HideThrowLine();
        ThrowAnim();
    }
    public void CreateThrowItem()                               // 실제로 던지는 타이밍
    {
        Vector3 force = PlayerAimVector * ThrowPower;
        GameObject item = Instantiate(PlayManager.GetThorwItemPrefab(ItemInHand), ThrowOffset, Quaternion.Euler(TempThrowRotation));
        ThrowItemScript script = item.GetComponent<ThrowItemScript>();
        script.SetFlying(force);
        DestroyInHand();
        PlayManager.UseThrowItem();
    }
    public void CheckThrowDone()                                // 던지기 완료 확인
    {
        if(IsThrowAnim && UpperAnimState >= 1)
        {
            DoneThrow();
        }
    }
    public void DoneThrow()                                     // 던지기 완료
    {
        ShowWeapon();
        CancelThrowAnim();
        ItemInHand = EThrowItemName.LAST;
        ChangeState(EPlayerState.IDLE);
    }
}
