using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public static bool IsInTitle { get { return SceneManager.GetActiveScene().buildIndex == ValueDefine.TITLE_SCENE_IDX; } }
    public static bool IsInGame { get { return SceneManager.GetActiveScene().buildIndex > ValueDefine.TITLE_SCENE_IDX; } }
    public static void StartGame()
    {
        MoveToPlayScene(null);
    }
    public static void LoadGame(int _idx)
    {
        SaveData data = GameData[_idx];
        MoveToPlayScene(data);
    }
    private static void MoveToPlayScene(SaveData _data)
    {
        PlayManager.SetCurData(_data);
        SceneManager.LoadScene(ValueDefine.HELL_SCENE_IDX);
    }


    // ������
    private DataManager m_dataManager;
    public static DataManager DataManager { get { return Inst.m_dataManager; } }
    public static List<SaveData> GameData { get { return DataManager.GameData; } }
    public static void SaveGameData(EOasisPointName _oasis) { DataManager.SaveCurData(_oasis); }
    public static void AddGameData(SaveData _data) { DataManager.AddGameData(_data); }
    public static void RegisterData(IHaveData _data) { DataManager.RegisterData(_data); }


    // ȭ��
    private DisplayManager m_displayManager;
    public static DisplayManager DisplayManager { get { return Inst.m_displayManager; } }
    public static float WidthRatio { get { return DisplayManager.WidthRatio; } }                                                // ȭ�� �ʺ� ����
    public static float HeightRatio { get { return DisplayManager.HeightRatio; } }                                              // ȭ�� ���� ����


    // �Ҹ�
    private SoundManager m_soundManager;
    public static SoundManager SoundManager { get { return Inst.m_soundManager; } }


    // �Է�
    private InputManager m_inputManager;
    public static InputManager InputManager { get { return Inst.m_inputManager; } }
    public static InputSystem.PlayerActions PlayerInputs { get { return InputManager.PlayerInputs; } }                          // �÷��̾� Input
    public static InputSystem.UIControlActions UIControlInputs { get { return InputManager.UIControlInputs; } }                 // UI���� Input
    public static EControlMode ControlMode { get { return InputManager.CurControlMode; } }                                      // ���� ���
    public static float MouseSensitive { get { return InputManager.MouseSensitive; } }                                          // ���콺 �ΰ���
    public static void SetControlMode(EControlMode _mode) { InputManager.SetControlMode(_mode); }                               // ���� ��� ����
    public static void SetMouseSensitive(float _sensitive) { InputManager.SetMouseSensitive(_sensitive); }                      // ���콺 �ΰ��� ����


    // ������
    private ItemManager m_itemManager;
    private static ItemManager ItemManager { get { return Inst.m_itemManager; } }
    private static GameObject[] ItemArray { get { return ItemManager.ItemArray; } }
    public static ItemInfo GetItemInfo(SItem _item) { return ItemManager.GetItemInfo(_item); }                                              // ������ ����
    public static ItemInfo GetItemInfo(string _id) { return ItemManager.GetItemInfo(_id); }                                                 // ������ ����
    public static ItemScriptable GetItemData(SItem _item) { return ItemManager.GetItemData(_item); }                                        // ������ ��ũ���ͺ�
    public static ItemInfo GetWeaponInfo(EWeaponName _weapon) { return GetItemInfo(new SItem(EItemType.WEAPON, (int)_weapon)); }            // ���� ����
    public static GameObject GetThorwItemPrefab(EThrowItemName _item) { return ItemManager.GetThrowItemPrefab(_item); }                     // ��ô ������ ������
    public static GameObject GetDropItemPrefab(EItemType _item) { return ItemManager.GetDropItemPrefab(_item); }                            // ��� ������ ������



    // ��ų
    private SkillManager m_skillManager;
    private static SkillManager SkillManager { get { return Inst.m_skillManager; } }
    private static GameObject[] SkillArray { get { return SkillManager.SkillArrays; } }
    public static SkillInfo GetSkillInfo(ESkillName _skill) { return SkillManager.GetSkillInfo(_skill); }                                   // ��ų ����
    public static SkillScriptable GetSkillData(ESkillName _skill) { return SkillManager.GetSkillData(_skill); }                             // ��ų ��ũ���ͺ�
    public static GameObject GetSkillObj(ESkillName _skill) { return SkillManager.GetSkillObj(_skill); }                                    // ��ų ������
    public static ESkillName[] SkillSlot { get { return SkillManager.SkillSlot; } }                                                         // ��ų ����
    public static void RegisterSkilSlot(ESkillName _skill, int _idx) { SkillManager.RegisterSkillSlot(_skill, _idx); PlayManager.UpdateSkillSlot(); }   // ��ų ���� ����
    public static void ObtainSkill(ESkillName _skill) { SkillManager.ObtainSkill(_skill); }


    // ����
    private MonsterManager m_monsterManager;
    private static MonsterManager MonsterManager { get { return Inst.m_monsterManager; } }
    public static GameObject[] MonsterArray { get { return MonsterManager.MonsterArray; } }
    public static MonsterInfo GetMonsterInfo(EMonsterName _monster) { return MonsterManager.GetMonsterInfo(_monster); }                     // ���� ����
    public static MonsterScriptable[] MonsterData { get { return MonsterManager.MonsterData; } }
    public static MonsterScriptable GetMonsterData(EMonsterName _monster) { return MonsterManager.GetMonsterData(_monster); }               // ���� ��ũ���ͺ�
    public static GameObject GetMonsterObj(EMonsterName _monster) { return MonsterManager.GetMonsterObj(_monster); }                        // ���� ������
    public static bool CheckNClearMonster(EMonsterName _monster) { return MonsterManager.CheckNClearMonster(_monster); }                    // ���� óġ Ȯ��



    // ����Ʈ
    private EffectManager m_effectManager;
    private static EffectManager EffectManager { get { return Inst.m_effectManager; } }
    private static GameObject[] EffectArray { get { return EffectManager.EffectArray; } }
    public static GameObject GetEffectObj(EEffectName _effect) { return EffectManager.GetEffectObj(_effect); }


    // UI
    private UIManager m_uiManager;
    public static UIManager UIManager { get { return Inst.m_uiManager; } }
    public static RectTransform GetCanvasTransform(ECanvasType _canvas) { return UIManager.GetCanvasTransform(_canvas); }       // ĵ���� Ʈ������
    public static void CreateSideTextAlarm(string _alarm) { UIManager.CreateSideTextAlarm(_alarm); }                            // ȭ�� ���̵� �ؽ�Ʈ �˶� ����
    public static Sprite GetMonsterSprite(EMonsterName _monster) { return UIManager.GetMonsterSprite(_monster); }               // ���� �̹���
    public static Sprite GetItemSprite(SItem _item) { return UIManager.GetItemSprite(_item); }                                  // ������ �̹���
    public static Sprite GetSkillSprite(ESkillName _skill) { return UIManager.GetSkillSprite(_skill); }                         // ��ų �̹���


    private PoolManager m_poolManager;
    private static PoolManager PoolManager { get { return Inst.m_poolManager; } }



    private void SetSubManagers()
    {
        m_dataManager = GetComponent<DataManager>();
        m_dataManager.SetManager();
        m_displayManager = GetComponent<DisplayManager>();
        m_soundManager = GetComponent<SoundManager>();
        m_inputManager = GetComponent<InputManager>();
        m_inputManager.SetManager();
        m_itemManager = GetComponent<ItemManager>();
        m_itemManager.SetManager();
        m_skillManager = GetComponent<SkillManager>();
        m_skillManager.SetManager();
        m_monsterManager = GetComponent<MonsterManager>();
        m_monsterManager.SetManager();
        m_effectManager = GetComponent<EffectManager>();
        m_effectManager.SetManager();
        m_uiManager = GetComponent<UIManager>();
        m_poolManager = GetComponent<PoolManager>();
        m_poolManager.SetManager(ItemArray, SkillArray, MonsterArray, EffectArray);
    }

    public override void Awake()
    {
        base.Awake();
        SetSubManagers();
    }
}
