//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ItemBoxDragScript : DragDropUIScript
//{
//    private AllItemElmScript m_parent;
//    public void SetParent(AllItemElmScript _parent) { m_parent = _parent; }

//    private EItemName Item { get { return m_parent.Cur; } }
//    private ItemBoxUIScript Box { get { return m_parent.Box; } }
//    public override Transform MoveTrans => Box.transform;

//    private int DropIdx { get; set; }


//    public override bool CheckPos()
//    {
//        RectTransform[] slots = Box.SlotTrans;
//        for (int i = 0; i < ValueDefine.MAX_INVENTORY; i++)
//        {
//            Vector3 slot = slots[i].position + Vector3.up * 0.48f;
//            float dist = Vector2.Distance(m_rect.position, slot);
//            if (dist < 0.5f)
//            {
//                DropIdx = i;
//                m_rect.position = slot;
//                return true;
//            }
//        }
//        DropIdx = -1;
//        return false;
//    }
//    public override void DropAction()
//    {
//        if (DropIdx == -1) { return; }
//        GameManager.RegisterSkilSlot(Skill, DropIdx);
//        Box.UpdateUI();
//    }


//}
