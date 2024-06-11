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
    public static void ApplyPlayerStat() { Player.ApplyStat(); }
    public static SPlayerWeaponInfo PlayerWeaponInfo { get { return Player.CurWeaponInfo; } }
    public static float GetDistToPlayer(Vector3 _pos) { if (!IsPlayerSet) return -1; return (PlayerPos-_pos).magnitude; }                   // �÷��̾���� �Ÿ�
    public static void SetPlayerWeapon(EWeaponName _weapon) { Player.SetCurWeapon(_weapon); }                                               // ���� ����
    public static void StopPlayerInteract() { Player.StopInteract(); }                                                                      // ��ȣ�ۿ� ����
    public static void ResetPlayer() { Player.ResetPlayer(); }
    public static void TeleportPlayer(Vector3 _pos) { Player.TeleportPlayer(_pos); }
    
    public static void TempGetBuff(float _amount, float _time) { Player.GetAdj(new(EAdjType.MAX_HP, _amount, _time)); }
    

    // ī�޶�
    [SerializeField]
    private CameraManager m_cameraManager;
    private static CameraManager CameraManager { get { return Inst.m_cameraManager; } }
    public static CinemachineFreeLook PlayerFreeLook { get { return CameraManager.PlayerFreeLook; } }
    public static float CameraRotation { get { return CameraManager.CameraRotation; } }                                                     // ī�޶� �¿� ����
    public static float CameraAngle { get { return CameraManager.CameraAngle; } }                                                           // ī�޶� ���Ʒ� ����
    public static void SetCameraMode(EControlMode _mode) { CameraManager.SetCameraMode(_mode); }                                            // ���� ��� ����
    public static void SetCameraSensitive(float _sensitive) { CameraManager.SetCameraSensitive(_sensitive); }                               // ���콺 �ΰ��� ����
    public static void CameraSwitch(CinemachineFreeLook _targetCamera) { CameraManager.SwitchToCamera(_targetCamera); }                    // ī�޶� ��ȯ


    // �κ��丮
    private InventoryManager m_invenManager;
    private static InventoryManager InvenManager { get { return Inst.m_invenManager; } }
    public static InventoryElm[] PlayerInventory { get { return InvenManager.Inventory; } }                                                     // �κ��丮 ������ ���
    private static void InventoryEditted() { UpdateInfoUI(); }
    public static void AddInventoryItem(SItem _item, int _num) { InvenManager.AddInventoryItem(_item, _num); }                                  // �� �κ��丮�� ������ �߰�
    public static void SetInventoryItem(int _idx, SItem _item, int _num) { InvenManager.SetInventoryItem(_idx, _item, _num); }                  // �κ��丮 �ش� Idx�� ������ ����
    public static void RemoveInventoryItem(int _idx) { InvenManager.RemoveInventoryItem(_idx); }                                                // �κ��丮 �ش� Idx ������ ����
    public static void SwapItemInven(int _idx1, int _idx2) { InvenManager.SwapItemInven(_idx1, _idx2); InventoryEditted(); }
    public static EWeaponName CurWeapon { get { return InvenManager.CurWeapon; } }                                                              // ���� ���� ����
    public static void SetCurWeapon(EWeaponName _weapon) { InvenManager.SetCurWeapon(_weapon); }                                                // ���� ����
    public static void EquipWeapon(EWeaponName _weapon) { InvenManager.EquipWeapon(_weapon); }                                                  // ���� ����
    public static EPatternName CurHealPattern { get { return InvenManager.CurHealPattern; } }                                                   // ���� ȸ�� ������
    public static EPatternName[] HealPatternList { get { return InvenManager.HealPatternList; } }                                               // ��ϵ� ȸ�� ������
    private static void HealPatternEditted() { UpdateInfoUI(); UpdateHealItemSlot(); }
    public static void UseHealPattern() { InvenManager.UseHealItem(); HealPatternEditted(); }                                                   // ȸ�� ������ ���
    public static void RegisterHealPattern(EPatternName _pattern) { InvenManager.RegisterHealItem(_pattern); HealPatternEditted(); }            // ���
    public static EThrowItemName CurThrowItem { get { return InvenManager.CurThrowItem; } }                                                     // ���� ������ ������ (LAST == null)
    public static List<EThrowItemName> ThrowItemList { get { return InvenManager.ThrowItemList; } }                                             // ��ϵ� ������ ������
    private static void ThrowItemEditted() { UpdateInfoUI(); UpdateThrowItemSlot(); }
    public static void UseThrowItem() { InvenManager.UseThrowItem(); UpdateInfoUI(); UpdateThrowItemSlot(); }                                   // ������ ������ ���
    public static void AddThrowItem(EThrowItemName _item) { InvenManager.AddThrowItem(_item); ThrowItemEditted(); }                             // ������ ������ �߰�
    public static void SetThrowItem(int _idx, EThrowItemName _item) { InvenManager.SetThrowItem(_idx, _item); ThrowItemEditted(); }             // ��ġ ����
    public static void SwapThrowItem(int _idx1, int _idx2) { InvenManager.SwapThrowItem(_idx1, _idx2); ThrowItemEditted(); }                    // �ٲٱ�
    public static void RemoveThrowItem(int _idx) { InvenManager.RemoveThrowItem(_idx); ThrowItemEditted(); }                                    // ����
    public static int SoulNum { get { return InvenManager.SoulNum; } }                                                                          // ��ȥ ����
    public static int PurifiedNum { get { return InvenManager.PurifiedNum; } }                                                                  // ���� ��ȥ ����
    public static int[] PatternNum { get { return InvenManager.PatternNum; } }                                                                  // ���纰 ����
    public static void AddSoul(int _num) { InvenManager.AddSoul(_num); }                                                                        // ��ȥ �߰�
    public static void AddPurified(int _num) { InvenManager.AddPurified(_num); }                                                                // ���� ��ȥ �߰�
    public static void AddPattern(EProperty _type, int _num) { InvenManager.AddPattern(_type, _num); }                                          // ���� �߰�
    public static void UseSoul(int _num) { InvenManager.UseSoul(_num); }                                                                        // ��ȥ ���
    public static void UsePurified(int _num) { InvenManager.UsePurified(_num); }                                                                // ���� ��ȥ ���
    public static void UsePattern(EProperty _type, int _num) { InvenManager.UsePattern(_type, _num); }                                          // ���� ���

    // ���丮
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


    // ȯ��
    private EnvironmentManager m_environmentManager;
    private static EnvironmentManager EnvironmentManager { get { return Inst.m_environmentManager; } }
    public static Vector3 MapLB { get { return EnvironmentManager.m_mapPositioner[0].position; } }
    public static Vector3 MapRT { get { return EnvironmentManager.m_mapPositioner[1].position; } }
    public static float MapWidth { get { return EnvironmentManager.MapWidth; } }
    public static float MapHeight { get { return EnvironmentManager.MapHeight; } }

    public static GameObject[] MapOasis { get { return EnvironmentManager.MapOasis; } }
    public static NPCScript[] NPCs { get { return EnvironmentManager.NPCs; } }


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
    public static void ShowInteractInfo(string _info) { PlayUIManager.ShowInteractInfo(_info); }
    public static void HideInteractInfo() { PlayUIManager.HideInteractInfo(); }
    public static void ToggleMapUI() { PlayUIManager.ToggleMapUI(); }                                                                       // �� UI ���ݱ�
    public static void ToggleQuestUI() { PlayUIManager.ToggleQuestUI(); }                                                                   // ����Ʈ â ���ݱ�
    public static void ShowNPCQuestUI() { PlayUIManager.ShowNPCQuestUI(); }                                                                 // ����Ʈ ����/���� â ǥ��
    public static void ExpressCurQuestInfo() { PlayUIManager.ExpressCurQuestInfo(); }                                                       // ���� ����Ʈ ���� ǥ��
    public static void ChangeBtnsTxt() { PlayUIManager.ChangeBtnsTxt(); }
    public static Image[] ClearImg { get { return PlayUIManager.ClearImg; } }
    public static void OpenNPCUI(QuestNPCScript _npc) { PlayUIManager.OpenDialogueUI(_npc); }                                               // NPC ��ȭâ ����
    public static void CloseNPCUI() { PlayUIManager.CloseDialogueUI(); }                                                                    // NPC ��ȭâ �ݱ�
    public static bool IsDialogueOpend { get { return PlayUIManager.IsDialogueUIOpend; } }                                                  // NPC ��ȭâ ���ȴ��� Ȯ��
    public static void ShowNextDialogue() { PlayUIManager.ShowNextDialogue(); }                                                             // ���� ��ȭ ���
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
    public static void SetLightState(bool _on) { PlayUIManager.SetLightState(_on); }                                                        // �� ����
    public static void ShowRaycastAim() { PlayUIManager.ShowRaycastAim(); }                                                                 // ����ĳ��Ʈ ���� on
    public static void SetRaycastAimState(bool _on) { PlayUIManager.SetRaycastAimState(_on); }                                              // ����ĳ��Ʈ ���� ����
    public static void HideRaycastAim() { PlayUIManager.HideRaycastAim(); }                                                                 // ����ĳ��Ʈ ���� off
    public static void ShowSkillAim(Vector3 _pos, float _radius, float _range) { PlayUIManager.ShowSkillAim(_pos, _radius, _range); }
    public static Vector3 TraceSkillAim(Vector3 _pos, float _range) { return PlayUIManager.TraceSkillAim(_pos, _range); }
    public static void HideSkillAim() { PlayUIManager.HideSkillAim(); }
    public static void DrawThrowLine(Vector3 _force, float _mass, Vector3 _start) { PlayUIManager.DrawThrowLine(_force, _mass, _start); }   // ������ ���� �׸���
    public static void HideThrowLine() { PlayUIManager.HideThrowLine(); }                                                                   // ������ ���� off
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
