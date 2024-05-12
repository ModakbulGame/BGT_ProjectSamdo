using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStatInfoName
{
    HEALTH,     // ü��
    ENDURE,     // ������
    STRENGTH,   // �ٷ�
    INTELLECT,  // ����
    RAPID,      // ���߷�
    MENTAL,     // ���ŷ�

    LAST
}
public enum ECombatInfoName
{
    MAX_HP,         // �ִ� HP
    MAX_STAMINA,    // �ִ� ���׹̳�
    ATTACK,         // ���� ���ݷ�
    MAGIC,          // ���� ���ݷ�
    DEFENSE,        // ����
    OVERDRIVE,    // ġ��Ÿ ������
    TOLERANCE,      // ����
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
    NONE,               // �Ϲ�(����)
    SLOW,               // ��ȭ
    STUN,               // ����
    POISON,             // �ߵ�
    BLEED,              // ����
    STAGGER,            // ����
    AIRBORNE,           // ����
    KNOCKBACK,          // �и�
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