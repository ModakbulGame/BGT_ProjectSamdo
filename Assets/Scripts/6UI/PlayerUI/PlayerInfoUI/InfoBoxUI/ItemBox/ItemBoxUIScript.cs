using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxUIScript : PlayerInfoBoxScript
{
    private ThrowItemSlotScript m_throwItemSlot;
    private AllItemBoxScript m_allItemBox;


    public override void InitUI()
    {
        UpdateUI();
    }

    public override void UpdateUI()
    {
        m_throwItemSlot.UpdateUI();
        m_allItemBox.UpdateUI();
    }


    public override void SetComps()
    {
        m_throwItemSlot = GetComponentInChildren<ThrowItemSlotScript>();
        m_throwItemSlot.SetComps();
        m_allItemBox = GetComponentInChildren<AllItemBoxScript>();
        m_allItemBox.SetComps();
    }
}
