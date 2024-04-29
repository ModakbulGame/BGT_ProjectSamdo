using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 아이템 관련 enum -> ItemEnum에 있음

[Serializable]
public struct SItem             // 아이템 구조체
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
public struct SDropItem         // 드랍 아이템 구조체
{
    public string ID;
    public float Prob;

    public SDropItem(string _id, float _prob) { ID = _id; Prob = _prob; }
}

public class ItemInfo
{
    public ItemScriptable ItemData { get; private set; }
    public string ItemID { get { return ItemData.ID; } }                    // 공통 필드
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
        Obtained = false;
    }
}

public class ItemManager : MonoBehaviour
{
    private readonly Dictionary<SItem, ItemInfo> m_itemInfo = new();
    public ItemInfo GetItemInfo(SItem _item)
    {
        if(_item.IsEmpty) { Debug.LogError("빈 아이템"); return null; }
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

    // 아이템 프리펍
    [SerializeField]
    private GameObject[] m_weaponPrefabs = new GameObject[(int)EWeaponName.LAST];
    [SerializeField]
    private GameObject[] m_throwItemPrefabs = new GameObject[(int)EThrowItemName.LAST];

    public GameObject GetWeaponPrefab(EWeaponName _weapon)
    {
        return m_weaponPrefabs[(int)_weapon];
    }
    public GameObject GetThrowItemPrefab(EThrowItemName _item)
    {
        return m_throwItemPrefabs[(int)_item];
    }


    // 드랍 아이템
    [SerializeField]
    private GameObject[] m_dropItemPrefab = new GameObject[(int)EItemType.LAST];
    public GameObject GetDropItemPrefab(EItemType _item) { return m_dropItemPrefab[(int)_item]; }


    // 영혼
    private int m_soulNum = 0;
    public int SoulNum { get { return m_soulNum; } }
    public void AddSoul(int _num) 
    { 
        m_soulNum += _num;
        GameManager.CreateSideTextAlarm($"영혼 {_num}개 획득!");
        PlayManager.UpdateMaterials();
    }
    public void UseSoul(int _num)
    {
        if (m_soulNum < _num) { Debug.LogError("보유 영혼보다 많은 개수 사용"); return; }
        m_soulNum -= _num;
        PlayManager.UpdateMaterials();
    }
    

    // 성불 영혼
    private int m_purifiedNum = 0;
    public int PurifiedNum { get { return m_purifiedNum; } }
    public void AddPurified(int _num) 
    {
        m_purifiedNum += _num;
        GameManager.CreateSideTextAlarm($"성불 영혼 {_num}개 획득!");
        PlayManager.UpdateMaterials();
    }
    public void UsePurified(int _num) 
    {
        if(m_purifiedNum < _num) { Debug.LogError("보유 성불 영혼보다 많은 개수 사용"); return; }
        m_purifiedNum -= _num;
        PlayManager.UpdateMaterials();
    }


    // 문양
    private readonly int[] m_patternNum = new int[(int)EProperty.LAST] { 0, 0, 0 };
    public int[] PatternNum { get { return m_patternNum; } }
    public void AddPattern(EProperty _type, int _num)
    {
        m_patternNum[(int)_type] += _num;
        GameManager.CreateSideTextAlarm($"문양 {_num}개 획득!");
        PlayManager.UpdateMaterials();
    }
    public void UsePattern(EProperty _type, int _num)
    { 
        if (m_patternNum[(int)_type] < _num) { Debug.LogError("보유 문양보다 많은 개수 사용"); return; }
        m_patternNum[(int)_type] -= _num;
        PlayManager.UpdateMaterials();
    }


    // 장비 인벤토리
    private readonly bool[] m_weaponObtained = new bool[(int)EWeaponName.LAST];         // 무기 획득 여부
    public EWeaponName CurWeapon { get; private set; } = EWeaponName.BASIC_SWORD;    // 장착 중인 무기
    public void SetCurWeapon(EWeaponName _weapon)               // 무기 설정
    {
        CurWeapon = _weapon;
    }
    public void EquipWeapon(EWeaponName _weapon)                // 무기 장착
    {
        if (!m_weaponObtained[(int)_weapon]) { Debug.LogError("무기 미습득"); return; }
        SetCurWeapon(_weapon);
        PlayManager.SetPlayerWeapon(_weapon);
    }
    public void ObtainWeapon(EWeaponName _weapon)               // 무기 획득
    {
        if (m_weaponObtained[(int)_weapon]) { Debug.Log("무기 이미 습득함"); return; }
        m_weaponObtained[(int)_weapon] = true;
        SItem item = new(EItemType.WEAPON, (int)_weapon);
        m_itemInfo[item].ObtainItem();
    }


    private readonly List<EThrowItemName> m_throwItemList = new();
    public EThrowItemName CurThrowItem { get { if (m_throwItemList.Count > 0) return m_throwItemList[0]; return EThrowItemName.LAST; } }    // 현재 던지기 아이템
    public List<EThrowItemName> ThrowItemList { get { return m_throwItemList; } }       // 추가된 던지기 아이템들
    public void UseThrowItem()                                                          // 던지기 아이템 사용
    {
        if(m_throwItemList.Count == 0) { Debug.LogError("던지기 아이템 없음"); return; }
        m_throwItemList.RemoveAt(0);
    }
    public void AddThrowItem(EThrowItemName _item)                                      // 인벤에서 던지기 아이템 추가
    { 
        if(m_throwItemList.Count == ValueDefine.MAX_THROW_ITEM) { Debug.Log("리스트 꽉 참"); return; }

        if(ChkNUseItem(new(EItemType.THROW, (int)_item), 1))
        {
            m_throwItemList.Add(_item);
        }
    }


    // 아이템 인벤토리
    private readonly ItemInventory m_itemInven = new();                                 // 아이템 인벤토리
    public InventoryElm[] Inventory { get { return m_itemInven.Inventory; } }           // 아이템 목록
    public void AddInventoryItem(SItem _item, int _num) { m_itemInven.AddItem(_item, _num); }                           // 아이템 추가
    public void SetInventoryItem(int _idx, SItem _item, int _num) { m_itemInven.SetItem(_idx, _item, _num); }           // idx에 아이템 설정
    public bool ChkNUseItem(SItem _item, int _num) { return m_itemInven.ChkNUseItem(_item, _num); }                     // 아이템 확인 후 사용
    public void RemoveInventoryItem(int _idx) { m_itemInven.RemoveItem(_idx); }                                         // 아이템 제거




    // 초기 설정
    private void TempSetItem()
    {
        for (int i = 0; i<(int)EWeaponName.GOBLIN_SCEPTER; i++)
        {
            SItem item = new(EItemType.WEAPON, i);
            m_itemInfo[item].ObtainItem();
            m_weaponObtained[i] = true;
        }

        for (int i = 0; i<(int)EThrowItemName.LAST; i++)
        {
            AddInventoryItem(new(EItemType.THROW, i), 10);
        }
        for (int i = 0; i<(int)EPatternName.LAST; i++)
        {
            AddInventoryItem(new(EItemType.PATTERN, i), 10);
        }
        for (int i = 0; i<(int)EOtherItemName.LAST; i++)
        {
            AddInventoryItem(new(EItemType.OTHERS, i), 10);
        }
    }
    private void TempSetThrowItem()
    {
        for (int i = 0; i<(int)EThrowItemName.LAST; i++)
        {
            m_throwItemList.Add((EThrowItemName)i);
        }
    }

    private readonly uint[] ItemCounts = new uint[(int)EItemType.LAST]
    { (uint)EWeaponName.LAST, (uint)EPatternName.LAST, (uint)EThrowItemName.LAST, (uint)EOtherItemName.LAST };
    public void SetManager()
    {
        for (int i = 0; i<(int)EItemType.LAST; i++)
        {
            EItemType type = (EItemType)i;
            uint cnt = ItemCounts[i];
            for (int j = 0; j<cnt; j++)                         // 아이템 종류에 따라 다르게 저장 추가 예정
            {
                SItem item = new(type, j);
                ItemScriptable scriptable = GameManager.GetItemRawData(item);
                m_itemInfo[item] = new(scriptable, item);
            }
        }

        TempSetItem();              // 아이템 습득 임시 설정
        TempSetThrowItem();         // 던지기 아이템 리스트 임시 설정
    }
}
