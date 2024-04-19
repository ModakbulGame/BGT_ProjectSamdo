using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject m_draggedIcon;

    [SerializeField]
    private Transform m_onDragParent;
    private Vector3 m_startPosition;

    [HideInInspector] 
    public Transform m_startParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_draggedIcon = gameObject;

        m_startPosition = transform.position;
        m_startParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        transform.SetParent(m_onDragParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_draggedIcon = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if(transform.parent == m_onDragParent)
        {
            transform.position = m_startPosition;
            transform.SetParent(m_startParent);
        }
    }
}
