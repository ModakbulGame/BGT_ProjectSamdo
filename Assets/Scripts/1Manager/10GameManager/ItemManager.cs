using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

// ������ ���� enum -> ItemEnum�� ����

[Serializable]
public struct SItem             // ������ ����ü
{
    public EItemType Type;
    public int Idx;

    public readonly bool IsEmpty { get { return Idx == -1; } }
    public static SItem Empty { get { return new SItem(EItemType.LAST, -1); } }
    public SItem(EItemType _type, int _idx) { Type = _type; Idx = _idx; }
    public static bool operator==(SItem _item1, SItem _item2) { return (_item1.Type == _item2.Type && _item1.Idx == _item2.Idx); }
    public static bool operator!=(SItem _item1, SItem _item2) { return !(_item1 == _item2); }
    public readonly override bool Equals(object obj) { return obj is SItem item && Type == item.Type && Idx == item.Idx; }
    public readonly override int GetHashCode() { return HashCode.Combine(Type, Idx); }
}


[Serializable]
public struct DropInfo
{
    public List<SDropItem> Items;
    public int StatPoint;
    public int Soul;
    public int Purified;
    public DropInfo(List<SDropItem> _items, int _stat, int _soul, int _purified) 
    {
        Items = new();
        foreach (SDropItem item in _items) { Items.Add(item); }
        StatPoint = _stat; Soul = _soul; Purified = _purified;
    }
}

[Serializable]
public struct SDropItem         // ��� ������ ����ü
{
    public string ID;
    public float Prob;

    public SDropItem(string _id, float _prob) { ID = _id; Prob = _prob; }
}

public class ItemInfo
{
    public ItemScriptable ItemData { get; private set; }
    public string ItemID { get { return ItemData.ID; } }                    // ���� �ʵ�
    public string ItemName { get { return ItemData.ItemName; } }
    public EItemType ItemType { get; private set; }
    public string ItemDescription { get { return ItemData.Description; } }
    public SItem Item { get; private set; }
    public bool Obtained { get; private set; }

    public void ObtainItem() { Obtained = true; }

    public ItemInfo(ItemScriptable _scriptable, SItem _item)
    {
        ItemData = _scriptable;
        Item = _item;
        ItemType = _item.Type;
        Obtained = ItemType != EItemType.WEAPON;
    }
}

public class ItemManager : MonoBehaviour
{
    // ������ ������
    [SerializeField]
    private GameObject[] m_dropItemPrefab = new GameObject[(int)EItemType.LAST];            // ��� ������
    public GameObject GetDropItemPrefab(EItemType _item)
    {
        return PoolManager.GetObject(m_dropItemPrefab[(int)_item]);
    }

    [SerializeField]
    private GameObject[] m_throwItemPrefabs = new GameObject[(int)EThrowItemName.LAST];     // ��ô ������
    public GameObject GetThrowItemPrefab(EThrowItemName _item)
    {
        return PoolManager.GetObject(m_throwItemPrefabs[(int)_item]);
    }

    public GameObject[] ItemArray { get {                                                   // ��ü ������
            List<GameObject> list = new();
            list.AddRange(m_dropItemPrefab);
            list.AddRange(m_throwItemPrefabs);
            return list.ToArray(); } }


    // ������ ����
    private readonly Dictionary<SItem, ItemInfo> m_itemInfo = new();
    public ItemInfo GetItemInfo(SItem _item)
    {
        if(_item.IsEmpty) { Debug.LogError("�� ������"); return null; }
        return m_itemInfo[_item];
    }
    public ItemInfo GetItemInfo(string _id)
    {
        foreach (KeyValuePair<SItem, ItemInfo> kv in m_itemInfo)
        {
            if (kv.Value.ItemID == _id) { return kv.Value; }
        }
        return null;
    }

    public void ObtainWeapon(EWeaponName _weapon)       // ���� ȹ��
    {
        SItem weapon = new(EItemType.WEAPON, (int)_weapon);
        if (m_itemInfo[weapon].Obtained) { return; }
        m_itemInfo[weapon].ObtainItem();
    }




    // ������ �� ���� ��
    private readonly uint[] ItemCounts = new uint[(int)EItemType.LAST]
    { (uint)EWeaponName.LAST, (uint)EPatternName.LAST, (uint)EThrowItemName.LAST, (uint)EOtherItemName.LAST };

    public void SetManager()
    {
        for (int i = 0; i<(int)EItemType.LAST; i++)
        {
            EItemType type = (EItemType)i;
            uint cnt = ItemCounts[i];
            for (int j = 0; j<cnt; j++)                         // ������ ������ ���� �ٸ��� ���� �߰� ����
            {
                SItem item = new(type, j);
                ItemScriptable scriptable = GameManager.GetItemRawData(item);
                m_itemInfo[item] = new(scriptable, item);
            }
        }
    }
}
