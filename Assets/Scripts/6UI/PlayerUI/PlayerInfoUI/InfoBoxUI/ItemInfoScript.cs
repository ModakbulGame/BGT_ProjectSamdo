using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemInfoScript : MouserOverScript
{
    // 일단은 무기로만 하는 거로
    private Image m_infoUI;
    [SerializeField]
    private Image m_rootUI;

    private TextMeshPro m_itemNameTxt;
    private TextMeshPro m_descriptionTxt;

    private WeaponBoxElmScript[] m_elms;
    private EWeaponName ElmWeapon { get; set; }     // 할당된 무기

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        m_infoUI.gameObject.SetActive(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        m_infoUI.gameObject.SetActive(false);
    }

    public void PlaceToFront()  
    {
        m_infoUI.transform.SetParent(m_rootUI.transform, false);
        m_infoUI.transform.SetAsLastSibling();
    }

    public void SetItemInfo(int _weapon)          // 정보 설정
    {
        EWeaponName weapon = (EWeaponName)_weapon;
        ElmWeapon = weapon;

        SItem item = new(EItemType.WEAPON, _weapon);

        ItemInfo info = PlayManager.GetWeaponInfo(weapon);

        m_itemNameTxt.text = info.ItemName;
        m_descriptionTxt.text = info.ItemDescription;
    }

    private void Update()
    {

    }

    public override void SetComps()
    {
        m_infoUI = GetComponentInChildren<Image>();
        Debug.Log(m_infoUI.name);
    }

    public override void Awake()
    {
        SetComps();
        PlaceToFront();
    }
}
