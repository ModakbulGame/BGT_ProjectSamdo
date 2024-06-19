using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterAttribue
{
    ID,                         // ID
    TYPE,                       // Ÿ��
    NAME,                       // �̸�
    MAX_HP,                     // �ִ� ü��
    DAMAGE,                     // ������
    ROAMING_SPEED,              // �ι� �ӵ�
    APPROACH_SPEED,             // ���� �ӵ�
    VIEW_ANGLE,                 // �þ߰�
    VIEW_RANGE,                 // �þ� ����
    ENGAGE_RANGE,               // ���� ����
    RETURN_RANGE,               // ���� ����
    ATTACK_RANGE,               // ���� ����
    ATTACK_SPEED,               // ���� �ӵ�
    APPROACH_DELAY,             // ���� ������
    FENCE_RANGE,                // �Ÿ� ���� ����
    DESCRIPTION,                // ����
    LAST
}

public enum EMonsterType
{
    NORMAL,
    ELITE,
    BOSS,
    LAST
}

public enum EMonsterName            // ���� �̸�
{
    STARVED_HHM,        // ���ָ� ��
    WOLF,               // ����
    BLOKAN,             // ���ĭ

    MISERABLE_HHM,      // ������ ��
    FROMETZ,            // ���θ���
    MARMULAK,           // �������� �ļ���

    ARROGANT_HHM,       // �Ÿ��� ��
    BLINK_BEAK,         // �����θ�
    UUM,                // ��

    BOSS,               // ���� ����

    SKURRABY_LIFE,      // ���� �ذ����
    SKURRABY_WATER,     // �� �ذ����
    SKURRABY_CRYSTAL,   // ���� �ذ����
    QUEEN_LIFE,         // ���� �ذ���� ����
    QUEEN_WATER,        // �� �ذ���� ����
    QUEEN_CRYSTAL,      // ���� �ذ���� ����
    HELLCAT_LIFE,       // ���� �������
    HELLCAT_WATER,      // �� �������
    HELLCAT_CRYSTAL,    // ���� �������
    SPIRIT1_LIFE,       // ���� ����1
    SPIRIT1_WATER,      // �� ����1
    SPIRIT1_CRYSTAL,    // ���� ����1
    SPIRIT2_LIFE,       // ���� ����2
    SPIRIT2_WATER,      // �� ����2
    SPIRIT2_CRYSTAL,    // ���� ����2
    MINIUUM_LIFE,       // ���� �̴Ͽ�
    MINIUUM_WATER,      // �� �̴Ͽ�
    MINIUUM_CRYSTAL,    // ���� �̴Ͽ�
    BLO,                // ���
    LIFE_GUARDIAN,      // ���� ������
    WATER_GUARDIAN,     // �� ������
    CRYSTAL_GUARDIAN,   // ���� ������

    LAST
}
