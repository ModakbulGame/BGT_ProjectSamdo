using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItemImgScript : MouseOverInfoUI
{
    private ThrowItemElmScript m_parent;
    public void SetParent(ThrowItemElmScript _parent) { m_parent = _parent; }

    public override void ShowInfo()
    {
        m_parent.ShowInfo();
    }
    public override void HideInfo()
    {
        m_parent.HideInfo();
    }
    public override void ActiveItem()
    {
        m_parent.ActiveItem();
    }
    public override void SetInfoPos(Vector2 _pos)
    {
        m_parent.SetInfoPos(_pos);
    }
}
