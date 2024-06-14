using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string SavedTime;
    public EOasisPointName OasisPoint;
    public Vector3 PlayerRot;

    public List<MonsterSaveData> MonsterData;

    public SaveData()
    { 
        MonsterData = new();
        SavedTime = DateTime.Now.ToString();
    }
    public SaveData(SaveData _other)
    {
        SavedTime = DateTime.Now.ToString();
        OasisPoint = _other.OasisPoint;
        PlayerRot = _other.PlayerRot;
        MonsterData = new();
        foreach(MonsterSaveData monster in _other.MonsterData) { MonsterData.Add(monster); }
    }
}
