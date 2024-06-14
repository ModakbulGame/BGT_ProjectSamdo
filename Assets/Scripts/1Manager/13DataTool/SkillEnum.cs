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
    MELEE_SLASH,
    MELEE_SOUL,
    MELEE_STUN,
    MELEE_MELANCHOLY,
    MELEE_EXTORTION,
    MELEE_AIRBORNE,
    RANGED_PROJ1,
    RANGED_PROJ2,
    RANGED_PROJ3,
    RANGED_PARAB1,
    RANGED_PARAB2,
    RANGED_PARAB3,
    RANGED_PARAB4,
    RANGED_PARAB5,
    RANGED_SLASH1,
    RANGED_SLASH2,
    RANGED_FATIGUE1,
    RANGED_FATIGUE2,
    RANGED_MELANCHOLY,
    RANGED_KNOCKBACK1,
    RANGED_KNOCKBACK2,
    CREATION_SOUL,
    CREATION_TOTEM1,
    CREATION_TOTEM2,
    CREATION_TOTEM3,
    CREATION_TOTEM4,
    CREATION_TOTEM5,
    AROUND_SHOCKWAVE,
    AROUND_SLASH,
    AROUND_FOG,
    AROUND_FATIGUE,
    AROUND_AIRBORNE,
    AROUND_BIND,
    AROUND_VOID,
    BUFF_MAXHP,
    BUFF_COMBAT,
    BUFF_FATIGUE,
    BUFF_STUN,
    BUFF_MELANCHOLY,
    BUFF_EXTORTION,

    LAST
}