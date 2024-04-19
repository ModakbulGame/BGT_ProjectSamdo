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
    // 맵 ui 위치 보정을 위한 빈 GameObject Array; 여기 넣을까 PlayUIManager에 넣을까 고민 중
    public Transform[] m_normalizeObjects = new Transform[4];
    public GameObject[] m_mapOasis;

    public void setManager()
    {
        m_mapOasis = GameObject.FindGameObjectsWithTag(ValueDefine.OASIS_TAG);
    }
}
