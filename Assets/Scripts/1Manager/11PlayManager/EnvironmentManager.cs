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

public enum EAltarName
{
    START1,
    LIFE1,
    TEMP1,

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

    private AltarScript[] m_altarList;
    public AltarScript[] AltarList { get { return m_altarList; } }

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
        if (m_oasisList.Length != (int)EOasisPointName.LAST) { Debug.LogError("오아시스 개수 안맞음"); return; }
        for (int i = 0; i<m_oasisList.Length; i++)
        {
            NPCScriptable scriptable = GameManager.GetNPCData(new(ENPCType.OASIS, i));
            m_oasisList[i].SetScriptable(scriptable);
        }
        m_altarList = m_mapObject.GetComponentsInChildren<AltarScript>();
        if (m_altarList.Length != (int)EOasisPointName.LAST) { Debug.LogError("제단 개수 안맞음"); return; }
        for (int i = 0; i<m_altarList.Length; i++)
        {
            NPCScriptable scriptable = GameManager.GetNPCData(new(ENPCType.ALTAR, i));
            m_altarList[i].SetScriptable(scriptable);
        }
    }
}
