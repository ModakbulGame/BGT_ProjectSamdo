using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ECanvasType         // 캔버스 종류
{
    INGAME,
    MAIN,
    CONTROL,
    LAST
}

public enum EAlarmType
{
    SIDE_TEXT,              // 화면 사이드 텍스트 알람
    LAST
}

public class UIManager : MonoBehaviour
{                                                         // 각 캔버스
    public RectTransform GetCanvasTransform(ECanvasType _canvas)            // 캔버스 Transform 반환
    {
        if (!PlayManager.IsPlaying) { Debug.LogError("캔버스 없음"); return null; }
        return PlayManager.GetCanvas(_canvas).GetComponent<RectTransform>();
    }


    // UI와 알람 차이점 -> UI는 조작이나 상호작용 가능, 알람은 한번 띄우고 끝 (사실 아직 정확하지 않지만 종류가 많아지면 복잡해질 거 같아서 나눠둠)

    [SerializeField]
    private GameObject[] m_alarmPrefabs = new GameObject[(int)EAlarmType.LAST];                     // 알람 프리펍들
    public GameObject GetAlarmPrefab(EAlarmType _alarm) { return m_alarmPrefabs[(int)_alarm]; }

    public GameObject CreateAlarm(ECanvasType _canvas, EAlarmType _alarm)                           // 해당 캔버스에 알람 생성
    {
        GameObject alarm = Instantiate(GetAlarmPrefab(_alarm), GetCanvasTransform(_canvas));
        return alarm;
    }

    private readonly List<SideTextAlarmUIScript> m_sideAlarms = new();
    public void CreateSideTextAlarm(string _alarm)
    {
        foreach (SideTextAlarmUIScript a in m_sideAlarms) { if (a != null) a.MoveUp(m_sideAlarms); }
        GameObject alarm = CreateAlarm(ECanvasType.MAIN, EAlarmType.SIDE_TEXT);
        SideTextAlarmUIScript script = alarm.GetComponent<SideTextAlarmUIScript>();
        script.SetAlarmTxt(_alarm);
        m_sideAlarms.Add(script);
    }


    public Sprite GetMonsterSprite(EMonsterName _monster)
    {
        return GameManager.GetMonsterData(_monster).MonsterProfile;
    }

    public Sprite GetItemSprite(SItem _item)
    {
        return GameManager.GetItemData(_item).ItemIcon;
    }

    public Sprite GetPowerSprite(EPowerName _skill)
    {
        return GameManager.GetPowerData(_skill).PowerIcon;
    }
}