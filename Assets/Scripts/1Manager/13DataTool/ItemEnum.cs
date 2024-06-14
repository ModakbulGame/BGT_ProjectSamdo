using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemAttribute
{
    ID,                     // ID
    TYPE,                   // Ÿ��
    NAME,                   // �̸�
    DESCRIPTION,            // ����
    DROP_RATE,              // �����
    PRICE,                  // ����
    MIN_ATTACK,             // �ּ� ����
    MAX_ATTACK,             // �ִ� ����
    MIN_MAGIC,              // �ּ� �ּ�
    MAX_MAGIC,              // �ִ� �ּ�
    ATTACK_SPEED,           // ���� �ӵ�
    HEAL_AMOUNT,            // ȸ����
    BUFF_TIME,              // ���� �ð�
    THROW_DAMAGE,           // ��ô ������ ������
    THROW_SPEED,            // ��ô �ӵ�
    EXPLODE_TIME,           // ���� ������
    LAST
}

public enum EItemType
{
    WEAPON,                 // ����
    PATTERN,                // ����
    THROW,                  // ��ô
    OTHERS,                 // ��Ÿ
    LAST
}

public enum EWeaponType
{
    BLADE,                  // ���
    SWORD,                  // ����
    SCEPTER,                // Ȧ

    LAST,
}


public enum EWeaponName
{
    BASIC_BLADE,            // �⺻ ���
    BASIC_SWORD,            // �⺻ ����
    BASIC_SCEPTER,          // �⺻ Ȧ
    GOBLIN_SCEPTER,         // ��� Ȧ

    LAST
}

public enum EPatternName
{
    LIFE,
    LIFE_UP,
    WATER,
    WATER_UP,
    GROUND,
    GROUND_UP,
    LAST
}

public enum EThrowItemName
{
    STONE,
    BOMB,
    SLOW,
    ENDER,
    PURIFY,
    LAST
}

public enum EOtherItemName
{
    O101,
    O102,
    O103,
    O104,
    O201,
    O202,
    O203,
    O204,
    O301,
    O302,
    O303,
    O304,
    O305,
    O501,
    O502,
    O503,
    O504,
    O505,
    O506,
    O507,
    O508,
    O509,
    O510,
    O511,
    O512,
    O513,
    O514,
    O515,
    O516,
    O517,
    O518,
    O519,
    O520,

    LAST
}

public enum EItemName       // ���� ���� ������ �̸�
{
    ITEM1,
    ITEM2,
    ITEM3,
    ITEM4,

    LAST
}

public enum EDropAttribute
{
    MONSTER,
    ITEM1,
    RATE1,
    ITEM2,
    RATE2,
    ITEM3,
    RATE3,
    ITEM4,
    RATE4,
    STAT,
    SOUL,
    PURIFIED,

    LAST
}