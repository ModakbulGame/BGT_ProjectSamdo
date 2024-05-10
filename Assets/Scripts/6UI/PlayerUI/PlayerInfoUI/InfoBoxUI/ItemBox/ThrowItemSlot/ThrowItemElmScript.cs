using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThrowItemElmScript : MonoBehaviour
{
    private ThrowItemSlotScript m_parent;
    public void SetParent(ThrowItemSlotScript _parent) { m_parent = _parent; }

    public Transform MoveTrans { get { return m_parent.MoveTrans; } }

    private ThrowItemImgScript m_itemImg;

    public int CurIdx { get; private set; }
    private SItem CurItem { get; set; }

    public void SetItem(int _idx, EThrowItemName _item)
    {
        CurIdx = _idx;
        if (!m_itemImg.gameObject.activeSelf) { m_itemImg.gameObject.SetActive(true); }
        CurItem = new(EItemType.THROW, (int)_item);
        Sprite itemSprite = GameManager.GetItemSprite(CurItem);
        m_itemImg.SetImg(itemSprite);
    }

    public void HideItem()
    {
        m_itemImg.gameObject.SetActive(false);
    }

    public void ShowInfo()
    {
        if (CurItem.IsEmpty) { return; }
        m_parent.ShowInfo(CurItem);
    }


    public void ItemElmClick()
    {
        PlayManager.RemoveThrowItem(CurIdx);
        HideInfo();
    }


    public void HideInfo()
    {
        m_parent.HideInfo();
    }
    public void SetInfoPos(Vector2 _pos)
    {
        m_parent.SetInfoPos(_pos);
    }

    public void SetComps()
    {
        m_itemImg = GetComponentInChildren<ThrowItemImgScript>();
        m_itemImg.SetParent(this);
        m_itemImg.SetComps();
    }
}
