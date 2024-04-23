using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AllItemElmScript : MonoBehaviour
{
    private ItemBoxUIScript m_parent;
    public void SetParent(ItemBoxUIScript _parent) { m_parent = _parent; }
    public ItemBoxUIScript Box { get { return m_parent; } }

    private EventTrigger m_trigger;
    private Image m_itemImg;
    private TextMeshProUGUI m_itemNumTxt;

    public SItem CurItem { get; set; }

    public void SetItem(InventoryElm _item)
    {
        if(!m_itemImg.gameObject.activeSelf) { m_itemImg.gameObject.SetActive(true); }

        CurItem = _item.m_item;
        Sprite itemSprite = GameManager.GetItemSprite(CurItem);
        m_itemImg.sprite = itemSprite;

        m_itemNumTxt.text = _item.m_num.ToString();
    }
    
    public void HideItem()
    {
        m_itemImg.gameObject.SetActive(false);
        m_itemNumTxt.text = "";
    }


    private void ItemElmClick(PointerEventData _data)
    {
        switch (CurItem.Type)
        {
            case EItemType.THROW:
                if(_data.button == PointerEventData.InputButton.Right)
                    PlayManager.AddThrowItem((EThrowItemName)CurItem.Idx);
                break;
        }
    }



    private void SetEvents()
    {
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerClick, ItemElmClick);
    }

    public void SetComps()
    {
        m_trigger = gameObject.AddComponent<EventTrigger>();
        m_itemImg = GetComponentsInChildren<Image>()[1];
        m_itemNumTxt = GetComponentInChildren<TextMeshProUGUI>();
        SetEvents();
    }
}
