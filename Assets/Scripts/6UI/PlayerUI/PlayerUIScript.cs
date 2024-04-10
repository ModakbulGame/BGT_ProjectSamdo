using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIScript : MonoBehaviour
{
    private PlayerImgUIScript m_imgUI;                      // �÷��̾� �̹��� UI
    private PlayerInfoUIScript m_infoUI;                    // �÷��̾� ����(������, Ư�� ��) UI
    private PlayerMaterialUIScript m_materialUI;            // �÷��̾� ��� UI

    private bool Opened { get; set; }                       // ���� �� �ִ��� (ó�� �������� Ȯ�ο�)


    public void OpenUI()                                    // UI ����
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

    public void UpdateMaterials()                           // ��ȭ ������Ʈ
    {
        if(!Opened) { SetComps(); }
        m_materialUI.UpdateMaterials();
    }

    public void UpdatePlayerModelWeapon()
    {
        m_imgUI.updatePlayerWeapon(PlayManager.CurWeapon);
    }

    public void CloseUI() { GameManager.SetControlMode(EControlMode.THIRD_PERSON); gameObject.SetActive(false); }      // �ݱ�


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
