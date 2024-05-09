using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThrowItemElmScript : MonoBehaviour
{
    private ThrowItemSlotScript m_parent;
    public void SetParent(ThrowItemSlotScript _parent) { m_parent = _parent; }

    private Image m_itemImg;
    private ThrowItemImgScript m_img;

    private SItem CurItem { get; set; }

    public void SetItem(EThrowItemName _item)
    {
        if (!m_itemImg.gameObject.activeSelf) { m_itemImg.gameObject.SetActive(true); }
        CurItem = new(EItemType.THROW, (int)_item);
        Sprite itemSprite = GameManager.GetItemSprite(CurItem);
        m_itemImg.sprite = itemSprite;
    }

    public void HideItem()
    {
        m_itemImg.gameObject.SetActive(false);
    }

    public void ShowInfo()
    {
        if(CurItem.IsEmpty) { return; }
        m_parent.ShowInfo(CurItem);
    }

    public void HideInfo()
    {
        m_parent.HideInfo();
    }
    public void ActivateMark(Transform _imgTransform)
    {
        m_parent.ActivateMark(_imgTransform);
    }
    public void SetInfoPos(Vector2 _pos)
    {
        m_parent.SetInfoPos(_pos);
    }

    public void SetComps()
    {
        m_itemImg = GetComponentsInChildren<Image>()[1];
        m_img = GetComponentInChildren<ThrowItemImgScript>();
        m_img.SetParent(this);
        m_img.SetComps();
    }
}
