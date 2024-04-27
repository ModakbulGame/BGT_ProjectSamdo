using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using MalbersAnimations.Utilities;
using System;

public class ItemInfoScript : MouserOverScript
{
    // �ϴ��� ����θ� �ϴ� �ŷ�
    private Image m_infoUI;
    private Vector2 m_originalPos;
    private Transform m_weaponElm;

    private TextMeshProUGUI m_weaponNameTxt;          // ���� �̸�
    private FRange m_attack;                          // ���� ���ݷ�
    private FRange m_magic;                           // ���� ���ݷ�
    private TextMeshProUGUI m_weaponDescriptionTxt;   // ����

    private WeaponBoxElmScript m_parent;
    public void SetParent(WeaponBoxElmScript _parent) { m_parent = _parent; }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        m_infoUI.gameObject.SetActive(true);
        SetInfo();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        m_infoUI.gameObject.SetActive(false);
    }

    public void SetInfo()
    {
        m_weaponNameTxt = GetComponentInChildren<TextMeshProUGUI>();

        m_weaponNameTxt.text = m_parent.WeaponNameTxt.text;
        Debug.Log(m_weaponNameTxt.text);
    }

    public void PlaceToFront()  
    {
        m_infoUI.transform.SetParent(m_weaponElm.parent, false);
        m_infoUI.transform.SetAsLastSibling();
        m_infoUI.transform.localPosition = m_originalPos;
    }

    private void Update()
    {

    }

    public override void SetComps()
    {
        m_infoUI = transform.GetChild(0).GetComponent<Image>();
        m_originalPos = m_infoUI.transform.localPosition;
        m_weaponElm = transform.parent.parent;
    }

    public override void Awake()
    {
        SetComps();
        // PlaceToFront();
    }
}
