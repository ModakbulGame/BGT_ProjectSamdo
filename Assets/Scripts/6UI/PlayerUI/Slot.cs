using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    GameObject Icon()
    {
        if (transform.childCount > 0)
            return transform.GetChild(0).gameObject;
        else
            return null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if(Icon() == null) 
        {
            IconDrag.m_draggedIcon.transform.SetParent(transform);
            IconDrag.m_draggedIcon.transform.position = transform.position;
        }
    }
}
