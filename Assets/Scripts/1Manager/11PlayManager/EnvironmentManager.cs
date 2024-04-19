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
    // �� ui ��ġ ������ ���� �� GameObject Array; ���� ������ PlayUIManager�� ������ ��� ��
    public Transform[] m_normalizeObjects = new Transform[4];
    public GameObject[] m_mapOasis;

    public void setManager()
    {
        m_mapOasis = GameObject.FindGameObjectsWithTag(ValueDefine.OASIS_TAG);
    }
}
