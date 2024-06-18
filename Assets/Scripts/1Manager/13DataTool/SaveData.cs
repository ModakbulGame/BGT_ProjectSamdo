using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // 기본 정보
    public string SavedTime;
    public EOasisPointName OasisPoint;

    // 플레이어 정보
    public Vector3 PlayerRot;
    public PlayerStatInfo StatInfo;
    public int LeftStatPoint;
    public int UsedStatPoint;

    // 아이템 정보
    public int Soul;
    public int PurifiedSoul;
    public int[] PatternNum;
    public List<EPatternName> HealPatternList;
    public bool[] WeaponObtained;
    public EWeaponName CurWeapon;
    public List<EThrowItemName> ThrowItemList;
    public InventoryElm[] Inventory;

    // 스킬 정보
    public EPowerName[] PowerSlot;
    public bool[] PowerObtained;


    // 퀘스트 정보
    public QuestInfo[] QuestInfos;


    // 몬스터 정보
    public List<MonsterSaveData> MonsterData;

    public SaveData()
    {
        SavedTime = DateTime.Now.ToString();

        StatInfo = new();
        PowerSlot = new EPowerName[ValueDefine.MAX_POWER_SLOT] { EPowerName.LAST,EPowerName.LAST,EPowerName.LAST};
        PowerObtained = new bool[(int)EPowerName.LAST];

        PatternNum = new int[(int)EPatternName.LAST];
        HealPatternList = new();
        WeaponObtained = new bool[(int)EWeaponName.LAST];
        for(int i=0; i<=(int)InventoryManager.InitialWeapon; i++) { WeaponObtained[i] = true; }
        ThrowItemList = new();
        Inventory = new InventoryElm[ValueDefine.MAX_INVENTORY];
        for(int i = 0; i<ValueDefine.MAX_INVENTORY; i++) { Inventory[i] = new(); }

        QuestInfos = new QuestInfo[(int)EQuestEnum.LAST];
        for(int i = 0; i<(int)EQuestEnum.LAST; i++) { QuestInfos[i] = new((EQuestEnum)i); }

        MonsterData = new();
    }
    public SaveData(SaveData _other)
    {
        SavedTime = DateTime.Now.ToString();
        OasisPoint = _other.OasisPoint;

        PlayerRot = _other.PlayerRot;
        StatInfo = new(_other.StatInfo);
        LeftStatPoint = _other.LeftStatPoint;
        UsedStatPoint = _other.UsedStatPoint;
        PowerSlot = new EPowerName[ValueDefine.MAX_POWER_SLOT];
        for(int i = 0; i<ValueDefine.MAX_POWER_SLOT; i++) { PowerSlot[i] = _other.PowerSlot[i]; }
        PowerObtained = new bool[(int)EPowerName.LAST];
        for(int i = 0; i<(int)EPowerName.LAST; i++) { PowerObtained[i] = _other.PowerObtained[i]; }

        Soul = _other.Soul;
        PurifiedSoul = _other.PurifiedSoul;
        PatternNum = new int[(int)EPatternName.LAST];
        for(int i = 0; i<(int)EPatternName.LAST; i++) { PatternNum[i] = _other.PatternNum[i]; }
        HealPatternList = new();
        foreach(EPatternName pattern in _other.HealPatternList) { HealPatternList.Add(pattern); }
        WeaponObtained = new bool[(int)EWeaponName.LAST];
        for(int i = 0; i<(int)EWeaponName.LAST; i++) { WeaponObtained[i] = _other.WeaponObtained[i]; }
        CurWeapon = _other.CurWeapon;
        ThrowItemList = new();
        foreach(EThrowItemName item in _other.ThrowItemList) { ThrowItemList.Add(item); }
        Inventory = new InventoryElm[ValueDefine.MAX_INVENTORY];
        for(int i = 0; i<ValueDefine.MAX_INVENTORY; i++) { Inventory[i] = new(_other.Inventory[i]); }

        QuestInfos = new QuestInfo[(int)EQuestEnum.LAST];
        for(int i = 0; i<(int)EQuestEnum.LAST; i++) { QuestInfos[i] = new(_other.QuestInfos[i]); }

        MonsterData = new();
        foreach(MonsterSaveData monster in _other.MonsterData) { MonsterData.Add(monster); }
    }
}
