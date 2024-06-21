using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameAlarmUIScript : MonoBehaviour
{
    private readonly int MaxAlarm = 8;

    public readonly float ElmHeight = 72;

    private readonly Queue<IngameAlarmElmScript> m_showingAlarms = new();
    private readonly Queue<IngameAlarmElmScript> m_hiddenAlarms = new();

    public void AddAlarm(string _alarm)
    {
        IngameAlarmElmScript next;
        if(m_hiddenAlarms.Count == 0) { next = GetLastElm(); }
        else { next = m_hiddenAlarms.Dequeue(); }
        next.AlarmOn(_alarm);
        foreach (IngameAlarmElmScript elm in m_showingAlarms) { elm.PlusIdx(); }
        m_showingAlarms.Enqueue(next);
    }

    private IngameAlarmElmScript GetLastElm()
    {
        IngameAlarmElmScript last = m_showingAlarms.Dequeue();
        last.ResetElm();
        return last;
    }


    public void ReturnElm()
    {
        IngameAlarmElmScript elm = m_showingAlarms.Dequeue();
        m_hiddenAlarms.Enqueue(elm);
    }





    private void SetComps()
    {
        IngameAlarmElmScript[] elms = GetComponentsInChildren<IngameAlarmElmScript>();
        if(elms.Length != MaxAlarm) { Debug.LogError("알람 개수 다름"); return; }
        foreach(IngameAlarmElmScript elm in elms) { elm.SetParent(this); elm.SetComps(); m_hiddenAlarms.Enqueue(elm); }
    }

    private void Awake()
    {
        SetComps();
    }
}
