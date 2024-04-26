using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouserOverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private EventTrigger m_trigger;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public virtual void SetEvents()
    {
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerEnter, OnPointerEnter);
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerExit, OnPointerExit);
    }

    public virtual void SetComps()
    {
       m_trigger = GetComponent<EventTrigger>();
    }

    public virtual void Awake()
    {
        SetComps();
        SetEvents();
    }
}
