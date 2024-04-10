using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowItemElmScript : MonoBehaviour
{
    private Image m_itemImg;


    public void SetItem(EThrowItemName _item)
    {
        if (!m_itemImg.gameObject.activeSelf) { m_itemImg.gameObject.SetActive(true); }
        Sprite itemSprite = GameManager.GetItemSprite(new(EItemType.THROW, (int)_item));
        m_itemImg.sprite = itemSprite;
    }

    public void HideItem()
    {
        m_itemImg.gameObject.SetActive(false);
    }



    public void SetComps()
    {
        m_itemImg = GetComponentsInChildren<Image>()[1];
    }
}
