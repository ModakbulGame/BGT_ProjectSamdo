using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameAlarmUIScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_alarmElm;
    [SerializeField]
    private Transform m_boxTrans;

    public readonly static float ElmHeight = 72;

    private readonly List<IngameAlarmElmScript> m_alarms = new();

    public void AddAlarm(string _alarm)
    {
        foreach (IngameAlarmElmScript showing in m_alarms) { showing.PlusIdx(); }
        GameObject alarm = Instantiate(m_alarmElm, m_boxTrans);
        alarm.GetComponent<RectTransform>().anchoredPosition = new(0, -72);
        IngameAlarmElmScript elm = alarm.GetComponent<IngameAlarmElmScript>();
        elm.AlarmOn(_alarm);
        m_alarms.Add(elm);
    }
}
