using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMapPointName
{
    POINT1,
    POINT2,
    POINT3,

    LAST
}

public class EnvironmentManager : MonoBehaviour
{
    public Transform[] m_mapPositioner = new Transform[2];
    public Vector3 MapLB { get { return m_mapPositioner[0].position; } }
    public Vector3 MapRT { get { return m_mapPositioner[1].position; } }
    public float MapWidth { get { return MapRT.x - MapLB.x; } }
    public float MapHeight { get { return MapRT.z - MapLB.z; } }

    public GameObject[] MapOasis;
    public NPCScript[] NPCs;


    public void setManager()
    {

    }
}
