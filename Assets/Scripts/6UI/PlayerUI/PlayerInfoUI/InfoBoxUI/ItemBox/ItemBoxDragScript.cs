using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxDragScript : DragDropUIScript
{
    private AllItemElmScript m_parent;
    public void SetParent(AllItemElmScript _parent) { m_parent = _parent; }

    private AllItemBoxScript Box { get { return m_parent.Box; } }
    public override Transform MoveTrans => Box.transform;

    private int DropIdx { get; set; }
    private int ElmCount { get { return Box.ElmNum; } }


    public override bool CheckPos()
    {
        RectTransform[] slots = Box.ElmTrans;
        for (int i = 0; i < ElmCount; i++)
        {
            Vector3 slot = slots[i].position + Vector3.up * 0.48f;
            float dist = Vector2.Distance(m_rect.position, slot);
            if (dist < 0.5f)
            {
                DropIdx = i;
                m_rect.position = slot;
                return true;
            }
        }
        DropIdx = -1;
        return false;
    }
    public override void DropAction()
    {
        if (DropIdx == -1) { return; }
        Box.UpdateUI();
    }
}
