using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStatName
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
    FATIGUE,            // �Ƿ�
    STUN,               // ����
    MELANCHOLY,         // ���
    EXTORTION,          // ����
    AIRBORNE,           // ���
    KNOCKBACK,          // �и�
    WEAKNESS,           // ����
    BIND,               // �ӹ�
    VOID,               // ���
    OBLIVION,           // ����
    BLIND,              // �Ǹ�

    LAST
}
