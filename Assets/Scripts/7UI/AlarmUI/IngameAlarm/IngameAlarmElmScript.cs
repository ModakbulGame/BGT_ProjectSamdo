using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameAlarmElmScript : MonoBehaviour
{
    private IngameAlarmUIScript m_parent;
    public void SetParent(IngameAlarmUIScript _parent) { m_parent = _parent; }

    private RectTransform m_rect;
    private TextMeshProUGUI m_alarmTxt;

    private readonly float MoveSpeed = 512;

    private readonly float FadeDelay = 3;
    private readonly float FadeSpeed = 0.5f;

    private float TextAlpha { get { return m_alarmTxt.color.a; } set { Color col = m_alarmTxt.color; col.a = value; m_alarmTxt.color = col; } }

    public bool IsShowing { get; private set; }
    private float FadeTimeCount { get; set; }
    private int TargetIdx { get; set; }

    private float ElmHeight { get { return m_parent.ElmHeight; } }
    private float TargetPos { get { return TargetIdx * ElmHeight; } }
    public void PlusIdx() { TargetIdx++; }

    public void AlarmOn(string _alarm)
    {
        m_alarmTxt.text  = _alarm;
        TargetIdx = 0;
        IsShowing = true;
        FadeTimeCount = FadeDelay;
        TextAlpha = 1;
    }

    public void DestoryElm()
    {
        Destroy(gameObject);
    }


    private void Awake()
    {
        SetComps();
    }
    private void SetComps()
    {
        m_rect = GetComponent<RectTransform>();
        m_alarmTxt = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (!IsShowing) { return; }
        if (FadeTimeCount > 0) { FadeTimeCount -= Time.deltaTime; }
        else if(TextAlpha > 0) 
        {
            TextAlpha -= FadeSpeed * Time.deltaTime;
            if(TextAlpha <= 0 && IsShowing) { DestoryElm(); }
        }
        if (m_rect.anchoredPosition.y < TargetPos)
        {
            m_rect.anchoredPosition += Time.deltaTime * MoveSpeed * Vector2.up;
            if (m_rect.anchoredPosition.y > TargetPos)
            {
                m_rect.anchoredPosition = Vector2.up * TargetPos;
            }
        }
    }
}
