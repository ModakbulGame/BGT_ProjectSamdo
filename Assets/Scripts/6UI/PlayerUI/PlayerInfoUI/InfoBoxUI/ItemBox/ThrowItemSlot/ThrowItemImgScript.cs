using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItemImgScript : MouseOverInfoUI
{
    private ThrowItemElmScript m_parent;
    private Transform m_imgTransform;
    public void SetParent(ThrowItemElmScript _parent) { m_parent = _parent; }

    public override void ShowInfo()
    {
        m_parent.ShowInfo();
    }
    public override void HideInfo()
    {
        m_parent.HideInfo();
    }
    public override void SetInfoPos(Vector2 _pos)
    {
        m_parent.SetInfoPos(_pos);
    }
    public void ActivateMark()
    {
        m_parent.ActivateMark(m_imgTransform);
    }

    public override void SetComps()
    {
        base.SetComps();
        m_imgTransform = GetComponent<RectTransform>();
    }
}
