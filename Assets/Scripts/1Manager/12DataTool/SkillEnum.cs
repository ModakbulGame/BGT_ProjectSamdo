using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ESkillAttribute
{
    ID,                         // ID
    CAST_TYPE,                  // 캐스팅 타입
    PROPERTY,                   // 스킬 속성
    NAME,                       // 이름
    ATTACK,                     // 물리 공격 계수
    MAGIC,                      // 주술 공격 계수
    MOVE_SPEED,                 // 이동 속도?
    CASTING_RANGE,              // 사거리
    HIT_RADIUS,                 // 공격하는 범위
    PRE_DELAY,                  // 선딜레이
    TOTAL_DELAY,                // 총 딜레이
    COOLTIME,                   // 쿨타임
    STAMINA_COST,               // 스테미나 사용량
    ADJ_TYPE,                   // 스탯 변동 (버프 or 디버프)
    ADJ_AMOUNT,                 // ㄴ변동량
    ADJ_TIME,                   // ㄴ지속 시간
    DESCRIPTION,                // 설명
    PRICE,                      // 가격

    LAST
}

public enum ECastType
{
    MELEE,
    MELEE_CC,
    RANGED,
    RANGED_CC,
    SUMMON,
    AROUND,
    AROUND_CC,
    BUFF,

    LAST
}

public enum ESkillProperty
{
    SLASH,
    HIT,
    EXPLOSION,
    SHOCKWAVE,
    FOG,
    TOTEM,
    LIGHT,
    SOUL,

    LAST
}

public enum ESkillName
{
    BLADE_BASIC,
    SWORD_BASIC,
    MELEE_SLOW,
    MELEE_STUN,
    MELEE_POISON,
    MELEE_BLEED,
    MELEE_STAGGER,
    MELEE_AIRBORNE,
    MELEE_KNOCKBACK,
    SCEPTER_BASIC,
    RANGED_SLOW,
    RANGED_STUN,
    RANGED_POISON,
    RANGED_BLEED,
    RANGED_STAGGER,
    RANGED_AIRBORNE,
    RANGED_KNOCKBACk,
    CREATION,
    AROUND,
    AROUND_SLOW,
    AROUND_STUN,
    AROUND_POISON,
    AROUND_BLEED,
    AROUND_CONFUSE,
    AROUND_AIRBORNE,
    AROUND_KNOCKBACK,
    BUFF,
    SAMPLE1,
    SAMPLE2,
    SAMPLE3,
    LAST
}