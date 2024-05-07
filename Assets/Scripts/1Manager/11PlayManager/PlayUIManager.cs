using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayUIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas[] m_canvases = new Canvas[(int)ECanvasType.LAST];
    public Canvas GetCanvas(ECanvasType _canvas) { return m_canvases[(int)_canvas]; }

    [SerializeField]
    private PlayerUIScript m_playerUI;                      // 플레이어 정보 UI (탭 누르면 나오는거)
    public void OpenPlayerUI() { m_playerUI.OpenUI(); }     // 열기
    public void ClosePlayerUI() { m_playerUI.CloseUI(); }   // 닫기
    public void UpdateMaterials() { if (m_playerUI.gameObject.activeSelf) m_playerUI.UpdateMaterials(); }   // 재료 업데이트
    public void UpdateInfoUI() { if (m_playerUI.gameObject.activeSelf) m_playerUI.UpdateInfoUI(); }         // 정보 업데이트

    private Vector3 MapLB { get { return PlayManager.MapLB; } }
    private Vector3 MapRT { get { return PlayManager.MapRT; } }
    private float MapWidth { get { return PlayManager.MapWidth; } }
    private float MapHeight { get { return PlayManager.MapHeight; } }

    private Vector2 m_mapArea;

    public Vector2 NormalizeLocation(Transform _obj)
    {
        Vector2 originalPos = new Vector2(Vector3.Distance(new Vector3(MapLB.x, 0f, 0f), new Vector3(_obj.position.x, 0f, 0f)),
                Vector3.Distance(new Vector3(0f, 0f, MapRT.z), new Vector3(0f, 0f, _obj.position.z)));
        Vector2 normalPos = new Vector2(originalPos.x / m_mapArea.x, originalPos.y / m_mapArea.y);

        return normalPos;
    }

    [SerializeField]
    private NPCDialogueScript m_dialogueUI;                 // NPC
    public void OpenDialogueUI(NPCScript _npc) { m_dialogueUI.OpenUI(_npc); }
    public void CloseDialogueUI() { m_dialogueUI.CloseUI(); }


    [SerializeField]
    private OasisUIScript m_oasisUI;                        // 오아시스 UI
    public void OpenOasisUI(OasisNPC _npc) { m_oasisUI.OpenUI(_npc); }
    public void CloseOasisUI() { m_oasisUI.CloseUI(); }

    [SerializeField]
    private MinimapScript m_miniMap;
    public void SetMinimapScale(float _scale) { m_miniMap.SetScale(_scale); }


    [SerializeField]
    private MapUIScript m_mapUI;                            // 맵 UI
    public void ToggleMapUI() { m_mapUI.ToggleMapUI(); }


    [SerializeField]
    private GameObject m_mainCanvas;                        // 메인 캔버스


    private PlayerHPBarScript m_hpBar;                      // HP 바
    public void SetMaxHP(float _hp) { m_hpBar.SetMaxHP(_hp); }
    public void SetCurHP(float _hp) { m_hpBar.SetCurHP(_hp); }

    private SkillSlotUIScript m_skillSlot;                  // 스킬 슬롯
    public void UpdateSkillSlot() { m_skillSlot.UpdateUI(); }
    public void UseSkillSlot(int _idx, float _cooltime) { m_skillSlot.UseSkill(_idx, _cooltime); }

    private EquipSlotUIScript m_equipSlot;
    public void UpdateEquipSlot() { m_equipSlot.UpdateUI(); }
    public void UpdateThrowItemSlot() { m_equipSlot.UpdateThrowItemImg(); }
    public void UpdateHealItemSlot() { m_equipSlot.UpdateHealItemImg(); }


    private AimUIScript m_aimUI;                            // 플레이어 에임 UI
    public void SetStaminaRate(float _rate) { m_aimUI.SetStaminaRate(_rate); }
    public void SetLightRate(float _rate) { m_aimUI.SetLightRate(_rate); }


    [SerializeField]
    private PlayerSkillAimScript m_skillAimUI;
    public void ShowSkillAim(Vector3 _pos, float _radius, float _range)       // 스킬 에임 보이기
    {
        m_skillAimUI.ShowDrawer(_radius);
        TraceSkillAim(_pos, _range);
    }
    public Vector3 TraceSkillAim(Vector3 _pos, float _range)
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
        m_skillAimUI.TraceAim(pos);
        return pos;
    }
    public void HideSkillAim()                                  // 스킬 에임 숨기기
    {
        m_skillAimUI.HideDrawer();
    }


    [SerializeField]
    private PlayerThrowLineRenderer m_throwLineUI;          // 플레이어 던지기 궤적 UI
    public void DrawThrowLine(Vector3 _force, float _mass, Vector3 _start) { m_throwLineUI.DrawThrowLine(_force, _mass, _start); }      // 던지기 궤적 그리기
    public void HideThrowLine() { m_throwLineUI.HideThrowLine(); }              // 던지기 궤적 숨기기


    [SerializeField]
    // private BlindUIScript m_blindUI;
    public void ShowBlindMark()
    {
        // 실명 발동
    }
    public void HideBlindMark()
    {
        // 실명 해제
    }







    public void SetManager()
    {
        m_hpBar = m_mainCanvas.GetComponentInChildren<PlayerHPBarScript>();
        m_hpBar.SetComps();
        m_skillSlot = m_mainCanvas.GetComponentInChildren<SkillSlotUIScript>();
        m_skillSlot.SetComps();
        m_aimUI = m_mainCanvas.GetComponentInChildren<AimUIScript>();
        m_equipSlot = m_mainCanvas.GetComponentInChildren<EquipSlotUIScript>();
        //m_mapArea = new Vector2(MapWidth, MapHeight);

        UpdateSkillSlot();
        UpdateEquipSlot();

    }
}
