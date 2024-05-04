using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValueDefine
{
    // 레이어
    public const int HIDING_LAYER = 1 << HIDING_LAYER_IDX;          // 숨기기 레이어
    public const int GROUND_LAYER = 1 << 6;                         // 땅 레이어
    public const int INTERACT_LAYER = 1 << 7;                       // 상호작용 레이어
    public const int HITTABLE_LAYER = 1 << HITTABLE_LAYER_IDX;      // 타격 가능 레이어

    // 레이어 인덱스
    public const int UI_LAYER_IDX = 5;                              // UI 레이어 순서
    public const int HIDING_LAYER_IDX = 3;                          // 숨기기 레이어의 순서
    public const int HITTABLE_LAYER_IDX = 9;

    // 태그
    public const string PLAYER_TAG = "Player";                      // 플레이어
    public const string CAMERA_TAG = "Camera";                      // 카메라
    public const string MONSTER_TAG = "Monster";                    // 몬스터
    public const string ITEM_TAG = "Item";                          // 아이템
    public const string NPC_TAG = "NPC";                            // NPC
    public const string OASIS_TAG = "Oasis";
    public const string PLAYER_WEAPON_TAG = "PlayerWeapon";         // 플레이어 무기
    public const string PLAYER_SKILL_TAG = "PlayerSkill";           // 플레이어 주술
    public const string MONSTER_ATTACK_TAG = "MonsterAttack";       // 몬스터 공격
    public const string TERRAIN_TAG = "Terrain";                    // 터레인
    public const string MONSTER_HIT_TAG = "MonsterHit";             // 몬스터 히트 부위
    public const string PLAYER_HIT_TAG = "PlayerHit";               // 플레이어 히트 부위


    // 값
    public const float PARABOLA_GRAVITY = 20;                       // 포물선 계산 시 중력
    public readonly static Vector3 NullVector = Vector3.up * 100;   // 아무것도 아닌 벡터
    public const int MAX_SKILL_SLOT = 3;                            // 장착 가능 스킬 수
    public const int MAX_INVENTORY = 64;                            // 인벤토리 최대 크기
    public const int MAX_ITEM_VARIETY = 128;                        // 아이템 종류별 개수
    public const int MAX_HEAL_ITEM = 3;                             // 최대 등록 가능 회복 아이템
    public const int MAX_THROW_ITEM = 8;                            // 최대 등록 가능 투척 아이템
    public const int MAX_REGISTER_PATTERN = 5;                      // 최대 각인 가능 문양


    // 쉐이더
    public static Shader URP_LIT_SHADER = Shader.Find("Universal Render Pipeline/Lit");
    public static Shader DISSOLVE_SHADER = Shader.Find("Shader Graphs/DissolveShader");


    // 애니메이션 (?)
    public const string MONSTER_ATTACK_TRIGGER = "Attack";          // 몬스터 공격 트리거
    public const string MONSTER_IDLE_TRIGGER = "Idle";              // 몬스터 기본 트리거
    public const string MONSTER_MOVE_TRIGGER = "Move";              // 몬스터 이동 트리거
    public const string MONSTER_HIT_TRIGGER = "Hurt";              // 몬스터 공격받음 트리거
    public const string MONSTER_DIE_TRIGGER = "Dead";              // 몬스터 사망 트리거

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