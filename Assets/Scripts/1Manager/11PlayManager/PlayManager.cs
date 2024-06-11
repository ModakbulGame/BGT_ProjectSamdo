using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public static void ApplyPlayerStat() { Player.ApplyStat(); }
    public static SPlayerWeaponInfo PlayerWeaponInfo { get { return Player.CurWeaponInfo; } }
    public static float GetDistToPlayer(Vector3 _pos) { if (!IsPlayerSet) return -1; return (PlayerPos-_pos).magnitude; }                   // 플레이어와의 거리
    public static void SetPlayerWeapon(EWeaponName _weapon) { Player.SetCurWeapon(_weapon); }                                               // 무기 설정
    public static void StopPlayerInteract() { Player.StopInteract(); }                                                                      // 상호작용 종료
    public static void ResetPlayer() { Player.ResetPlayer(); }
    public static void TeleportPlayer(Vector3 _pos) { Player.TeleportPlayer(_pos); }
    
    public static void TempGetBuff(float _amount, float _time) { Player.GetAdj(new(EAdjType.MAX_HP, _amount, _time)); }
    

    // 카메라
    [SerializeField]
    private CameraManager m_cameraManager;
    private static CameraManager CameraManager { get { return Inst.m_cameraManager; } }
    public static CinemachineFreeLook PlayerFreeLook { get { return CameraManager.PlayerFreeLook; } }
    public static float CameraRotation { get { return CameraManager.CameraRotation; } }                                                     // 카메라 좌우 각도
    public static float CameraAngle { get { return CameraManager.CameraAngle; } }                                                           // 카메라 위아래 각도
    public static void SetCameraMode(EControlMode _mode) { CameraManager.SetCameraMode(_mode); }                                            // 조작 모드 전달
    public static void SetCameraSensitive(float _sensitive) { CameraManager.SetCameraSensitive(_sensitive); }                               // 마우스 민감도 전달
    public static void CameraSwitch(CinemachineFreeLook _targetCamera) { CameraManager.SwitchToCamera(_targetCamera); }                    // 카메라 변환


    // 인벤토리
    private InventoryManager m_invenManager;
    private static InventoryManager InvenManager { get { return Inst.m_invenManager; } }
    public static InventoryElm[] PlayerInventory { get { return InvenManager.Inventory; } }                                                     // 인벤토리 아이템 목록
    private static void InventoryEditted() { UpdateInfoUI(); }
    public static void AddInventoryItem(SItem _item, int _num) { InvenManager.AddInventoryItem(_item, _num); }                                  // 빈 인벤토리에 아이템 추가
    public static void SetInventoryItem(int _idx, SItem _item, int _num) { InvenManager.SetInventoryItem(_idx, _item, _num); }                  // 인벤토리 해당 Idx에 아이템 설정
    public static void RemoveInventoryItem(int _idx) { InvenManager.RemoveInventoryItem(_idx); }                                                // 인벤토리 해당 Idx 아이템 제거
    public static void SwapItemInven(int _idx1, int _idx2) { InvenManager.SwapItemInven(_idx1, _idx2); InventoryEditted(); }
    public static EWeaponName CurWeapon { get { return InvenManager.CurWeapon; } }                                                              // 장착 중인 무기
    public static void SetCurWeapon(EWeaponName _weapon) { InvenManager.SetCurWeapon(_weapon); }                                                // 무기 설정
    public static void EquipWeapon(EWeaponName _weapon) { InvenManager.EquipWeapon(_weapon); }                                                  // 무기 장착
    public static EPatternName CurHealPattern { get { return InvenManager.CurHealPattern; } }                                                   // 현재 회복 아이템
    public static EPatternName[] HealPatternList { get { return InvenManager.HealPatternList; } }                                               // 등록된 회복 아이템
    private static void HealPatternEditted() { UpdateInfoUI(); UpdateHealItemSlot(); }
    public static void UseHealPattern() { InvenManager.UseHealItem(); HealPatternEditted(); }                                                   // 회복 아이템 사용
    public static void RegisterHealPattern(EPatternName _pattern) { InvenManager.RegisterHealItem(_pattern); HealPatternEditted(); }            // 등록
    public static EThrowItemName CurThrowItem { get { return InvenManager.CurThrowItem; } }                                                     // 현재 던지기 아이템 (LAST == null)
    public static List<EThrowItemName> ThrowItemList { get { return InvenManager.ThrowItemList; } }                                             // 등록된 던지기 아이템
    private static void ThrowItemEditted() { UpdateInfoUI(); UpdateThrowItemSlot(); }
    public static void UseThrowItem() { InvenManager.UseThrowItem(); UpdateInfoUI(); UpdateThrowItemSlot(); }                                   // 던지기 아이템 사용
    public static void AddThrowItem(EThrowItemName _item) { InvenManager.AddThrowItem(_item); ThrowItemEditted(); }                             // 던지기 아이템 추가
    public static void SetThrowItem(int _idx, EThrowItemName _item) { InvenManager.SetThrowItem(_idx, _item); ThrowItemEditted(); }             // 위치 지정
    public static void SwapThrowItem(int _idx1, int _idx2) { InvenManager.SwapThrowItem(_idx1, _idx2); ThrowItemEditted(); }                    // 바꾸기
    public static void RemoveThrowItem(int _idx) { InvenManager.RemoveThrowItem(_idx); ThrowItemEditted(); }                                    // 제거
    public static int SoulNum { get { return InvenManager.SoulNum; } }                                                                          // 영혼 개수
    public static int PurifiedNum { get { return InvenManager.PurifiedNum; } }                                                                  // 성불 영혼 개수
    public static int[] PatternNum { get { return InvenManager.PatternNum; } }                                                                  // 문양별 개수
    public static void AddSoul(int _num) { InvenManager.AddSoul(_num); }                                                                        // 영혼 추가
    public static void AddPurified(int _num) { InvenManager.AddPurified(_num); }                                                                // 성불 영혼 추가
    public static void AddPattern(EProperty _type, int _num) { InvenManager.AddPattern(_type, _num); }                                          // 문양 추가
    public static void UseSoul(int _num) { InvenManager.UseSoul(_num); }                                                                        // 영혼 사용
    public static void UsePurified(int _num) { InvenManager.UsePurified(_num); }                                                                // 성불 영혼 사용
    public static void UsePattern(EProperty _type, int _num) { InvenManager.UsePattern(_type, _num); }                                          // 문양 사용

    // 스토리
    private StoryManager m_storyManager;
    private static StoryManager StoryManager { get { return Inst.m_storyManager; } }
    public static List<QuestScriptable> QuestList { get { return StoryManager.QuestList; } }
    public static List<QuestScriptable> CurQuestList { get { return StoryManager.CurQuestList; } }
    public static void AcceptQuest(string _id) { StoryManager.AcceptQuest(_id); }
    public static void GiveUpQuest(string _id) { StoryManager.GiveUpQuest(_id); }
    public static void ClearQuest(string _id) { StoryManager.ClearQuest(_id); }
    public static void CompleteQuest(string _id) { StoryManager.CompleteQuest(_id); }
    public static void DoObjectQuest(string _obj, int _amount) { StoryManager.DoObjectQuest(_obj, _amount); }
    public static void SetQuestStartObjectStatus(string _start) { StoryManager.SetQuestStartObjectStatus(_start); }
    public static void SetQuestEndObjectStatus(string _end) { StoryManager.SetQuestEndObjectStatus(_end); }
    public static bool CheckRequiredQuestObject(string _name) { return StoryManager.CheckRequiredQuestObject(_name); }


    // 환경
    private EnvironmentManager m_environmentManager;
    private static EnvironmentManager EnvironmentManager { get { return Inst.m_environmentManager; } }
    public static Vector3 MapLB { get { return EnvironmentManager.m_mapPositioner[0].position; } }
    public static Vector3 MapRT { get { return EnvironmentManager.m_mapPositioner[1].position; } }
    public static float MapWidth { get { return EnvironmentManager.MapWidth; } }
    public static float MapHeight { get { return EnvironmentManager.MapHeight; } }

    public static GameObject[] MapOasis { get { return EnvironmentManager.MapOasis; } }
    public static NPCScript[] NPCs { get { return EnvironmentManager.NPCs; } }


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
    public static void ShowInteractInfo(string _info) { PlayUIManager.ShowInteractInfo(_info); }
    public static void HideInteractInfo() { PlayUIManager.HideInteractInfo(); }
    public static void ToggleMapUI() { PlayUIManager.ToggleMapUI(); }                                                                       // 맵 UI 여닫기
    public static void ToggleQuestUI() { PlayUIManager.ToggleQuestUI(); }                                                                   // 퀘스트 창 여닫기
    public static void ShowNPCQuestUI() { PlayUIManager.ShowNPCQuestUI(); }                                                                 // 퀘스트 수락/거절 창 표시
    public static void ExpressCurQuestInfo() { PlayUIManager.ExpressCurQuestInfo(); }                                                       // 현재 퀘스트 정보 표시
    public static void ChangeBtnsTxt() { PlayUIManager.ChangeBtnsTxt(); }
    public static Image[] ClearImg { get { return PlayUIManager.ClearImg; } }
    public static void OpenNPCUI(QuestNPCScript _npc) { PlayUIManager.OpenDialogueUI(_npc); }                                               // NPC 대화창 열기
    public static void CloseNPCUI() { PlayUIManager.CloseDialogueUI(); }                                                                    // NPC 대화창 닫기
    public static bool IsDialogueOpend { get { return PlayUIManager.IsDialogueUIOpend; } }                                                  // NPC 대화창 열렸는지 확인
    public static void ShowNextDialogue() { PlayUIManager.ShowNextDialogue(); }                                                             // 다음 대화 출력
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
    public static void SetLightState(bool _on) { PlayUIManager.SetLightState(_on); }                                                        // 고갈 설정
    public static void ShowRaycastAim() { PlayUIManager.ShowRaycastAim(); }                                                                 // 레이캐스트 에임 on
    public static void SetRaycastAimState(bool _on) { PlayUIManager.SetRaycastAimState(_on); }                                              // 레이캐스트 에임 상태
    public static void HideRaycastAim() { PlayUIManager.HideRaycastAim(); }                                                                 // 레이캐스트 에임 off
    public static void ShowSkillAim(Vector3 _pos, float _radius, float _range) { PlayUIManager.ShowSkillAim(_pos, _radius, _range); }
    public static Vector3 TraceSkillAim(Vector3 _pos, float _range) { return PlayUIManager.TraceSkillAim(_pos, _range); }
    public static void HideSkillAim() { PlayUIManager.HideSkillAim(); }
    public static void DrawThrowLine(Vector3 _force, float _mass, Vector3 _start) { PlayUIManager.DrawThrowLine(_force, _mass, _start); }   // 던지기 궤적 그리기
    public static void HideThrowLine() { PlayUIManager.HideThrowLine(); }                                                                   // 던지기 궤적 off
    public static void ShowBlindMark() { PlayUIManager.ShowBlindMark(); }
    public static void HideBlindMark() { PlayUIManager.HideBlindMark(); }



    private void SetSubManagers()
    {
        m_invenManager = GetComponent<InventoryManager>();
        m_invenManager.SetManager();
        m_storyManager = GetComponent<StoryManager>();
        m_storyManager.SetManager();
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
