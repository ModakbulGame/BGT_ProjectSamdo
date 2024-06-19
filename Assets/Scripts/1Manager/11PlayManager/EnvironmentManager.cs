using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour, IHaveData
{
    [SerializeField]
    private Transform[] m_mapPositioner = new Transform[2];
    public Vector3 MapLB { get { return m_mapPositioner[0].position; } }
    public Vector3 MapRT { get { return m_mapPositioner[1].position; } }
    public float MapWidth { get { return MapRT.x - MapLB.x; } }
    public float MapHeight { get { return MapRT.z - MapLB.z; } }

    [SerializeField]
    private GameObject m_mapObject;

    private NPCScript[,] m_npcList;

    public OasisNPC[] OasisList { get { return FunctionDefine.GetRow<NPCScript, OasisNPC>(m_npcList, ((int)ENPCType.OASIS)); } }
    public AltarNPC[] AltarList { get { return FunctionDefine.GetRow<NPCScript, AltarNPC>(m_npcList, ((int)ENPCType.ALTAR)); } }
    public SlateNPC[] SlateList { get { return FunctionDefine.GetRow<NPCScript, SlateNPC>(m_npcList, ((int)ENPCType.SLATE)); } }


    [SerializeField]
    private MonsterSpawnPoint[] m_spawnPointList;
    public MonsterSpawnPoint[] SpawnPointList { get { return m_spawnPointList; } }

    private bool[] m_monsterKilled;

    public void MonsterKilled(EMonsterName _monster, EMonsterDeathType _type)
    {
        int idx = (int)_monster;
        if (!m_monsterKilled[idx]) { FirstKillMonster(_monster); }
        if (_type == EMonsterDeathType.BY_PLAYER)
        {

        }
        else if (_type == EMonsterDeathType.PURIFY)
        {

        }
    }
    private void FirstKillMonster(EMonsterName _monster)
    {

    }


    public void LoadData()
    {
        GameManager.RegisterData(this);
        m_monsterKilled = new bool[(int)EMonsterName.LAST];
        if (PlayManager.IsNewData) { return; }

        SaveData data = PlayManager.CurSaveData;

        for (int i = 0; i<(int)EMonsterName.LAST; i++)
        {
            m_monsterKilled[i] = data.MonsterKilled[i];
        }
    }
    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        for (int i = 0; i<(int)EMonsterName.LAST; i++)
        {
            data.MonsterKilled[i] = m_monsterKilled[i];
        }
    }


    public void SetManager()
    {
        if (!GameManager.IsInGame) { return; }

        m_npcList = new NPCScript[(int)ENPCType.LAST, ValueDefine.MAX_NPC_NUM];
        int[] cnt = new int[(int)ENPCType.LAST];
        NPCScript[] list = m_mapObject.GetComponentsInChildren<NPCScript>();
        for (int i = 0; i<list.Length; i++)
        {
            ENPCType type = list[i].NPC.Type;
            m_npcList[(int)type, cnt[(int)type++]] = list[i];
        }

        LoadData();
    }
}
