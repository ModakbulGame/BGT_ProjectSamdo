using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemScript : PooledItem, IInteractable
{
    [SerializeField]
    private SItem m_dropItem;
    [SerializeField]
    private int m_itemNum;

    public SItem DropItem { get { return m_dropItem; } }
    public int ItemNum { get { return m_itemNum; } }

    public void SetDropItem(string _id) { SItem item = GameManager.GetItemInfo(_id).Item; SetDropItem(item); }
    public void SetDropItem(SItem _item) { SetDropItem(_item, 1); }
    public void SetDropItem(SItem _item, int _num) { m_dropItem = _item; m_itemNum = _num; }

    // ------------- ¿Œ≈Õ∆‰¿ÃΩ∫ ±∏«ˆ ------------------ //
    public EInteractType InteractType { get { return EInteractType.ITEM; } }

    public float UIOffset { get { return 1.5f; } }
    public virtual string InfoTxt { get { return "»πµÊ"; } }

    public virtual void StartInteract()
    {
        GetItem();
    }

    public virtual void StopInteract()
    {
       
    }


    public override void ReleaseToPool()
    {
        base.ReleaseToPool();
        m_dropItem = SItem.Empty;
    }


    public void GetItem()
    {
        PlayManager.AddInventoryItem(DropItem, 1);
        ReleaseToPool();
    }
}
