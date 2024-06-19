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

    // ���̾� �ܾ�
    public const string HITTABLE_LAYER_NAME = "Hittable";

    // �±�
    public const string PLAYER_TAG = "Player";                      // �÷��̾�
    public const string CAMERA_TAG = "Camera";                      // ī�޶�
    public const string MONSTER_TAG = "Monster";                    // ����
    public const string NPC_TAG = "NPC";                            // NPC
    public const string OASIS_TAG = "Oasis";
    public const string PLAYER_WEAPON_TAG = "PlayerWeapon";         // �÷��̾� ����
    public const string PLAYER_POWER_TAG = "PlayerPower";           // �÷��̾� �ּ�
    public const string MONSTER_ATTACK_TAG = "MonsterAttack";       // ���� ����
    public const string WATER_TAG = "Water";                        // ��
    public const string TERRAIN_TAG = "Terrain";                    // ����


    // �� �ε���
    public const int LOGO_SCENE_IDX = 0;
    public const int TITLE_SCENE_IDX = 1;
    public const int LOADING_SCENE_IDX = 2;
    public const int HELL_SCENE_IDX = 3;


    // ��
    public const int INITIAL_STAT = 10;
    public const int MAX_SAVE = 12;
    public const float PARABOLA_GRAVITY = 20;                       // ������ ��� �� �߷�
    public readonly static Vector3 NullVector = Vector3.up * 100;   // �ƹ��͵� �ƴ� ����
    public const int MAX_POWER_SLOT = 3;                            // ���� ���� ��ų ��
    public const int MAX_INVENTORY = 64;                            // �κ��丮 �ִ� ũ��
    public const int MAX_ITEM_VARIETY = 128;                        // ������ ������ ����
    public const int MAX_HEAL_ITEM = 3;                             // �ִ� ��� ���� ȸ�� ������
    public const int MAX_THROW_ITEM = 8;                            // �ִ� ��� ���� ��ô ������
    public const int MAX_REGISTER_PATTERN = 5;                      // �ִ� ���� ���� ����
    public const int MAX_QUEST_NUM = 4;                             // �ִ� ���� ���� ����Ʈ
    public const int DIE_DAMAGE = 999;
    public const int MAX_NPC_NUM = 99;                              // ������ NPC ��


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

    public const char FATIGUE_CODE = '1';
    public const char STUN_CODE = '2';
    public const char MELANCHOLY_CODE = '3';
    public const char EXTORTION_CODE = '4';
    public const char AIRBORNE_CODE = '5';
    public const char KNOCKBACK_CODE = '6';
    public const char WEAKNESS_CODE = '7';
    public const char BIND_CODE = '8';
    public const char VOID_CODE = '9';
}