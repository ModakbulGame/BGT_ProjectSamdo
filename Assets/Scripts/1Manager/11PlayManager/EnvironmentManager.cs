using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EOasisPointName
{
    START1,
    START2,
    LIFE1,
    LIFE2,

    LAST
}

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_mapPositioner = new Transform[2];
    public Vector3 MapLB { get { return m_mapPositioner[0].position; } }
    public Vector3 MapRT { get { return m_mapPositioner[1].position; } }
    public float MapWidth { get { return MapRT.x - MapLB.x; } }
    public float MapHeight { get { return MapRT.z - MapLB.z; } }

    [SerializeField]
    private GameObject m_mapObject;

    private OasisNPC[] m_oasisList;
    public OasisNPC[] OasisList { get { return m_oasisList; } }

    [SerializeField]
    private MonsterSpawnPoint[] m_spawnPointList;
    public MonsterSpawnPoint[] SpawnPointList { get { return m_spawnPointList; } }


    [SerializeField]
    private QuestNPCScript[] m_npcList;
    public QuestNPCScript[] NPCList { get { return m_npcList; } }


    public void SetManager()
    {
        if (!GameManager.IsInGame) { return; }
        m_oasisList = m_mapObject.GetComponentsInChildren<OasisNPC>();
        m_npcList = m_mapObject.GetComponentsInChildren<QuestNPCScript>();
        for (int i = 0; i<m_oasisList.Length; i++) { OasisNPC oasis = m_oasisList[i]; oasis.SetPoint((EOasisPointName)i); }
        // for (int i = 0; i<m_npcList.Length; i++) { NPCList[i].SetComps(); } 

    }
}
