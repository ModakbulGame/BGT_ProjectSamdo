using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValueDefine
{
    // ���̾�
    public const int HIDING_LAYER = 1 << HIDING_LAYER_IDX;          // ����� ���̾�
    public const int GROUND_LAYER = 1 << 6;                         // �� ���̾�
    public const int INTERACT_LAYER = 1 << 7;                       // ��ȣ�ۿ� ���̾�
    public const int HITTABLE_LAYER = 1 << HITTABLE_LAYER_IDX;      // Ÿ�� ���� ���̾�

    // ���̾� �ε���
    public const int UI_LAYER_IDX = 5;                              // UI ���̾� ����
    public const int HIDING_LAYER_IDX = 3;                          // ����� ���̾��� ����
    public const int HITTABLE_LAYER_IDX = 9;

    // �±�
    public const string PLAYER_TAG = "Player";                      // �÷��̾�
    public const string CAMERA_TAG = "Camera";                      // ī�޶�
    public const string MONSTER_TAG = "Monster";                    // ����
    public const string ITEM_TAG = "Item";                          // ������
    public const string NPC_TAG = "NPC";                            // NPC
    public const string OASIS_TAG = "Oasis";
    public const string PLAYER_WEAPON_TAG = "PlayerWeapon";         // �÷��̾� ����
    public const string PLAYER_SKILL_TAG = "PlayerSkill";           // �÷��̾� �ּ�
    public const string MONSTER_ATTACK_TAG = "MonsterAttack";       // ���� ����
    public const string TERRAIN_TAG = "Terrain";                    // �ͷ���
    public const string MONSTER_HIT_TAG = "MonsterHit";             // ���� ��Ʈ ����
    public const string PLAYER_HIT_TAG = "PlayerHit";               // �÷��̾� ��Ʈ ����


    // ��
    public const float PARABOLA_GRAVITY = 20;                       // ������ ��� �� �߷�
    public readonly static Vector3 NullVector = Vector3.up * 100;   // �ƹ��͵� �ƴ� ����
    public const int MAX_SKILL_SLOT = 3;                            // ���� ���� ��ų ��
    public const int MAX_INVENTORY = 64;                            // �κ��丮 �ִ� ũ��
    public const int MAX_ITEM_VARIETY = 128;                        // ������ ������ ����
    public const int MAX_HEAL_ITEM = 3;                             // �ִ� ��� ���� ȸ�� ������
    public const int MAX_THROW_ITEM = 8;                            // �ִ� ��� ���� ��ô ������
    public const int MAX_REGISTER_PATTERN = 5;                      // �ִ� ���� ���� ����


    // ���̴�
    public static Shader URP_LIT_SHADER = Shader.Find("Universal Render Pipeline/Lit");
    public static Shader DISSOLVE_SHADER = Shader.Find("Shader Graphs/DissolveShader");


    // �ִϸ��̼� (?)
    public const string MONSTER_ATTACK_TRIGGER = "Attack";          // ���� ���� Ʈ����
    public const string MONSTER_IDLE_TRIGGER = "Idle";              // ���� �⺻ Ʈ����
    public const string MONSTER_MOVE_TRIGGER = "Move";              // ���� �̵� Ʈ����
    public const string MONSTER_HIT_TRIGGER = "Hurt";              // ���� ���ݹ��� Ʈ����
    public const string MONSTER_DIE_TRIGGER = "Dead";              // ���� ��� Ʈ����

    // ID
    public const char MONSTER_CODE = 'M';
    public const char WEAPON_CODE = 'W';
    public const char PATTERN_CODE = 'P';
    public const char THROW_ITEM_CODE = 'T';
    public const char OTHER_ITEM_CODE = 'O';

    public const char BLADE_CODE = '1';
    public const char SWORD_CODE = '2';
    public const char SCEPTER_CODE = '3';

    public const char MELEE_CODE = '1';
    public const char MELEE_CC_CODE = '2';
    public const char RANGED_CODE = '3';
    public const char RANGED_CC_CODE = '4';
    public const char SUMMON_CODE = '5';
    public const char AROUND_CODE = '6';
    public const char AROUND_CC_CODE = '7';

    public const char SLOW_CODE = '1';
    public const char STUN_CODE = '2';
    public const char POISON_CODE = '3';
    public const char BLEED_CODE = '4';
    public const char STAGGER_CODE = '5';
    public const char AIRBORNE_CODE = '6';
    public const char KNOCKBACK_CODE = '7';
}