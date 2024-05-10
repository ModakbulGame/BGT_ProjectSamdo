using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragMouseOverInfoUI : DragDropUIScript            // 마우스 올리면 정보 뜨는 애
{
    private bool IsMouseOn { get; set; }


    public override void StartDrag(PointerEventData _data)
    {
        base.StartDrag(_data);
        HideInfo();
    }


    private void PointerOn(PointerEventData _data) { if (Dragging) { return; } IsMouseOn = true; ShowInfo(); StartCoroutine(ShowingUI()); }
    private void PointerOff(PointerEventData _data) { IsMouseOn = false; HideInfo(); }

    public virtual void ShowInfo() { }
    public virtual void HideInfo() { }
    public virtual void SetInfoPos(Vector2 _pos) { }
    private IEnumerator ShowingUI()
    {
        while (!Dragging && IsMouseOn)
        {
            Vector2 mouse = Mouse.current.position.ReadValue();
            SetInfoPos(mouse);
            yield return null;
        }
    }


    public override void SetEvents()
    {
        base.SetEvents();
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerEnter, PointerOn);
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerExit, PointerOff);
    }
}
