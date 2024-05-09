using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ESkillAttribute
{
    ID,                         // ID
    CAST_TYPE,                  // ĳ���� Ÿ��
    PROPERTY,                   // ��ų �Ӽ�
    NAME,                       // �̸�
    ATTACK,                     // ���� ���� ���
    MAGIC,                      // �ּ� ���� ���
    MOVE_SPEED,                 // �̵� �ӵ�?
    CASTING_RANGE,              // ��Ÿ�
    HIT_RADIUS,                 // �����ϴ� ����
    PRE_DELAY,                  // ��������
    TOTAL_DELAY,                // �� ������
    COOLTIME,                   // ��Ÿ��
    STAMINA_COST,               // ���׹̳� ��뷮
    ADJ_TYPE,                   // ���� ���� (���� or �����)
    ADJ_AMOUNT,                 // ��������
    ADJ_TIME,                   // ������ �ð�
    DESCRIPTION,                // ����
    PRICE,                      // ����

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