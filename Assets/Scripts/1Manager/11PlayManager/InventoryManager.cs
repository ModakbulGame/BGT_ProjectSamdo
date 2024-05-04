using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
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
        if (m_purifiedNum < _num) { Debug.LogError("보유 성불 영혼보다 많은 개수 사용"); return; }
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
    private bool[] WeaponObatined { get {
            bool[] list = new bool[(int)EWeaponName.LAST];
            for (int i = 0; i<(int)EWeaponName.LAST; i++)
            { list[i] = GameManager.GetItemInfo(new SItem(EItemType.WEAPON, i)).Obtained; }
            return list; } }
    public EWeaponName CurWeapon { get; private set; } = EWeaponName.BASIC_SWORD;    // 장착 중인 무기
    public void SetCurWeapon(EWeaponName _weapon)               // 무기 설정
    {
        CurWeapon = _weapon;
    }
    public void EquipWeapon(EWeaponName _weapon)                // 무기 장착
    {
        if (!WeaponObatined[(int)_weapon]) { Debug.LogError("무기 미습득"); return; }
        SetCurWeapon(_weapon);
        PlayManager.SetPlayerWeapon(_weapon);
    }


    // 회복 아이템 (각인 문양?)
    private readonly List<EPatternName> m_healPatternList = new();
    public EPatternName CurHealPattern { get { if (m_healPatternList.Count > 0) return m_healPatternList[0]; return EPatternName.LAST; } }
    public EPatternName[] HealPatternList { get { return m_healPatternList.ToArray(); } }
    public void UseHealItem()
    {
        if (m_healPatternList.Count == 0) { Debug.LogError("회복 아이템 없음"); return; }
        m_healPatternList.RemoveAt(0);
    }
    public void RegisterHealItem(EPatternName _pattern)
    {
        if (m_healPatternList.Count >= ValueDefine.MAX_HEAL_ITEM) { Debug.Log("리스트 꽉 참"); return; }

        if (ChkNUseItem(new(EItemType.PATTERN, (int)_pattern), 1))
        {
            m_healPatternList.Add(_pattern);
        }
    }

    // 투척 아이템
    private readonly List<EThrowItemName> m_throwItemList = new();
    public EThrowItemName CurThrowItem { get { if (m_throwItemList.Count > 0) return m_throwItemList[0]; return EThrowItemName.LAST; } }    // 현재 던지기 아이템
    public List<EThrowItemName> ThrowItemList { get { return m_throwItemList; } }       // 추가된 던지기 아이템들
    public void UseThrowItem()                                                          // 던지기 아이템 사용
    {
        if (m_throwItemList.Count == 0) { Debug.LogError("던지기 아이템 없음"); return; }
        m_throwItemList.RemoveAt(0);
    }
    public void AddThrowItem(EThrowItemName _item)                                      // 인벤에서 던지기 아이템 추가
    {
        if (m_throwItemList.Count == ValueDefine.MAX_THROW_ITEM) { Debug.Log("리스트 꽉 참"); return; }

        if (ChkNUseItem(new(EItemType.THROW, (int)_item), 1))
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
            GameManager.ObtainWeapon((EWeaponName)i);
            WeaponObatined[i] = true;
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
    private void TempSetItemSlot()
    {
        for (int i = 0; i<(int)EPatternName.LAST; i+=2)
        {
            m_healPatternList.Add((EPatternName)i);
        }
        for (int i = 0; i<(int)EThrowItemName.LAST; i++)
        {
            m_throwItemList.Add((EThrowItemName)i);
        }
    }


    public void SetManager()
    {
        TempSetItem();              // 아이템 습득 임시 설정
        TempSetItemSlot();         // 던지기 아이템 리스트 임시 설정
    }
}
