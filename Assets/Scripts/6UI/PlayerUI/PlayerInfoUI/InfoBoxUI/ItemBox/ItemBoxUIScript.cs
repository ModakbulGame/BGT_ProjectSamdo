using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxUIScript : PlayerInfoBoxScript
{
    private ThrowItemSlotScript m_throwItemSlot;
    private AllItemBoxScript m_allItemBox;
    [SerializeField]
    private Image m_curItemMark;

    public override void InitUI()
    {
        UpdateUI();
    }

    public override void UpdateUI()
    {
        m_throwItemSlot.UpdateUI();
        m_allItemBox.UpdateUI();
    }

    public void ShowItemInfoUI(SItem _item)
    {
        m_parent.ShowItemInfoUI(_item);
    }
    public void HideItemInfoUI()
    {
        m_parent.HideItemInfoUI();
    }
    public void ActivateMark()
    {
        m_curItemMark.rectTransform.anchoredPosition = Vector2.zero; 
        m_curItemMark.gameObject.SetActive(true);
    }
    public void SetItemInfoUIPos(Vector2 _pos)
    {
        m_parent.SetItemInfoUIPos(_pos);
    }


    public override void SetComps()
    {
        m_throwItemSlot = GetComponentInChildren<ThrowItemSlotScript>();
        m_throwItemSlot.SetParent(this);
        m_throwItemSlot.SetComps();
        m_allItemBox = GetComponentInChildren<AllItemBoxScript>();
        m_allItemBox.SetParent(this);
        m_allItemBox.SetComps();
        
       // m_curItemMark.gameObject.SetActive(false);
    }
}
