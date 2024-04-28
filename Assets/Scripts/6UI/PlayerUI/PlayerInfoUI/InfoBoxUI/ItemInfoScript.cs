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
    // 일단은 무기로만 하는 거로
    [SerializeField]
    private GameObject m_infoUIPrefab;
    private GameObject m_infoUI;

    private Transform m_weaponElm;
    private GridLayoutGroup m_layoutGroup;

    private TextMeshProUGUI[] m_weaponInfo;

    private WeaponBoxElmScript m_parent;
    public void SetParent(WeaponBoxElmScript _parent) { m_parent = _parent; }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        m_infoUI = Instantiate(m_infoUIPrefab, Vector3.zero, Quaternion.identity, m_weaponElm.parent);
        // PlaceToFront();
        SetInfo();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        // Destroy(m_infoUI);
    }

    public void SetInfo()
    {
        m_weaponInfo = m_infoUI.GetComponentsInChildren<TextMeshProUGUI>();

        m_weaponInfo[0].text = m_parent.WeaponNameTxt.text;

        Debug.Log(m_weaponInfo[0].text);
    }

    public void PlaceToFront()  
    {
        m_infoUI.transform.SetAsLastSibling();
        m_infoUI.transform.SetParent(m_weaponElm.parent, false);
    }

    private void Update()
    {
    
    }

    public override void SetComps()
    {
        m_weaponElm = transform.parent.parent;
        m_layoutGroup = m_weaponElm.GetComponent<GridLayoutGroup>();
    }

    public override void Awake()
    {
        SetComps();
    }
}
