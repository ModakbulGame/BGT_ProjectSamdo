using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItemSlotScript : MonoBehaviour
{
    private ThrowItemElmScript[] m_elms;


    public void UpdateUI()
    {
        List<EThrowItemName> throwItemList = PlayManager.ThrowItemList;
        for (int i = 0; i<ValueDefine.MAX_THROW_ITEM; i++)
        {
            if (throwItemList.Count <= i) { m_elms[i].HideItem(); continue; }
            m_elms[i].SetItem(throwItemList[i]);
        }
    }


    public void SetComps()
    {
        m_elms = GetComponentsInChildren<ThrowItemElmScript>();
        if(m_elms.Length != ValueDefine.MAX_THROW_ITEM) { Debug.LogError("던지기 아이템 UI 개수 다름"); }
        foreach(ThrowItemElmScript elm in m_elms) { elm.SetComps(); }
    }
}
