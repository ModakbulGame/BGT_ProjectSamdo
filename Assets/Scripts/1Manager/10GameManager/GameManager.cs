using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : SingleTon<GameManager>
{
    // 데이터
    private DataManager m_dataManager;
    public static DataManager DataManager { get { return Inst.m_dataManager; } }
    public static MonsterScriptable GetMonsterRawData(EMonsterName _monster) { return DataManager.GetMonsterData(_monster); }   // 몬스터 스크립터블
    public static ItemScriptable GetItemRawData(SItem _item) { return DataManager.GetItemData(_item); }                         // 아이템 스크립터블
    public static SkillScriptable GetSkillRawData(ESkillName _skill) { return DataManager.GetSkillData(_skill); }               // 스킬 스크립터블


    // 화면
    private DisplayManager m_displayManager;
    public static DisplayManager DisplayManager { get { return Inst.m_displayManager; } }
    public static float WidthRatio { get { return DisplayManager.WidthRatio; } }                                                // 화면 너비 비율
    public static float HeightRatio { get { return DisplayManager.HeightRatio; } }                                              // 화면 높이 비율


    // 소리
    private SoundManager m_soundManager;
    public static SoundManager SoundManager { get { return Inst.m_soundManager; } }


    // 입력
    private InputManager m_inputManager;
    public static InputManager InputManager { get { return Inst.m_inputManager; } }
    public static InputSystem.PlayerActions PlayerInputs { get { return InputManager.PlayerInputs; } }                          // 플레이어 Input
    public static InputSystem.UIControlActions UIControlInputs { get { return InputManager.UIControlInputs; } }                 // UI조작 Input
    public static EControlMode ControlMode { get { return InputManager.CurControlMode; } }                                      // 조작 모드
    public static float MouseSensitive { get { return InputManager.MouseSensitive; } }                                          // 마우스 민감도
    public static void SetControlMode(EControlMode _mode) { InputManager.SetControlMode(_mode); }                               // 조작 모드 변경
    public static void SetMouseSensitive(float _sensitive) { InputManager.SetMouseSensitive(_sensitive); }                      // 마우스 민감도 설정

    // 스킬
    private SkillManager m_skillManager;
    private static SkillManager SkillManager { get { return Inst.m_skillManager; } }
    private static GameObject[] SkillArray { get { return SkillManager.SkillArrays; } }
    public static SkillInfo GetSkillInfo(ESkillName _skill) { return SkillManager.GetSkillInfo(_skill); }                                   // 스킬 정보
    public static GameObject GetSkillObj(ESkillName _skill) { return SkillManager.GetSkillObj(_skill); }                                    // 스킬 프리펍
    public static ESkillName[] SkillSlot { get { return SkillManager.SkillSlot; } }                                                         // 스킬 슬롯
    public static void RegisterSkilSlot(ESkillName _skill, int _idx) { SkillManager.RegisterSkillSlot(_skill, _idx); PlayManager.UpdateSkillSlot(); }   // 스킬 슬롯 설정
    public static void ObtainSkill(ESkillName _skill) { SkillManager.ObtainSkill(_skill); }

    // 아이템
    public static EItemName[] ItemSlot { get { return null; } }                                                         // 아이템 슬롯
    public static void RegisterItemSlot(EItemName _item, int _idx) { }   // 아이템 슬롯 설정


    // 몬스터
    private MonsterManager m_monsterManager;
    private static MonsterManager MonsterManager { get { return Inst.m_monsterManager; } }
    private static GameObject[] MonsterArray { get { return MonsterManager.MonsterArray; } }
    public static MonsterInfo GetMonsterInfo(EMonsterName _monster) { return MonsterManager.GetMonsterInfo(_monster); }                     // 몬스터 정보
    public static GameObject GetMonsterObj(EMonsterName _monster) { return MonsterManager.GetMonsterObj(_monster); }                        // 몬스터 프리펍
    public static bool CheckNClearMonster(EMonsterName _monster) { return MonsterManager.CheckNClearMonster(_monster); }                    // 최초 처치 확인



    // 이펙트
    private EffectManager m_effectManager;
    private static EffectManager EffectManager { get { return Inst.m_effectManager; } }
    private static GameObject[] EffectArray { get { return EffectManager.EffectArray; } }
    public static GameObject GetEffectObj(EEffectName _effect) { return EffectManager.GetEffectObj(_effect); }


    // UI
    private UIManager m_uiManager;
    public static UIManager UIManager { get { return Inst.m_uiManager; } }
    public static RectTransform GetCanvasTransform(ECanvasType _canvas) { return UIManager.GetCanvasTransform(_canvas); }       // 캔버스 트랜스폼
    public static void CreateSideTextAlarm(string _alarm) { UIManager.CreateSideTextAlarm(_alarm); }                            // 화면 사이드 텍스트 알람 생성
    public static Sprite GetMonsterSprite(EMonsterName _monster) { return UIManager.GetMonsterSprite(_monster); }               // 몬스터 이미지
    public static Sprite GetItemSprite(SItem _item) { return UIManager.GetItemSprite(_item); }                                  // 아이템 이미지
    public static Sprite GetSkillSprite(ESkillName _skill) { return UIManager.GetSkillSprite(_skill); }                         // 스킬 이미지


    private PoolManager m_poolManager;
    private static PoolManager PoolManager { get { return Inst.m_poolManager; } }



    private void SetSubManagers()
    {
        m_dataManager = GetComponent<DataManager>();
        m_dataManager.SetManager();
        m_displayManager = GetComponent<DisplayManager>();
        m_soundManager = GetComponent<SoundManager>();
        m_inputManager = GetComponent<InputManager>();
        m_inputManager.SetSubManager();
        m_skillManager = GetComponent<SkillManager>();
        m_skillManager.SetManager();
        m_monsterManager = GetComponent<MonsterManager>();
        m_monsterManager.SetManager();
        m_effectManager = GetComponent<EffectManager>();
        m_effectManager.SetManager();
        m_uiManager = GetComponent<UIManager>();
        m_poolManager = GetComponent<PoolManager>();
        m_poolManager.SetManager(SkillArray, MonsterArray, EffectArray);
    }

    public override void Awake()
    {
        base.Awake();
        SetSubManagers();
    }
}
