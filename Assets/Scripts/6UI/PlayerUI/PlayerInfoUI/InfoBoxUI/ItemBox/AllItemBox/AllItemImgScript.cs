using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AllItemImgScript : DragMouseOverInfoUI
{
    private AllItemElmScript m_parent;
    public void SetParent(AllItemElmScript _parent) { m_parent = _parent; }

    private Image m_itemImg;

    public override Transform MoveTrans => m_parent.MoveTrans;


    public void SetImg(Sprite _img) { m_itemImg.sprite = _img; }

    public override void StartDrag(PointerEventData _data)
    {
        if (_data.button == PointerEventData.InputButton.Right) { m_parent.ItemElmClick(); return; }
        base.StartDrag(_data);
    }

    public override bool CheckPos()
    {


        return false;
    }

    public override void DropAction()
    {
        base.DropAction();
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
