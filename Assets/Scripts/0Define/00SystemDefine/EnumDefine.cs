using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStatInfoName
{
    HEALTH,     // 체력
    ENDURE,     // 지구력
    STRENGTH,   // 근력
    INTELLECT,  // 지력
    RAPID,      // 순발력
    MENTAL,     // 정신력

    LAST
}
public enum ECombatInfoName
{
    MAX_HP,         // 최대 HP
    MAX_STAMINA,    // 최대 스테미나
    ATTACK,         // 물리 공격력
    MAGIC,          // 마법 공격력
    DEFENSE,        // 방어력
    OVERDRIVE,    // 치명타 데미지
    TOLERANCE,      // 내성
    LAST
}

public enum EProperty
{
    WATER = 0,
    LIFE = 1,
    CRYSTAL = 2,

    LAST,
}

public enum EInteractType
{
    NPC,
    OASIS,
    ITEM,
    LAST
} 

public enum ECCType
{
    NONE,               // 일반(없음)
    SLOW,               // 둔화
    STUN,               // 기절
    POISON,             // 중독
    BLEED,              // 출혈
    STAGGER,            // 경직
    AIRBORNE,           // 공중
    KNOCKBACK,          // 밀림
    BLIND,

    LAST
}

public enum EQuestType
{
    TALKING,
    COLLECTION,
    HUNTING,

    LAST
}

public enum EQuestStatus
{
    NOT_AVAILABLE,
    AVAILABLE,
    ACCEPTED,
    COMPLETE,
    DONE,

    LAST
}