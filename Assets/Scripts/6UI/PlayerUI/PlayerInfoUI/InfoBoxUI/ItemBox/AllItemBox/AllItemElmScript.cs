using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AllItemElmScript : MonoBehaviour
{
    private AllItemBoxScript m_parent;
    public AllItemBoxScript Box { get { return m_parent; } }
    public void SetParent(AllItemBoxScript _parent) { m_parent = _parent; }

    public Transform MoveTrans { get { return m_parent.MoveTrans; } }


    private AllItemImgScript m_itemImg;
    private TextMeshProUGUI m_itemNumTxt;

    public int CurIdx { get; private set; }
    public SItem CurItem { get; set; }

    public void SetItem(int _idx, InventoryElm _item)
    {
        CurIdx = _idx;

        if (!m_itemImg.gameObject.activeSelf) { m_itemImg.gameObject.SetActive(true); }

        CurItem = _item.Item;
        Sprite itemSprite = GameManager.GetItemSprite(CurItem);
        m_itemImg.SetImg(itemSprite);

        m_itemNumTxt.text = _item.Num.ToString();
    }

    public void HideItem()
    {
        m_itemImg.gameObject.SetActive(false);
        m_itemNumTxt.text = "";
    }


    public void ItemElmClick()
    {
        switch (CurItem.Type)
        {
            case EItemType.THROW:
                PlayManager.AddThrowItem((EThrowItemName)CurItem.Idx);
                HideInfo();
                break;
        }
    }


    public int CheckThrowItemPos(RectTransform _trans)
    {
        return m_parent.CheckThrowItemPos(_trans);
    }
    public int CheckAllItemPos(RectTransform _trans)
    {
        return m_parent.CheckAllItemPos(_trans);
    }


    public void ShowInfo()
    {
        if (CurItem.IsEmpty) { return; }
        m_parent.ShowInfo(CurItem);
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
        m_itemNumTxt = GetComponentInChildren<TextMeshProUGUI>();
        m_itemImg = GetComponentInChildren<AllItemImgScript>();
        m_itemImg.SetParent(this);
        m_itemImg.SetComps();
    }
}
