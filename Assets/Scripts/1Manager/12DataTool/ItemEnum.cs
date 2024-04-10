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
    GREEN_FORCE,
    GOBLIN_FENG,
    BOMBCRAB_CORE,
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
