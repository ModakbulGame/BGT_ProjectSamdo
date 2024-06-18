using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private NPCScript[,] m_npcList;

    public OasisNPC[] OasisList { get { return FunctionDefine.GetRow<NPCScript, OasisNPC>(m_npcList, ((int)ENPCType.OASIS)); } }
    public AltarNPC[] AltarList { get { return FunctionDefine.GetRow<NPCScript, AltarNPC>(m_npcList, ((int)ENPCType.ALTAR)); } }
    public SlateNPC[] SlateList { get { return FunctionDefine.GetRow<NPCScript, SlateNPC>(m_npcList, ((int)ENPCType.SLATE)); } }


    [SerializeField]
    private MonsterSpawnPoint[] m_spawnPointList;
    public MonsterSpawnPoint[] SpawnPointList { get { return m_spawnPointList; } }

    
    private void SetNPCList(ENPCType _type)
    {
        NPCScript[] list;
        switch (_type)
        {
            case ENPCType.OASIS: list = m_mapObject.GetComponentsInChildren<OasisNPC>(); break;
            case ENPCType.ALTAR: list = m_mapObject.GetComponentsInChildren<AltarNPC>(); break;
            case ENPCType.SLATE: list = m_mapObject.GetComponentsInChildren<SlateNPC>(); break;
            case ENPCType.OTHER: return;
            default: list = new NPCScript[ValueDefine.MAX_NPC_NUM]; break;
        }
        for (int i=0;i<list.Length;i++)
        {
            m_npcList[(int)_type, i] = list[i];
        }
    }

    public void SetManager()
    {
        if (!GameManager.IsInGame) { return; }

        m_npcList = new NPCScript[(int)ENPCType.LAST, ValueDefine.MAX_NPC_NUM];

        for (int i = 0; i<(int)ENPCType.LAST; i++)
        {
            SetNPCList((ENPCType)i);
        }
    }
}
