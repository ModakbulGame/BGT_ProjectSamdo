using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropUI : MonoBehaviour
{
    private EventTrigger m_trigger;





    private void SetEvents()
    {

    }
    private void SetComps()
    {
        m_trigger = gameObject.AddComponent<EventTrigger>();
        SetEvents();
    }
    private void Awake()
    {
        SetComps();
    }
}
