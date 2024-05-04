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


    // 메인 함수
    public static bool IsPlaying { get { return Inst != null && Inst.gameObject != null; } }                // 플레이 중인지


    // 플레이어
    private static PlayerController Player { get; set; }
    public static void SetCurPlayer(PlayerController _player) { Player = _player; }                                                         // 플레이어 등록
    public static bool CheckIsPlayer(ObjectScript _object) { return _object == Player; }                                                    // 플레이어인지 확인
    public static bool IsPlayerSet { get { return Player != null; } }                                                                       // 플레이어 등록 여부
    public static bool IsPlayerGuarding { get { return Player.IsGuarding; } }                                                               // 플레이어 가드 중
    public static bool IsPlayerDead { get { return Player.IsDead; } }                                                                       // 플레이어 사망 여부
    public static bool IsPlayerLightOn { get { return Player.IsLightOn; } }                                                                 // 플레이어 능력 사용중
    public static Vector3 PlayerPos { get { if (IsPlayerSet) return Player.transform.position; return ValueDefine.NullVector; } }           // 플레이어 위치
    public static Vector2 PlayerPos2 { get { if (IsPlayerSet) return Player.Position2; return ValueDefine.NullVector; } }                   // 플레이어 평면 위치

    public static Vector2 PlayerAimDirection { get { return Player.PlayerAimDirection; } }                                                  // 카메라 조준 벡터
    public static Transform PlayerTransform { get { if (IsPlayerSet) return Player.transform; return null; } }
    public static PlayerStatInfo PlayerStatInfo { get { return Player.GetStatInfo(); } }
    public static SPlayerWeaponInfo PlayerWeaponInfo { get { return Player.CurWeaponInfo; } }
    public static float GetDistToPlayer(Vector3 _pos) { if (!IsPlayerSet) return -1; return (PlayerPos-_pos).magnitude; }                   // 플레이어와의 거리
    public static void SetPlayerWeapon(EWeaponName _weapon) { Player.SetCurWeapon(_weapon); }                                               // 무기 설정
    public static void StopPlayerInteract() { Player.StopInteract(); }                                                                      // 상호작용 종료
    public static void TeleportPlayer(Vector3 _pos) { Player.TeleportPlayer(_pos); }
    
    public static void TempGetBuff(float _amount, float _time) { Player.GetStatAdjust(new(EAdjType.MAX_HP, _amount, _time)); }
    

    // 카메라
    [SerializeField]
    private CameraManager m_cameraManager;
    private static CameraManager CameraManager { get { return Inst.m_cameraManager; } }
    public static CinemachineFreeLook PlayerFreeLook { get { return CameraManager.PlayerFreeLook; } }
    public static float CameraRotation { get { return CameraManager.CameraRotation; } }                                                     // 카메라 좌우 각도
    public static float CameraAngle { get { return CameraManager.CameraAngle; } }                                                           // 카메라 위아래 각도
    public static void SetCameraMode(EControlMode _mode) { CameraManager.SetCameraMode(_mode); }                                            // 조작 모드 전달
    public static void SetCameraSensitive(float _sensitive) { CameraManager.SetCameraSensitive(_sensitive); }                               // 마우스 민감도 전달
    public static void CameraSwitch(CinemachineFreeLook targetCamera) { CameraManager.SwitchToCamera(targetCamera); }                       // 카메라 변환


    // 아이템
    private ItemManager m_itemManager;
    private static ItemManager ItemManager { get { return Inst.m_itemManager; } }
    public static ItemInfo GetItemInfo(SItem _item) { return ItemManager.GetItemInfo(_item); }                                              // 아이템 정보
    public static ItemInfo GetItemInfo(string _id) { return ItemManager.GetItemInfo(_id); }                                                 // 아이템 정보
    public static ItemInfo GetWeaponInfo(EWeaponName _weapon) { return GetItemInfo(new SItem(EItemType.WEAPON, (int)_weapon)); }            // 무기 정보
    public static GameObject GetWeaponPrefab(EWeaponName _weapon) { return ItemManager.GetWeaponPrefab(_weapon); }                          // 무기 프리펍
    public static GameObject GetThorwItemPrefab(EThrowItemName _item) { return ItemManager.GetThrowItemPrefab(_item); }                     // 투척 아이템 프리펍
    public static GameObject GetDropItemPrefab(EItemType _item) { return ItemManager.GetDropItemPrefab(_item); }                            // 드랍 아이템 프리펍
    public static InventoryElm[] PlayerInventory { get { return ItemManager.Inventory; } }                                                  // 인벤토리 아이템 목록
    public static void AddInventoryItem(SItem _item, int _num) { ItemManager.AddInventoryItem(_item, _num); }                               // 빈 인벤토리에 아이템 추가
    public static void SetInventoryItem(int _idx, SItem _item, int _num) { ItemManager.SetInventoryItem(_idx, _item, _num); }               // 인벤토리 해당 Idx에 아이템 설정
    public static void RemoveInventoryItem(int _idx) { ItemManager.RemoveInventoryItem(_idx); }                                             // 인벤토리 해당 Idx 아이템 제거
        // 장비 관련
    public static EWeaponName CurWeapon { get { return ItemManager.CurWeapon; } }                                                           // 장착 중인 무기
    public static void SetCurWeapon(EWeaponName _weapon) { ItemManager.SetCurWeapon(_weapon); }                                             // 무기 설정
    public static void EquipWeapon(EWeaponName _weapon) { ItemManager.EquipWeapon(_weapon); }                                               // 무기 장착
    public static void ObtainWeapon(EWeaponName _weapon) { ItemManager.ObtainWeapon(_weapon); }                                             // 무기 획득
        // 등록 아이템 관련
    public static EPatternName CurHealPattern { get { return ItemManager.CurHealPattern; } }
    public static EPatternName[] HealPatternList { get { return ItemManager.HealPatternList; } }
    public static void UseHealPattern() { ItemManager.UseHealItem(); UpdateInfoUI(); UpdateHealItemSlot(); }
    public static void RegisterHealPattern(EPatternName _pattern) { ItemManager.RegisterHealItem(_pattern); UpdateInfoUI(); UpdateHealItemSlot(); }
    public static EThrowItemName CurThrowItem { get { return ItemManager.CurThrowItem; } }                                                  // 현재 던지기 아이템 (LAST == null)
    public static List<EThrowItemName> ThrowItemList { get { return ItemManager.ThrowItemList; } }                                          // 등록된 던지기 아이템
    public static void UseThrowItem() { ItemManager.UseThrowItem(); UpdateInfoUI(); UpdateThrowItemSlot(); }                                // 던지기 아이템 사용
    public static void AddThrowItem(EThrowItemName _item) { ItemManager.AddThrowItem(_item); UpdateInfoUI(); UpdateThrowItemSlot(); }       // 던지기 아이템 추가
        // 재화 관련
    public static int SoulNum { get { return ItemManager.SoulNum; } }                                                                       // 영혼 개수
    public static int PurifiedNum { get { return ItemManager.PurifiedNum; } }                                                               // 성불 영혼 개수
    public static int[] PatternNum { get { return ItemManager.PatternNum; } }                                                               // 문양별 개수
    public static void AddSoul(int _num) { ItemManager.AddSoul(_num); }                                                                     // 영혼 추가
    public static void AddPurified(int _num) { ItemManager.AddPurified(_num); }                                                             // 성불 영혼 추가
    public static void AddPattern(EProperty _type, int _num) { ItemManager.AddPattern(_type, _num); }                                       // 문양 추가
    public static void UseSoul(int _num) { ItemManager.UseSoul(_num); }                                                                     // 영혼 사용
    public static void UsePurified(int _num) { ItemManager.UsePurified(_num); }                                                             // 성불 영혼 사용
    public static void UsePattern(EProperty _type, int _num) { ItemManager.UsePattern(_type, _num); }                                       // 문양 사용

    // 스토리
    private StoryManager m_storyManager;
    private static StoryManager StoryManager { get { return Inst.m_storyManager; } }


    // 환경
    private EnvironmentManager m_environmentManager;
    private static EnvironmentManager EnvironmentManager { get { return Inst.m_environmentManager; } }
    public static Vector3 MapLB { get { return EnvironmentManager.m_mapPositioner[0].position; } }
    public static Vector3 MapRT { get { return EnvironmentManager.m_mapPositioner[1].position; } }
    public static float MapWidth { get { return EnvironmentManager.MapWidth; } }
    public static float MapHeight { get { return EnvironmentManager.MapHeight; } }

    public static Transform[] NormalizeObjects { get { return null; } }
    public static GameObject[] MapOasis { get { return null; } }


    // 업그레이드
    private UpgradeManager m_upgradeManager;
    private static UpgradeManager UpgradeManager { get { return Inst.m_upgradeManager; } }
    public static int LeftStatPoint { get { return UpgradeManager.LeftStatPoint; } }
    public static void AddStatPoint(int _add) { UpgradeManager.AddStatPoint(_add); }
    public static void UpgradeStat(int[] _point) { UpgradeManager.UpgradeStat(_point); }



    // GUI
    private PlayUIManager m_playUIManager;
    private static PlayUIManager PlayUIManager { get { return Inst.m_playUIManager; } }
    public static Vector2 NormalizeLocation(Transform _obj) { return PlayUIManager.NormalizeLocation(_obj); }                              // 위치 정규화(3D -> 2D)
    public static Canvas GetCanvas(ECanvasType _canvas) { return PlayUIManager.GetCanvas(_canvas); }                                        // 캔버스
    public static RectTransform CanvasTrans(ECanvasType _canvas) { return GetCanvas(_canvas).GetComponent<RectTransform>(); }               
    public static void OpenPlayerUI() { PlayUIManager.OpenPlayerUI(); }                                                                     // Player UI 열기
    public static void ClosePlayerUI() { PlayUIManager.ClosePlayerUI(); }                                                                   // Player UI 닫기
    public static void SetMinimapScale(float _scale) { PlayUIManager.SetMinimapScale(_scale); }                                             // 미니맵 축척
    public static void ToggleMapUI() { PlayUIManager.ToggleMapUI(); }                                                                       // 맵 UI 여닫기
    public static void OpenOasisUI(OasisNPC _npc) { PlayUIManager.OpenOasisUI(_npc); }                                                      // 화톳불 UI 열기
    public static void CloseOasisUI() { PlayUIManager.CloseOasisUI(); }                                                                     // 화톳불 UI 닫기
    public static void UpdateSkillSlot() { PlayUIManager.UpdateSkillSlot(); }                                                               // 스킬 슬롯 UI
    public static void UseSkillSlot(int _idx, float _cooltime) { PlayUIManager.UseSkillSlot(_idx, _cooltime); }                             // 스킬 쿨타임 진행
    public static void UpdateThrowItemSlot() { PlayUIManager.UpdateThrowItemSlot(); }                                                       // 던지기 아이템 UI
    public static void UpdateHealItemSlot() { PlayUIManager.UpdateHealItemSlot(); }                                                         // 회복 아이템 UI
    public static void UpdateInfoUI() { PlayUIManager.UpdateInfoUI(); }                                                                     // 플레이어 인포 UI 업데이트
    public static void UpdateMaterials() { PlayUIManager.UpdateMaterials(); }                                                               // 재화 업데이트
    public static void SetPlayerMaxHP(float _hp) { PlayUIManager.SetMaxHP(_hp); }                                                           // 체력바 최대 체력
    public static void SetPlayerCurHP(float _hp) { PlayUIManager.SetCurHP(_hp); }                                                           // 체력바 현재 체력
    public static void SetStaminaRate(float _rate) { PlayUIManager.SetStaminaRate(_rate); }                                                 // 스태미나 비율
    public static void SetLightRate(float _rate) { PlayUIManager.SetLightRate(_rate); }                                                     // 능력 비율
    public static void ShowSkillAim(Vector3 _pos, float _radius, float _range) { PlayUIManager.ShowSkillAim(_pos, _radius, _range); }
    public static Vector3 TraceSkillAim(Vector3 _pos, float _range) { return PlayUIManager.TraceSkillAim(_pos, _range); }
    public static void HideSkillAim() { PlayUIManager.HideSkillAim(); }
    public static void DrawThrowLine(Vector3 _force, float _mass, Vector3 _start) { PlayUIManager.DrawThrowLine(_force, _mass, _start); }   // 던지기 궤적 그리기
    public static void HideThrowLine() { PlayUIManager.HideThrowLine(); }                                                                   // 던지기 궤적 off



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
