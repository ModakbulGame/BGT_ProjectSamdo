using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // ��ȥ
    private int m_soulNum = 0;
    public int SoulNum { get { return m_soulNum; } }
    public void AddSoul(int _num)
    {
        m_soulNum += _num;
        GameManager.CreateSideTextAlarm($"��ȥ {_num}�� ȹ��!");
        PlayManager.UpdateMaterials();
    }
    public void UseSoul(int _num)
    {
        if (m_soulNum < _num) { Debug.LogError("���� ��ȥ���� ���� ���� ���"); return; }
        m_soulNum -= _num;
        PlayManager.UpdateMaterials();
    }


    // ���� ��ȥ
    private int m_purifiedNum = 0;
    public int PurifiedNum { get { return m_purifiedNum; } }
    public void AddPurified(int _num)
    {
        m_purifiedNum += _num;
        GameManager.CreateSideTextAlarm($"���� ��ȥ {_num}�� ȹ��!");
        PlayManager.UpdateMaterials();
    }
    public void UsePurified(int _num)
    {
        if (m_purifiedNum < _num) { Debug.LogError("���� ���� ��ȥ���� ���� ���� ���"); return; }
        m_purifiedNum -= _num;
        PlayManager.UpdateMaterials();
    }

    // ����
    private readonly int[] m_patternNum = new int[(int)EProperty.LAST] { 0, 0, 0 };
    public int[] PatternNum { get { return m_patternNum; } }
    public void AddPattern(EProperty _type, int _num)
    {
        m_patternNum[(int)_type] += _num;
        GameManager.CreateSideTextAlarm($"���� {_num}�� ȹ��!");
        PlayManager.UpdateMaterials();
    }
    public void UsePattern(EProperty _type, int _num)
    {
        if (m_patternNum[(int)_type] < _num) { Debug.LogError("���� ���纸�� ���� ���� ���"); return; }
        m_patternNum[(int)_type] -= _num;
        PlayManager.UpdateMaterials();
    }


    // ��� �κ��丮
    private bool[] WeaponObatined { get {
            bool[] list = new bool[(int)EWeaponName.LAST];
            for (int i = 0; i<(int)EWeaponName.LAST; i++)
            { list[i] = GameManager.GetItemInfo(new SItem(EItemType.WEAPON, i)).Obtained; }
            return list; } }
    public EWeaponName CurWeapon { get; private set; } = EWeaponName.BASIC_SWORD;    // ���� ���� ����
    public void SetCurWeapon(EWeaponName _weapon)               // ���� ����
    {
        CurWeapon = _weapon;
    }
    public void EquipWeapon(EWeaponName _weapon)                // ���� ����
    {
        if (!WeaponObatined[(int)_weapon]) { Debug.LogError("���� �̽���"); return; }
        SetCurWeapon(_weapon);
        PlayManager.SetPlayerWeapon(_weapon);
    }


    // ȸ�� ������ (���� ����?)
    private readonly List<EPatternName> m_healPatternList = new();
    public EPatternName CurHealPattern { get { if (m_healPatternList.Count > 0) return m_healPatternList[0]; return EPatternName.LAST; } }
    public EPatternName[] HealPatternList { get { return m_healPatternList.ToArray(); } }
    public void UseHealItem()
    {
        if (m_healPatternList.Count == 0) { Debug.LogError("ȸ�� ������ ����"); return; }
        m_healPatternList.RemoveAt(0);
    }
    public void RegisterHealItem(EPatternName _pattern)
    {
        if (m_healPatternList.Count >= ValueDefine.MAX_HEAL_ITEM) { Debug.Log("����Ʈ �� ��"); return; }

        if (ChkNUseItem(new(EItemType.PATTERN, (int)_pattern), 1))
        {
            m_healPatternList.Add(_pattern);
        }
    }

    // ��ô ������
    private readonly List<EThrowItemName> m_throwItemList = new();
    public EThrowItemName CurThrowItem { get { if (m_throwItemList.Count > 0) return m_throwItemList[0]; return EThrowItemName.LAST; } }    // ���� ������ ������
    public List<EThrowItemName> ThrowItemList { get { return m_throwItemList; } }       // �߰��� ������ �����۵�
    public void UseThrowItem()                                                          // ������ ������ ���
    {
        if (m_throwItemList.Count == 0) { Debug.LogError("������ ������ ����"); return; }
        m_throwItemList.RemoveAt(0);
    }
    public void AddThrowItem(EThrowItemName _item)                                      // �κ����� ������ ������ �߰�
    {
        if (m_throwItemList.Count == ValueDefine.MAX_THROW_ITEM) { Debug.Log("����Ʈ �� ��"); return; }

        if (ChkNUseItem(new(EItemType.THROW, (int)_item), 1))
        {
            m_throwItemList.Add(_item);
        }
    }


    // ������ �κ��丮
    private readonly ItemInventory m_itemInven = new();                                 // ������ �κ��丮
    public InventoryElm[] Inventory { get { return m_itemInven.Inventory; } }           // ������ ���
    public void AddInventoryItem(SItem _item, int _num) { m_itemInven.AddItem(_item, _num); }                           // ������ �߰�
    public void SetInventoryItem(int _idx, SItem _item, int _num) { m_itemInven.SetItem(_idx, _item, _num); }           // idx�� ������ ����
    public bool ChkNUseItem(SItem _item, int _num) { return m_itemInven.ChkNUseItem(_item, _num); }                     // ������ Ȯ�� �� ���
    public void RemoveInventoryItem(int _idx) { m_itemInven.RemoveItem(_idx); }                                         // ������ ����


    // �ʱ� ����
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
        TempSetItem();              // ������ ���� �ӽ� ����
        TempSetItemSlot();         // ������ ������ ����Ʈ �ӽ� ����
    }
}
