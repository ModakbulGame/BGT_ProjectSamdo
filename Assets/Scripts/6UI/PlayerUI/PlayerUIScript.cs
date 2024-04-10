using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIScript : MonoBehaviour
{
    private PlayerImgUIScript m_imgUI;                      // 플레이어 이미지 UI
    private PlayerInfoUIScript m_infoUI;                    // 플레이어 정보(아이템, 특성 등) UI
    private PlayerMaterialUIScript m_materialUI;            // 플레이어 재료 UI

    private bool Opened { get; set; }                       // 열린 적 있는지 (처음 열리는지 확인용)


    public void OpenUI()                                    // UI 열기
    {
        gameObject.SetActive(true);
        if(!Opened) { SetComps(); }
        m_imgUI.OpenUI();
        m_infoUI.OpenUI();
        m_materialUI.OpenUI();
    }

    public void UpdateInfoUI()
    {
        m_infoUI.UpdateUI();
    }

    public void UpdateMaterials()                           // 재화 업데이트
    {
        if(!Opened) { SetComps(); }
        m_materialUI.UpdateMaterials();
    }

    public void UpdatePlayerModelWeapon()
    {
        m_imgUI.updatePlayerWeapon(PlayManager.CurWeapon);
    }

    public void CloseUI() { GameManager.SetControlMode(EControlMode.THIRD_PERSON); gameObject.SetActive(false); }      // 닫기


    private void SetComps()
    {
        m_imgUI = GetComponentInChildren<PlayerImgUIScript>();
        m_infoUI = GetComponentInChildren<PlayerInfoUIScript>();
        m_materialUI = GetComponentInChildren<PlayerMaterialUIScript>();

        m_imgUI.SetComps();
        m_infoUI.SetParent(this);
        m_infoUI.SetComps();
        m_materialUI.SetComps();
        Opened = true;
    }

    private void Awake()
    {
        GameManager.UIControlInputs.ClosePlayerUI.started += delegate { CloseUI(); };
    }

    private void Start()
    {
        if(!Opened) { SetComps(); OpenUI(); }
    }
}
