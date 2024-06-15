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
    public Vector3 PlayerRot;

    // 아이템 정보
    public int Soul;
    public int PurifiedSoul;
    public int[] PatternNum;
    public List<EPatternName> HealPatternList;
    public bool[] WeaponObtained;
    public EWeaponName CurWeapon;
    public List<EThrowItemName> ThrowItemList;
    public InventoryElm[] Inventory;


    // 몬스터 정보
    public List<MonsterSaveData> MonsterData;

    public SaveData()
    {
        SavedTime = DateTime.Now.ToString();

        PatternNum = new int[(int)EPatternName.LAST];
        HealPatternList = new();
        WeaponObtained = new bool[(int)EWeaponName.LAST];
        for(int i=0; i<=(int)InventoryManager.InitialWeapon; i++) { WeaponObtained[i] = true; }
        ThrowItemList = new();
        Inventory = new InventoryElm[ValueDefine.MAX_INVENTORY];
        for(int i = 0; i<ValueDefine.MAX_INVENTORY; i++) { Inventory[i] = new(); }

        MonsterData = new();
    }
    public SaveData(SaveData _other)
    {
        SavedTime = DateTime.Now.ToString();
        OasisPoint = _other.OasisPoint;
        PlayerRot = _other.PlayerRot;

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

        MonsterData = new();
        foreach(MonsterSaveData monster in _other.MonsterData) { MonsterData.Add(monster); }
    }
}
