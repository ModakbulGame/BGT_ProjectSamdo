using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseOverInfoUI : MonoBehaviour            // 마우스 올리면 정보 뜨는 애
{
    private RectTransform m_rect;

    private EventTrigger m_trigger;

    private bool IsMouseOn { get; set; }


    private void PointerOn(PointerEventData _data) { IsMouseOn = true; ShowInfo(); StartCoroutine(ShowingUI()); }
    private void PointerOff(PointerEventData _data) { IsMouseOn = false; HideInfo(); }

    public virtual void ShowInfo() { }
    public virtual void HideInfo() { }
    public virtual void SetInfoPos(Vector2 _pos) { }
    private IEnumerator ShowingUI()
    {
        while (IsMouseOn)
        {
            Vector2 mouse = Mouse.current.position.ReadValue();
            SetInfoPos(mouse);
            yield return null;
        }
    }



    private void SetEvents()
    {
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerEnter, PointerOn);
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerExit, PointerOff);
    }
    public virtual void SetComps()
    {
        m_rect = GetComponent<RectTransform>();
        m_trigger = gameObject.AddComponent<EventTrigger>();
        SetEvents();
    }
}
