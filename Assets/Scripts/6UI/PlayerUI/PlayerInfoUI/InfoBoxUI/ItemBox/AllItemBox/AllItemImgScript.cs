using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllItemImgScript : MouseOverInfoUI
{
    private AllItemElmScript m_parent;
    public void SetParent(AllItemElmScript _parent) { m_parent = _parent; }

    private Image m_itemImg;

    public void SetItemImage(Sprite _img)
    {
        m_itemImg.sprite = _img;
    }

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

    public override void SetComps()
    {
        base.SetComps();
        m_itemImg = GetComponent<Image>();
    }
}
