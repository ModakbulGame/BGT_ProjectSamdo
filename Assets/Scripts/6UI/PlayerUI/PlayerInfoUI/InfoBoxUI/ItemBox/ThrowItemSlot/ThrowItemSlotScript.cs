using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowItemSlotScript : MonoBehaviour
{
    private ItemBoxUIScript m_parent;
    public void SetParent(ItemBoxUIScript _parent) { m_parent = _parent; }

    public Transform MoveTrans { get { return m_parent.transform; } }

    private ThrowItemElmScript[] m_elms;

    public void UpdateUI()
    {
        List<EThrowItemName> throwItemList = PlayManager.ThrowItemList;
        for (int i = 0; i<ValueDefine.MAX_THROW_ITEM; i++)
        {
            if (throwItemList.Count <= i) { m_elms[i].HideItem(); continue; }
            m_elms[i].SetItem(i, throwItemList[i]);
        }
    }

    public void ShowInfo(SItem _item)
    {
        m_parent.ShowItemInfoUI(_item);
    }
    public void HideInfo()
    {
        m_parent.HideItemInfoUI();
    }
    public void SetInfoPos(Vector2 _pos)
    {
        m_parent.SetItemInfoUIPos(_pos);
    }


    public void SetComps()
    {
        m_elms = GetComponentsInChildren<ThrowItemElmScript>();
        if(m_elms.Length != ValueDefine.MAX_THROW_ITEM) { Debug.LogError("던지기 아이템 UI 개수 다름"); }
        foreach (ThrowItemElmScript elm in m_elms) { elm.SetParent(this); elm.SetComps(); }
    }
}
