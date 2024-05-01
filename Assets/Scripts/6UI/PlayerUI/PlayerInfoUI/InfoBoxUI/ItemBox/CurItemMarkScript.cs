using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurItemMarkScript : MonoBehaviour
{
    private RectTransform m_rect;
    public bool isCliked;

    public void SetPos(Vector2 _pos)
    {
        if (isCliked)
        {
            m_rect.anchoredPosition = _pos;
        }
    }

    public void CheckPos(Vector2 _pos)
    {

    }

    public void SetComps()
    {
        m_rect = GetComponent<RectTransform>();
        isCliked = false;
        transform.gameObject.SetActive(false);
    }
}
