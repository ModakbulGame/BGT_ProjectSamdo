using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltarIconScript : MonoBehaviour
{
    private RectTransform m_rect;

    public void SetPosition(Vector2 _pos)
    {
        m_rect.anchoredPosition = _pos;
    }
    public void SetRotation(float _rot)
    {
        m_rect.localEulerAngles = new(0, 0, _rot);
    }

    public void SetComps()
    {
        m_rect = GetComponent<RectTransform>();
    }
}

