using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItemBoxScript : MonoBehaviour
{
    private AllItemElmScript[] m_elms;

    private const int ElmCount = 24;


    public void UpdateUI()
    {
        InventoryElm[] inven = PlayManager.PlayerInventory;
        for (int i = 0; i<ElmCount; i++)
        {
            if (inven[i].IsEmpty) { m_elms[i].HideItem(); continue; }
            m_elms[i].SetItem(inven[i]);
        }
    }


    public void SetComps()
    {
        m_elms = GetComponentsInChildren<AllItemElmScript>();
        if (m_elms.Length != ElmCount) { Debug.LogError("아이템 UI 개수 다름"); }
        foreach (AllItemElmScript elm in m_elms) { elm.SetComps(); }
    }
}
