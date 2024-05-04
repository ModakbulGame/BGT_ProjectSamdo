using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayManager : MonoBehaviour
{
    public static PlayManager Inst;


    // ���� �Լ�
    public static bool IsPlaying { get { return Inst != null && Inst.gameObject != null; } }                // �÷��� ������


    // �÷��̾�
    private static PlayerController Player { get; set; }
    public static void SetCurPlayer(PlayerController _player) { Player = _player; }                                                         // �÷��̾� ���
    public static bool CheckIsPlayer(ObjectScript _object) { return _object == Player; }                                                    // �÷��̾����� Ȯ��
    public static bool IsPlayerSet { get { return Player != null; } }                                                                       // �÷��̾� ��� ����
    public static bool IsPlayerGuarding { get { return Player.IsGuarding; } }                                                               // �÷��̾� ���� ��
    public static bool IsPlayerDead { get { return Player.IsDead; } }                                                                       // �÷��̾� ��� ����
    public static bool IsPlayerLightOn { get { return Player.IsLightOn; } }                                                                 // �÷��̾� �ɷ� �����
    public static Vector3 PlayerPos { get { if (IsPlayerSet) return Player.transform.position; return ValueDefine.NullVector; } }           // �÷��̾� ��ġ
    public static Vector2 PlayerPos2 { get { if (IsPlayerSet) return Player.Position2; return ValueDefine.NullVector; } }                   // �÷��̾� ��� ��ġ

    public static Vector2 PlayerAimDirection { get { return Player.PlayerAimDirection; } }                                                  // ī�޶� ���� ����
    public static Transform PlayerTransform { get { if (IsPlayerSet) return Player.transform; return null; } }
    public static PlayerStatInfo PlayerStatInfo { get { return Player.GetStatInfo(); } }
    public static SPlayerWeaponInfo PlayerWeaponInfo { get { return Player.CurWeaponInfo; } }
    public static float GetDistToPlayer(Vector3 _pos) { if (!IsPlayerSet) return -1; return (PlayerPos-_pos).magnitude; }                   // �÷��̾���� �Ÿ�
    public static void SetPlayerWeapon(EWeaponName _weapon) { Player.SetCurWeapon(_weapon); }                                               // ���� ����
    public static void StopPlayerInteract() { Player.StopInteract(); }                                                                      // ��ȣ�ۿ� ����
    public static void TeleportPlayer(Vector3 _pos) { Player.TeleportPlayer(_pos); }
    
    public static void TempGetBuff(float _amount, float _time) { Player.GetStatAdjust(new(EAdjType.MAX_HP, _amount, _time)); }
    

    // ī�޶�
    [SerializeField]
    private CameraManager m_cameraManager;
    private static CameraManager CameraManager { get { return Inst.m_cameraManager; } }
    public static CinemachineFreeLook PlayerFreeLook { get { return CameraManager.PlayerFreeLook; } }
    public static float CameraRotation { get { return CameraManager.CameraRotation; } }                                                     // ī�޶� �¿� ����
    public static float CameraAngle { get { return CameraManager.CameraAngle; } }                                                           // ī�޶� ���Ʒ� ����
    public static void SetCameraMode(EControlMode _mode) { CameraManager.SetCameraMode(_mode); }                                            // ���� ��� ����
    public static void SetCameraSensitive(float _sensitive) { CameraManager.SetCameraSensitive(_sensitive); }                               // ���콺 �ΰ��� ����
    public static void CameraSwitch(CinemachineFreeLook targetCamera) { CameraManager.SwitchToCamera(targetCamera); }                       // ī�޶� ��ȯ


    // ������
    private ItemManager m_itemManager;
    private static ItemManager ItemManager { get { return Inst.m_itemManager; } }
    public static ItemInfo GetItemInfo(SItem _item) { return ItemManager.GetItemInfo(_item); }                                              // ������ ����
    public static ItemInfo GetItemInfo(string _id) { return ItemManager.GetItemInfo(_id); }                                                 // ������ ����
    public static ItemInfo GetWeaponInfo(EWeaponName _weapon) { return GetItemInfo(new SItem(EItemType.WEAPON, (int)_weapon)); }            // ���� ����
    public static GameObject GetWeaponPrefab(EWeaponName _weapon) { return ItemManager.GetWeaponPrefab(_weapon); }                          // ���� ������
    public static GameObject GetThorwItemPrefab(EThrowItemName _item) { return ItemManager.GetThrowItemPrefab(_item); }                     // ��ô ������ ������
    public static GameObject GetDropItemPrefab(EItemType _item) { return ItemManager.GetDropItemPrefab(_item); }                            // ��� ������ ������
    public static InventoryElm[] PlayerInventory { get { return ItemManager.Inventory; } }                                                  // �κ��丮 ������ ���
    public static void AddInventoryItem(SItem _item, int _num) { ItemManager.AddInventoryItem(_item, _num); }                               // �� �κ��丮�� ������ �߰�
    public static void SetInventoryItem(int _idx, SItem _item, int _num) { ItemManager.SetInventoryItem(_idx, _item, _num); }               // �κ��丮 �ش� Idx�� ������ ����
    public static void RemoveInventoryItem(int _idx) { ItemManager.RemoveInventoryItem(_idx); }                                             // �κ��丮 �ش� Idx ������ ����
        // ��� ����
    public static EWeaponName CurWeapon { get { return ItemManager.CurWeapon; } }                                                           // ���� ���� ����
    public static void SetCurWeapon(EWeaponName _weapon) { ItemManager.SetCurWeapon(_weapon); }                                             // ���� ����
    public static void EquipWeapon(EWeaponName _weapon) { ItemManager.EquipWeapon(_weapon); }                                               // ���� ����
    public static void ObtainWeapon(EWeaponName _weapon) { ItemManager.ObtainWeapon(_weapon); }                                             // ���� ȹ��
        // ��� ������ ����
    public static EPatternName CurHealPattern { get { return ItemManager.CurHealPattern; } }
    public static EPatternName[] HealPatternList { get { return ItemManager.HealPatternList; } }
    public static void UseHealPattern() { ItemManager.UseHealItem(); UpdateInfoUI(); UpdateHealItemSlot(); }
    public static void RegisterHealPattern(EPatternName _pattern) { ItemManager.RegisterHealItem(_pattern); UpdateInfoUI(); UpdateHealItemSlot(); }
    public static EThrowItemName CurThrowItem { get { return ItemManager.CurThrowItem; } }                                                  // ���� ������ ������ (LAST == null)
    public static List<EThrowItemName> ThrowItemList { get { return ItemManager.ThrowItemList; } }                                          // ��ϵ� ������ ������
    public static void UseThrowItem() { ItemManager.UseThrowItem(); UpdateInfoUI(); UpdateThrowItemSlot(); }                                // ������ ������ ���
    public static void AddThrowItem(EThrowItemName _item) { ItemManager.AddThrowItem(_item); UpdateInfoUI(); UpdateThrowItemSlot(); }       // ������ ������ �߰�
        // ��ȭ ����
    public static int SoulNum { get { return ItemManager.SoulNum; } }                                                                       // ��ȥ ����
    public static int PurifiedNum { get { return ItemManager.PurifiedNum; } }                                                               // ���� ��ȥ ����
    public static int[] PatternNum { get { return ItemManager.PatternNum; } }                                                               // ���纰 ����
    public static void AddSoul(int _num) { ItemManager.AddSoul(_num); }                                                                     // ��ȥ �߰�
    public static void AddPurified(int _num) { ItemManager.AddPurified(_num); }                                                             // ���� ��ȥ �߰�
    public static void AddPattern(EProperty _type, int _num) { ItemManager.AddPattern(_type, _num); }                                       // ���� �߰�
    public static void UseSoul(int _num) { ItemManager.UseSoul(_num); }                                                                     // ��ȥ ���
    public static void UsePurified(int _num) { ItemManager.UsePurified(_num); }                                                             // ���� ��ȥ ���
    public static void UsePattern(EProperty _type, int _num) { ItemManager.UsePattern(_type, _num); }                                       // ���� ���

    // ���丮
    private StoryManager m_storyManager;
    private static StoryManager StoryManager { get { return Inst.m_storyManager; } }


    // ȯ��
    private EnvironmentManager m_environmentManager;
    private static EnvironmentManager EnvironmentManager { get { return Inst.m_environmentManager; } }
    public static Vector3 MapLB { get { return EnvironmentManager.m_mapPositioner[0].position; } }
    public static Vector3 MapRT { get { return EnvironmentManager.m_mapPositioner[1].position; } }
    public static float MapWidth { get { return EnvironmentManager.MapWidth; } }
    public static float MapHeight { get { return EnvironmentManager.MapHeight; } }

    public static Transform[] NormalizeObjects { get { return null; } }
    public static GameObject[] MapOasis { get { return null; } }


    // ���׷��̵�
    private UpgradeManager m_upgradeManager;
    private static UpgradeManager UpgradeManager { get { return Inst.m_upgradeManager; } }
    public static int LeftStatPoint { get { return UpgradeManager.LeftStatPoint; } }
    public static void AddStatPoint(int _add) { UpgradeManager.AddStatPoint(_add); }
    public static void UpgradeStat(int[] _point) { UpgradeManager.UpgradeStat(_point); }



    // GUI
    private PlayUIManager m_playUIManager;
    private static PlayUIManager PlayUIManager { get { return Inst.m_playUIManager; } }
    public static Vector2 NormalizeLocation(Transform _obj) { return PlayUIManager.NormalizeLocation(_obj); }                              // ��ġ ����ȭ(3D -> 2D)
    public static Canvas GetCanvas(ECanvasType _canvas) { return PlayUIManager.GetCanvas(_canvas); }                                        // ĵ����
    public static RectTransform CanvasTrans(ECanvasType _canvas) { return GetCanvas(_canvas).GetComponent<RectTransform>(); }               
    public static void OpenPlayerUI() { PlayUIManager.OpenPlayerUI(); }                                                                     // Player UI ����
    public static void ClosePlayerUI() { PlayUIManager.ClosePlayerUI(); }                                                                   // Player UI �ݱ�
    public static void SetMinimapScale(float _scale) { PlayUIManager.SetMinimapScale(_scale); }                                             // �̴ϸ� ��ô
    public static void ToggleMapUI() { PlayUIManager.ToggleMapUI(); }                                                                       // �� UI ���ݱ�
    public static void OpenOasisUI(OasisNPC _npc) { PlayUIManager.OpenOasisUI(_npc); }                                                      // ȭ��� UI ����
    public static void CloseOasisUI() { PlayUIManager.CloseOasisUI(); }                                                                     // ȭ��� UI �ݱ�
    public static void UpdateSkillSlot() { PlayUIManager.UpdateSkillSlot(); }                                                               // ��ų ���� UI
    public static void UseSkillSlot(int _idx, float _cooltime) { PlayUIManager.UseSkillSlot(_idx, _cooltime); }                             // ��ų ��Ÿ�� ����
    public static void UpdateThrowItemSlot() { PlayUIManager.UpdateThrowItemSlot(); }                                                       // ������ ������ UI
    public static void UpdateHealItemSlot() { PlayUIManager.UpdateHealItemSlot(); }                                                         // ȸ�� ������ UI
    public static void UpdateInfoUI() { PlayUIManager.UpdateInfoUI(); }                                                                     // �÷��̾� ���� UI ������Ʈ
    public static void UpdateMaterials() { PlayUIManager.UpdateMaterials(); }                                                               // ��ȭ ������Ʈ
    public static void SetPlayerMaxHP(float _hp) { PlayUIManager.SetMaxHP(_hp); }                                                           // ü�¹� �ִ� ü��
    public static void SetPlayerCurHP(float _hp) { PlayUIManager.SetCurHP(_hp); }                                                           // ü�¹� ���� ü��
    public static void SetStaminaRate(float _rate) { PlayUIManager.SetStaminaRate(_rate); }                                                 // ���¹̳� ����
    public static void SetLightRate(float _rate) { PlayUIManager.SetLightRate(_rate); }                                                     // �ɷ� ����
    public static void ShowSkillAim(Vector3 _pos, float _radius, float _range) { PlayUIManager.ShowSkillAim(_pos, _radius, _range); }
    public static Vector3 TraceSkillAim(Vector3 _pos, float _range) { return PlayUIManager.TraceSkillAim(_pos, _range); }
    public static void HideSkillAim() { PlayUIManager.HideSkillAim(); }
    public static void DrawThrowLine(Vector3 _force, float _mass, Vector3 _start) { PlayUIManager.DrawThrowLine(_force, _mass, _start); }   // ������ ���� �׸���
    public static void HideThrowLine() { PlayUIManager.HideThrowLine(); }                                                                   // ������ ���� off



    private void SetSubManagers()
    {
        m_itemManager = GetComponent<ItemManager>();
        m_itemManager.SetManager();
        m_storyManager = GetComponent<StoryManager>();
        m_environmentManager = GetComponent<EnvironmentManager>();
        m_environmentManager.setManager();
        m_upgradeManager = GetComponent<UpgradeManager>();
        m_playUIManager = GetComponent<PlayUIManager>();
        m_playUIManager.SetManager();
    }

    private void Awake()
    {
        if (Inst != null) { Destroy(Inst.gameObject); }
        Inst = this;
        SetSubManagers();
    }
}
