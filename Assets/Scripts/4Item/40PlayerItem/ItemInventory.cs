using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryElm       // �� �κ��丮 ĭ�� ����
{
    public SItem m_item;                                                            // ������ �̸� idx
    public int m_num;                                                               // ������ ����
    public bool IsEmpty { get { return m_item.IsEmpty; } }                          // �������
    public void SetItem(SItem _item, int _num) { m_item = _item; m_num = _num; }    // �κ��丮 ����
    public void UseItem(int _num) { if(m_num >= _num) { m_num -= _num; } }          // ������ ���
    public void EmptyInventory() { m_item = SItem.Empty; m_num = 0; }               // ����
    public InventoryElm() { EmptyInventory(); }                                     // �� ������
}

public class ItemInventory
{
    private readonly InventoryElm[] m_inventory = new InventoryElm[ValueDefine.MAX_INVENTORY];        // �κ��丮 �迭
    public InventoryElm[] Inventory { get { return m_inventory; } }

    private int EmptyIdx { get { for (int i = 0; i<ValueDefine.MAX_INVENTORY; i++) { if (m_inventory[i].IsEmpty) return i; } return -1; } }     // �� �κ��丮 idx


    public void AddItem(SItem _item, int _num)                  // �� �κ��丮�� ������ �߰�
    {
        if(EmptyIdx == -1) { Debug.LogError("�� �κ��丮 ����"); return; }
        SetItem(EmptyIdx, _item, _num);
    }
    public void SetItem(int _idx, SItem _item, int _num)        // Idx��° �κ��丮�� ������ �Ҵ�
    {
        m_inventory[_idx].SetItem(_item, _num);
    }
    public bool ChkNUseItem(SItem _item, int _num)              // ������ Ȯ�� �� ���
    {
        for (int i = 0; i<Inventory.Length; i++)
        {
            if (_item == Inventory[i].m_item)
            {
                if (Inventory[i].m_num >= _num)
                {
                    Inventory[i].UseItem(_num);
                    return true;
                }
                return false;
            }
        }
        return false;
    }
    public void RemoveItem(int _idx)                            // Idx��° �κ��丮 ������ ����
    {
        m_inventory[_idx].EmptyInventory();
    }


    public ItemInventory() { for(int i=0;i<ValueDefine.MAX_INVENTORY; i++) { m_inventory[i] = new(); } }
}
