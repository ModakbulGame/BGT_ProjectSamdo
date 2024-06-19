using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class InventoryManager : MonoBehaviour, IHaveData
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
    public void LooseSoul(int _num)
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
    private readonly int[] m_patternNum = new int[(int)EPatternName.LAST];
    public int[] PatternNum { get { return m_patternNum; } }
    public void AddPattern(EPatternName _type, int _num)
    {
        m_patternNum[(int)_type] += _num;
        GameManager.CreateSideTextAlarm($"���� {_num}�� ȹ��!");
        PlayManager.UpdateMaterials();
    }
    public void UsePattern(EPatternName _type, int _num)
    {
        if (m_patternNum[(int)_type] < _num) { Debug.LogError("���� ���纸�� ���� ���� ���"); return; }
        m_patternNum[(int)_type] -= _num;
        PlayManager.UpdateMaterials();
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


    // ��� �κ��丮
    private readonly bool[] m_weaponObtained = new bool[(int)EWeaponName.LAST];
    public bool[] WeaponObatined { get { return m_weaponObtained; } }
    public EWeaponName CurWeapon { get; private set; } = EWeaponName.BASIC_BLADE;    // ���� ���� ����

    public readonly static EWeaponName InitialWeapon = EWeaponName.BASIC_SCEPTER;

    private void InitWeaponObtained() { for(int i = 0; i<=(int)InitialWeapon; i++) { m_weaponObtained[i] = true; } }
    public void ObtainWeapon(EWeaponName _weapon)
    {
        m_weaponObtained[(int)_weapon] = true;
    }
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
    public void SetThrowItem(int _idx, EThrowItemName _item)
    {
        if(_idx >= m_throwItemList.Count) { AddThrowItem(_item); return; }
        m_throwItemList.Insert(_idx, _item);
    }
    public void SwapThrowItem(int _idx1, int _idx2)
    {
        if(_idx2 >= m_throwItemList.Count || _idx1 >= m_throwItemList.Count) { return; }
        EThrowItemName item1 = m_throwItemList[_idx1], item2 = m_throwItemList[_idx2];
        m_throwItemList[_idx2] = item1; m_throwItemList[_idx1] = item2;
    }
    public void RemoveThrowItem(int _idx)
    {
        if(_idx >= m_throwItemList.Count) { return; }
        EThrowItemName item = m_throwItemList[_idx];
        m_throwItemList.RemoveAt(_idx);
        AddInventoryItem(new(EItemType.THROW, (int)item), 1);
    }



    // ������ �κ��丮
    private ItemInventory m_itemInven;                                                  // ������ �κ��丮
    public InventoryElm[] Inventory { get { return m_itemInven.Inventory; } }           // ������ ���
    public void AddInventoryItem(SItem _item, int _num) { AddInventoryItem(_item, _num, false); }                       // ������ �߰�
    public void AddInventoryItem(SItem _item, int _num, bool _isNew)            
    {
        m_itemInven.AddItem(_item, _num);
        if (_isNew) { CheckItemObtained(_item, _num); }
    }
    public void SetInventoryItem(int _idx, SItem _item, int _num) { m_itemInven.SetItem(_idx, _item, _num); }           // idx�� ������ ����
    public void SwapItemInven(int _idx1, int _idx2) { m_itemInven.SwapItem(_idx1, _idx2); }
    public bool ChkNUseItem(SItem _item, int _num) { return m_itemInven.ChkNUseItem(_item, _num); }                     // ������ Ȯ�� �� ���
    public void RemoveInventoryItem(int _idx) { m_itemInven.RemoveItem(_idx); }                                         // ������ ����

    private void CheckItemObtained(SItem _item, int _num)                   // ������ ȹ�� �� ����Ʈ Ȯ��
    {
        List<QuestInfo> infos = PlayManager.QuestInfoList;

        EQuestType questType = EQuestType.COLLECT;
        if (questType == EQuestType.LAST) { return; }

        foreach (QuestInfo quest in infos)
        {
            if (quest.State != EQuestState.ACCEPTED || quest.QuestContent.Type != questType
                || quest.QuestContent.Item != _item) { continue; }

            float target = quest.QuestContent.Amount;
            float prog = quest.QuestProgress + _num;
            if(prog > target) { prog = target; }
            PlayManager.SetQuestProgress(quest.QuestName, prog);
        }
    }



    public void LoadData()
    {
        GameManager.RegisterData(this);
        if (PlayManager.IsNewData) { m_itemInven = new(); InitWeaponObtained(); return; }

        SaveData data = PlayManager.CurSaveData;

        m_soulNum = data.Soul;
        m_purifiedNum = data.PurifiedSoul;
        for(int i = 0; i<(int)EPatternName.LAST; i++) { m_patternNum[i] = data.PatternNum[i]; }
        foreach(EPatternName pattern in data.HealPatternList) { m_healPatternList.Add(pattern); }
        for(int i = 0; i<(int)EWeaponName.LAST; i++) { m_weaponObtained[i] = data.WeaponObtained[i]; }
        CurWeapon = data.CurWeapon;
        foreach(EThrowItemName throwItem in data.ThrowItemList) { m_throwItemList.Add(throwItem); }
        m_itemInven = new(data.Inventory);
    }
    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        data.Soul = m_soulNum;
        data.PurifiedSoul = m_purifiedNum;
        for (int i = 0; i<(int)EPatternName.LAST; i++) { data.PatternNum[i] = m_patternNum[i]; }
        foreach (EPatternName pattern in m_healPatternList) { data.HealPatternList.Add(pattern); }
        for(int i = 0; i<(int)EWeaponName.LAST; i++) { data.WeaponObtained[i] = m_weaponObtained[i]; }
        data.CurWeapon = CurWeapon;
        foreach(EThrowItemName throwItem in m_throwItemList) { data.ThrowItemList.Add(throwItem); }
        for (int i = 0; i<ValueDefine.MAX_INVENTORY; i++) { data.Inventory[i].SetItem(Inventory[i]); }
    }



    public void SetManager()
    {
        LoadData();
    }
}
