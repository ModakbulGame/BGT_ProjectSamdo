using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItemBoxScript : MonoBehaviour
{
    private const int ElmCount = 24;
    public int ElmNum { get { return ElmCount; } }

    private AllItemElmScript[] m_elms;
    public RectTransform[] ElmTrans { get; private set; } = new RectTransform[ElmCount];


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
        foreach (AllItemElmScript elm in m_elms) { elm.SetComps(); elm.SetParent(this); }
        for(int i = 0; i < ElmCount; i++) { ElmTrans[i] = m_elms[i].GetComponent<RectTransform>(); }
    }
}
