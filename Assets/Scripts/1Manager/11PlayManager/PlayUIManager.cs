using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayUIManager : MonoBehaviour
{
    // 캔버스
    [SerializeField]
    private Canvas[] m_canvases = new Canvas[(int)ECanvasType.LAST];
    public Canvas GetCanvas(ECanvasType _canvas) { return m_canvases[(int)_canvas]; }

    private Canvas MainCanvas { get { return m_canvases[(int)ECanvasType.MAIN]; } }
    private Canvas OptionCanvas { get { return m_canvases[(int)ECanvasType.OPTION]; } }



    // 온 오프 UI
    [SerializeField]
    private OptionUIScript m_optionUI;
    public bool IsOptionOpen { get { return m_optionUI.gameObject.activeSelf; } }
    public void ToggleOptionUI(bool _on) { if (_on) { m_optionUI.OpenUI(); } else { m_optionUI.CloseUI(); } }


    [SerializeField]
    private PlayerUIScript m_playerUI;                      // 플레이어 정보 UI (탭 누르면 나오는거)
    public bool IsPlayerUIOpen { get { return m_playerUI.gameObject.activeSelf; } }
    public void TogglePlayerUI(bool _on) { if (_on) { m_playerUI.OpenUI(); } else { m_playerUI.CloseUI(); } } // 여닫기
    public void UpdateMaterials() { if (m_playerUI.gameObject.activeSelf) m_playerUI.UpdateMaterials(); }   // 재료 업데이트
    public void UpdateInfoUI() { if (m_playerUI.gameObject.activeSelf) m_playerUI.UpdateInfoUI(); }         // 정보 업데이트


    [SerializeField]
    private MapUIScript m_mapUI;                            // 맵 UI
    public void ToggleMapUI() { m_mapUI.ToggleMapUI(); }


    [SerializeField]
    private QuestUIScript m_questUI;                        // 퀘스트 UI
    public bool IsQuestUIOpen { get { return m_questUI.gameObject.activeSelf; } }
    public void ToggleQuestUI(bool _on) { if (_on) { m_questUI.OpenUI(); } else { m_questUI.CloseUI(); } }


    [SerializeField]
    private OasisUIScript m_oasisUI;                        // 오아시스 UI
    public void OpenOasisUI(OasisNPC _npc) { m_oasisUI.OpenUI(_npc); }
    public void CloseOasisUI() { m_oasisUI.CloseUI(); }



    // 메인 캔버스 상시 UI
    private PlayerHPBarScript m_hpBar;                      // HP 바
    public void SetMaxHP(float _hp) { m_hpBar.SetMaxHP(_hp); }
    public void SetCurHP(float _hp) { m_hpBar.SetCurHP(_hp); }


    private PowerSlotUIScript m_powerSlot;                  // 스킬 슬롯
    public void UpdatePowerSlot() { m_powerSlot.UpdateUI(); }
    public void UsePowerSlot(int _idx, float _cooltime) { m_powerSlot.UsePower(_idx, _cooltime); }


    private EquipSlotUIScript m_equipSlot;
    public void UpdateThrowItemSlot() { m_equipSlot.UpdateThrowItemImg(); }
    public void UpdateHealItemSlot() { m_equipSlot.UpdateHealItemImg(); }


    private QuestSideBarScript m_questSideBar;
    public void UpdateQuestSideBar() { m_questSideBar.UpdateUI(); }


    private MinimapScript m_miniMap;
    public void SetMinimapScale(float _scale) { m_miniMap.SetScale(_scale); }


    private AimUIScript m_aimUI;                            // 플레이어 에임 UI
    public void SetStaminaRate(float _rate) { m_aimUI.SetStaminaRate(_rate); }
    public void SetLightRate(float _rate) { m_aimUI.SetLightRate(_rate); }
    public void SetLightState(bool _on) { m_aimUI.SetLightState(_on); }
    public void ShowRaycastAim() { m_aimUI.ShowAimUI(); }
    public void SetRaycastAimState(bool _on) { m_aimUI.SetAimUI(_on); }
    public void HideRaycastAim() { m_aimUI.HideAimUI(); }


    // 인게임 UI
    private IngameAlarmUIScript m_ingameAlarm;
    public void AddAlarm(string _alarm) { m_ingameAlarm.AddAlarm(_alarm); }



    private PlayerPowerAimScript m_powerAimUI;
    public void ShowPowerAim(Vector3 _pos, float _radius, float _range)       // 스킬 에임 보이기
    {
        m_powerAimUI.ShowDrawer(_radius);
        TracePowerAim(_pos, _range);
    }
    public Vector3 TracePowerAim(Vector3 _pos, float _range)
    {
        Vector3 pos = _pos;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit[] hits = Physics.RaycastAll(ray, 100, ValueDefine.GROUND_LAYER);
        foreach (RaycastHit hit in hits)
        {
            GameObject obj = hit.transform.gameObject;
            if (FunctionDefine.IsTerrain(obj))
            {
                pos = hit.point;
                if (Vector3.Distance(_pos, pos) > _range)
                {
                    Vector3 dir = _range * (pos-_pos).normalized;
                    pos = _pos + dir;
                }
            }
        }
        m_powerAimUI.TraceAim(pos);
        return pos;
    }
    public void HidePowerAim()                                  // 스킬 에임 숨기기
    {
        m_powerAimUI.HideDrawer();
    }


    private PlayerThrowLineRenderer m_throwLineUI;          // 플레이어 던지기 궤적 UI
    public void DrawThrowLine(Vector3 _force, float _mass, Vector3 _start) { m_throwLineUI.DrawThrowLine(_force, _mass, _start); }      // 던지기 궤적 그리기
    public void HideThrowLine() { m_throwLineUI.HideThrowLine(); }              // 던지기 궤적 숨기기


    [SerializeField]
    private InteractInfoUI m_interactInfoUI;
    public void ShowInteractInfo(string _info) { m_interactInfoUI.ShowInteractInfo(_info); }
    public void HideInteractInfo() { m_interactInfoUI.HideInteractInfo(); }


    private BlackoutImageScript m_blackoutUI;
    public void StartBlackout() { m_blackoutUI.ShowImg(); }
    public void EndBlackout() { m_blackoutUI.HideImg(); }


    private SpitPoisonUIScript m_spitUI;
    public void ShowBlindMark()
    {
        m_spitUI.ShowBlind();
    }
    public void HideBlindMark()
    {
        m_spitUI.HideBlind();
    }


    // NPC
    [SerializeField]
    private NPCDialogueUI m_dialogueUI;                 // NPC UI
    public bool IsDialogueUIOpend { get { return m_dialogueUI.gameObject.activeSelf; } }
    public void OpenDialogueUI(NPCScript _npc, int _idx) { m_dialogueUI.OpenUI(_npc, _idx); }

    [SerializeField]
    // private SlateUI m_slateUI;
    public void OpenSlateUI(SlateNPC _slate) { /*m_slateUI.OpenUI(_slate);*/ }

    [SerializeField]
    private QuestAcceptUIScript m_questAcceptUI;        // NPC 대화 끝에 나오는 퀘스트 창
    public void ShowNPCQuestUI(EQuestName _quest, bool _isStart, FPointer _confirm) { m_questAcceptUI.ShowNPCQuestUI(_quest, _isStart, _confirm); }



    // 맵 관련
    private Vector3 MapLB { get { return PlayManager.MapLB; } }
    private Vector3 MapRT { get { return PlayManager.MapRT; } }

    private Vector2 MapArea { get { return new(MapRT.x - MapLB.x, MapRT.y - MapLB.y); } }

    public Vector2 NormalizeLocation(Transform _obj)
    {
        Vector2 originalPos = new(Vector3.Distance(new Vector3(MapLB.x, 0f, 0f), new Vector3(_obj.position.x, 0f, 0f)),
                Vector3.Distance(new Vector3(0f, 0f, MapRT.z), new Vector3(0f, 0f, _obj.position.z)));
        Vector2 normalPos = new(originalPos.x / MapArea.x, originalPos.y / MapArea.y);

        return normalPos;
    }






    public void SetManager()
    {
        m_hpBar = MainCanvas.GetComponentInChildren<PlayerHPBarScript>();
        m_hpBar.SetComps();
        m_powerSlot = MainCanvas.GetComponentInChildren<PowerSlotUIScript>();
        m_powerSlot.SetComps();
        m_miniMap = MainCanvas.GetComponentInChildren<MinimapScript>();
        m_questSideBar = MainCanvas.GetComponentInChildren<QuestSideBarScript>();
        m_aimUI = MainCanvas.GetComponentInChildren<AimUIScript>();
        m_equipSlot = MainCanvas.GetComponentInChildren<EquipSlotUIScript>();

        m_ingameAlarm = MainCanvas.GetComponentInChildren<IngameAlarmUIScript>();
        m_powerAimUI = GetComponentInChildren<PlayerPowerAimScript>();
        m_throwLineUI = GetComponentInChildren<PlayerThrowLineRenderer>();

        m_blackoutUI = OptionCanvas.GetComponentInChildren<BlackoutImageScript>();
        m_spitUI = MainCanvas.GetComponentInChildren<SpitPoisonUIScript>();

        UpdatePowerSlot();
        UpdateThrowItemSlot();
        UpdateHealItemSlot();
    }
}
