using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    public static PlayManager Inst;


    // ���� �Լ�
    public static bool IsPlaying { get; private set; }                // �÷��� ������
    public static SaveData CurSaveData { get; private set; }
    public static bool IsNewData { get { return CurSaveData == null; } }
    private static bool IsFromTitle { get; set; }
    public static void SetCurData(SaveData _data) { CurSaveData = _data; IsFromTitle = true; }
    private static void StartPlay()
    {
        if (IsNewData && IsFromTitle) { CurSaveData = new(); GameManager.AddGameData(CurSaveData); }
        IsPlaying = true;
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
        GameManager.SetMouseSensitive(1);
    }
    public static void EndPlay()
    {
        IsPlaying = false;
        IsFromTitle = false;
    }
    public static void RestAtPoint(OasisNPC _oasis)
    {
        StartBlackoutUI();
        Player.RestAnimation();
        Inst.StartCoroutine(Inst.RestDoneCoroutine(_oasis));
    }
    private IEnumerator RestDoneCoroutine(OasisNPC _oasis)
    {
        yield return new WaitForSeconds(2);
        TeleportPlayer(_oasis.RespawnPoint);
        Player.RestorePlayer();
        yield return new WaitForSeconds(1);
        EndBlackoutUI();
        GameManager.SaveGameData(_oasis.PointName);
    }
    public static void TransportToOasis(EOasisName _target)
    {
        StartBlackoutUI();
        Inst.StartCoroutine(Inst.TransportCoroutine(_target));
    }
    private IEnumerator TransportCoroutine(EOasisName _oasis)
    {
        yield return new WaitForSeconds(2);
        TeleportPlayer(OasisList[(int)_oasis].RespawnPoint);
        yield return new WaitForSeconds(1);
        EndBlackoutUI();
    }
    public static void PlayerDead()
    {

    }
    public static void RestartGame(SaveData _data)
    {

    }


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
    public static float PlayerDirection { get { return Player.Direction; } }
    public static Vector2 PlayerAimDirection { get { return Player.PlayerAimDirection; } }                                                  // ī�޶� ���� ����
    public static Transform PlayerTransform { get { if (IsPlayerSet) return Player.transform; return null; } }
    public static Transform CameraFocusTransform { get { if (IsPlayerSet) { return Player.CameraFocus; } return null; } }
    public static PlayerStatInfo PlayerStatInfo { get { return Player.GetStatInfo(); } }
    public static void ApplyStatReset() { Player.ResetStatInfo(); }
    public static void ApplyPlayerStat() { Player.ApplyStat(); }
    public static SPlayerWeaponInfo PlayerWeaponInfo { get { return Player.CurWeaponInfo; } }
    public static float GetDistToPlayer(Vector3 _pos) { if (!IsPlayerSet) return -1; return (PlayerPos-_pos).magnitude; }                   // �÷��̾���� �Ÿ�
    public static void SetPlayerWeapon(EWeaponName _weapon) { Player.SetCurWeapon(_weapon); }                                               // ���� ����
    public static void StopPlayerInteract() { Player.StopInteract(); }                                                                      // ��ȣ�ۿ� ����
    public static void ResetPlayer() { Player.ResetPlayerAction(); }
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
    public static void SetNPCView() { CameraManager.SetNPCView(); }
    public static void SetCameraSensitive(float _sensitive) { CameraManager.SetCameraSensitive(_sensitive); }                               // ���콺 �ΰ��� ����
    public static void LooseCameraFocus() { CameraManager.LooseFocus(); }
    public static void CameraSwitch(CinemachineFreeLook _targetCamera) { CameraManager.SwitchToCamera(_targetCamera); }                    // ī�޶� ��ȯ


    // �κ��丮
    private InventoryManager m_invenManager;
    private static InventoryManager InvenManager { get { return Inst.m_invenManager; } }
    public static InventoryElm[] PlayerInventory { get { return InvenManager.Inventory; } }                                                     // �κ��丮 ������ ���
    private static void InventoryEditted() { UpdateInfoUI(); }
    public static void AddInventoryItem(SItem _item, int _num) { InvenManager.AddInventoryItem(_item, _num); }                                  // �� �κ��丮�� ������ �߰�
    public static void AddInventoryItem(SItem _item, int _num, bool _isNew) { InvenManager.AddInventoryItem(_item, _num, _isNew); }             // �� �κ��丮�� ������ �߰� (�ű�)
    public static void SetInventoryItem(int _idx, SItem _item, int _num) { InvenManager.SetInventoryItem(_idx, _item, _num); }                  // �κ��丮 �ش� Idx�� ������ ����
    public static void RemoveInventoryItem(int _idx) { InvenManager.RemoveInventoryItem(_idx); }                                                // �κ��丮 �ش� Idx ������ ����
    public static void SwapItemInven(int _idx1, int _idx2) { InvenManager.SwapItemInven(_idx1, _idx2); InventoryEditted(); }
    public static bool[] WeaponObtained { get { return InvenManager.WeaponObatined; } }
    public static EWeaponName CurWeapon { get { return InvenManager.CurWeapon; } }                                                              // ���� ���� ����
    public static void ObtainWeapon(EWeaponName _weapon) { InvenManager.ObtainWeapon(_weapon); }
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
    public static void UseSoul(int _num) { InvenManager.LooseSoul(_num); }                                                                      // ��ȥ ���
    public static void LooseSoul(int _num, bool _absorbed) { InvenManager.LooseSoul(_num, _absorbed); }                                         // ��ȥ ���� ����
    public static void UsePurified(int _num) { InvenManager.UsePurified(_num); }                                                                // ���� ��ȥ ���

    // ���丮
    private QuestManager m_questManager;
    private static QuestManager QuestManager { get { return Inst.m_questManager; } }
    public static List<QuestInfo> QuestInfoList { get { return QuestManager.QuestInfoList; } }
    public static void SetQuestStatus(EQuestName _quest, EQuestState _status) { QuestManager.SetQuestStatus(_quest, _status); }
    public static void SetQuestProgress(EQuestName _quest, float _prog) { QuestManager.SetQuestProgress(_quest, _prog); }


    // ȯ��
    private EnvironmentManager m_environmentManager;
    private static EnvironmentManager EnvironmentManager { get { return Inst.m_environmentManager; } }
    public static Vector3 MapLB { get { return EnvironmentManager.MapLB; } }
    public static Vector3 MapRT { get { return EnvironmentManager.MapRT; } }
    public static float MapWidth { get { return EnvironmentManager.MapWidth; } }
    public static float MapHeight { get { return EnvironmentManager.MapHeight; } }
    public static OasisNPC[] OasisList { get { return EnvironmentManager.OasisList; } }
    public static AltarNPC[] AltarList { get { return EnvironmentManager.AltarList; } }
    public static SlateNPC[] SlateList { get { return EnvironmentManager.SlateList; } }
    public static MonsterSpawnPoint[] SpawnPointList { get { return EnvironmentManager.SpawnPointList; } }
    public static void MonsterKilled(EMonsterName _monster, EMonsterDeathType _type) { EnvironmentManager.MonsterKilled(_monster, _type); }
    public static void UnlockDialogue(NPCDialogue _dial) { EnvironmentManager.UnlockDialogue(_dial); }

    public static void TempSetNPCs(NPCScript[] _list) { EnvironmentManager.TempSetNPCs(_list); }                // �ӽ� �Լ�


    // �÷��̾� �ɷ�ġ, �Ǵ�
    private PlayerForceManager m_forceManager;
    private static PlayerForceManager ForceManager { get { return Inst.m_forceManager; } }
    public static int LeftStatPoint { get { return ForceManager.LeftStatPoint; } }
    public static int UsedStatPoint { get { return ForceManager.UsedStatPoint; } }
    public static void AddStatPoint(int _add) { ForceManager.AddStatPoint(_add); }
    public static void UpgradeStat(int[] _point) { ForceManager.UpgradeStat(_point); }                                                                  // ���� ����
    public static void UpgradeStat(EStatName _stat, int _amount) { ForceManager.UpgradeStat(_stat, _amount, true); }                                    // ���� ����
    public static void ResetStat() { ForceManager.ResetStat(); }
    public static bool[] PowerObtained { get { return ForceManager.PowerObtained; } }
    public static EPowerName[] PowerSlot { get { return ForceManager.PowerSlot; } }                                                                     // ��ų ����
    public static void RegisterPowerSlot(EPowerName _popwer, int _idx) { ForceManager.RegisterPowerSlot(_popwer, _idx); UpdatePowerSlot(); }            // ��ų ���� ����
    public static void ObtainPower(EPowerName _power) { ForceManager.ObtainPower(_power); }


    // GUI
    private PlayUIManager m_playUIManager;
    private static PlayUIManager PlayUIManager { get { return Inst.m_playUIManager; } }

        // �¿��� UI
    public static bool IsOptionOpen { get { return PlayUIManager.IsOptionOpen; } }
    public static void ToggleOptionUI(bool _on) { PlayUIManager.ToggleOptionUI(_on); }                                                                  // �ɼ� UI ���ݱ�
    public static bool IsPlayerUIOpen { get { return PlayUIManager.IsPlayerUIOpen; } }
    public static void TogglePlayerUI(bool _on) { PlayUIManager.TogglePlayerUI(_on); }                                                      // �÷��̾� ���� UI ���ݱ�
    public static void UpdateInfoUI() { PlayUIManager.UpdateInfoUI(); }                                                                     // �÷��̾� ���� UI ������Ʈ
    public static void UpdateMaterials() { PlayUIManager.UpdateMaterials(); }                                                               // ��ȭ ������Ʈ
    public static void ToggleMapUI() { PlayUIManager.ToggleMapUI(); }                                                                       // �� UI ���ݱ�
    public static bool IsQuestUIOpen { get { return PlayUIManager.IsQuestUIOpen; } }
    public static void ToggleQuestUI(bool _on) { PlayUIManager.ToggleQuestUI(_on); }                                                        // ����Ʈ â ���ݱ�
    public static void OpenOasisUI(OasisNPC _npc) { PlayUIManager.OpenOasisUI(_npc); }                                                      // ���ƽý� UI ����
    public static void CloseOasisUI() { PlayUIManager.CloseOasisUI(); }                                                                     // ���ƽý� UI �ݱ�

        // ���� ĵ���� ��� UI
    public static void SetPlayerMaxHP(float _hp) { PlayUIManager.SetMaxHP(_hp); }                                                                       // ü�¹� �ִ� ü��
    public static void SetPlayerCurHP(float _hp) { PlayUIManager.SetCurHP(_hp); }                                                                       // ü�¹� ���� ü��
    public static void UpdatePowerSlot() { PlayUIManager.UpdatePowerSlot(); }                                                                           // ��ų ���� UI
    public static void UsePowerSlot(int _idx, float _cooltime) { PlayUIManager.UsePowerSlot(_idx, _cooltime); }                                         // ��ų ��Ÿ�� ����
    public static void UpdateThrowItemSlot() { PlayUIManager.UpdateThrowItemSlot(); }                                                                   // ������ ������ UI
    public static void UpdateHealItemSlot() { PlayUIManager.UpdateHealItemSlot(); }                                                                     // ȸ�� ������ UI
    public static void UpdateQuestSidebar() { PlayUIManager.UpdateQuestSideBar(); }                                                                     // ����Ʈ �� ������Ʈ
    public static void SetMinimapScale(float _scale) { PlayUIManager.SetMinimapScale(_scale); }                                                         // �̴ϸ� ��ô ����
    public static void SetStaminaRate(float _rate) { PlayUIManager.SetStaminaRate(_rate); }                                                             // ���¹̳� ����
    public static void SetLightRate(float _rate) { PlayUIManager.SetLightRate(_rate); }                                                                 // �ɷ� ����
    public static void SetLightState(bool _on) { PlayUIManager.SetLightState(_on); }                                                                    // ���� ����
    public static void ShowRaycastAim() { PlayUIManager.ShowRaycastAim(); }                                                                             // ����ĳ��Ʈ ���� on
    public static void SetRaycastAimState(bool _on) { PlayUIManager.SetRaycastAimState(_on); }                                                          // ����ĳ��Ʈ ���� ����
    public static void HideRaycastAim() { PlayUIManager.HideRaycastAim(); }                                                                             // ����ĳ��Ʈ ���� off

        // �ΰ��� UI
    public static void AddIngameAlarm(string _alarm) { PlayUIManager.AddAlarm(_alarm); }                                                                      // �ΰ��� �˶�
    public static void ShowInteractInfo(string _info) { PlayUIManager.ShowInteractInfo(_info); }                                                        // ��ȣ�ۿ� Ű on
    public static void HideInteractInfo() { PlayUIManager.HideInteractInfo(); }                                                                         // ��ȣ�ۿ� Ű off
    public static void ShowPowerAim(Vector3 _pos, float _radius, float _range) { PlayUIManager.ShowPowerAim(_pos, _radius, _range); }                   // �Ǵ� ���� on
    public static Vector3 TracePowerAim(Vector3 _pos, float _range) { return PlayUIManager.TracePowerAim(_pos, _range); }                               // �Ǵ� ���� ��ġ ����
    public static void HidePowerAim() { PlayUIManager.HidePowerAim(); }                                                                                 // �Ǵ� ���� off
    public static void DrawThrowLine(Vector3 _force, float _mass, Vector3 _start) { PlayUIManager.DrawThrowLine(_force, _mass, _start); }               // ������ ���� �׸���
    public static void HideThrowLine() { PlayUIManager.HideThrowLine(); }                                                                               // ������ ���� off
    private static void StartBlackoutUI() { PlayUIManager.StartBlackout(); }                                                                            // fade ����
    private static void EndBlackoutUI() { PlayUIManager.EndBlackout(); }                                                                                // fade ����
    public static void ShowBlindMark() { PlayUIManager.ShowBlindMark(); }                                                                               // �Ǹ� on
    public static void HideBlindMark() { PlayUIManager.HideBlindMark(); }                                                                               // �Ǹ� off

    // NPC UI
    public static void ShowNPCQuestUI(EQuestName _quest, bool _isStart, FPointer _confirm) { PlayUIManager.ShowNPCQuestUI(_quest, _isStart, _confirm); }    // ����Ʈ ����/���� â ǥ��
    public static void OpenDialogueUI(NPCScript _npc, int _idx) { PlayUIManager.OpenDialogueUI(_npc, _idx); }                                           // NPC ��ȭâ ����
    public static void OpenSlateUI(SlateNPC _slate) { PlayUIManager.OpenSlateUI(_slate); }
    public static bool IsDialogueOpend { get { return PlayUIManager.IsDialogueUIOpend; } }                                                              // NPC ��ȭâ ���ȴ��� Ȯ��

    // ��Ÿ
    public static Vector2 NormalizeLocation(Transform _obj) { return PlayUIManager.NormalizeLocation(_obj); }                                           // ��ġ ����ȭ(3D -> 2D)



    private void SetSubManagers()
    {
        m_invenManager = GetComponent<InventoryManager>();
        m_invenManager.SetManager();
        m_questManager = GetComponent<QuestManager>();                  // Quest�� Environment���� ���� �־����
        m_questManager.SetManager();
        m_environmentManager = GetComponent<EnvironmentManager>();
        m_environmentManager.SetManager();
        m_forceManager = GetComponent<PlayerForceManager>();
        m_forceManager.SetManager();
        m_playUIManager = GetComponent<PlayUIManager>();
        m_playUIManager.SetManager();
    }

    private void Awake()
    {
        if (Inst != null) { Destroy(Inst.gameObject); }
        Inst = this;
        SetSubManagers();
    }
    private void Start()
    {
        StartPlay();
    }
}
