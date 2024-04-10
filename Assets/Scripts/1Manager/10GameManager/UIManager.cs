using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ECanvasType         // ĵ���� ����
{
    INGAME,
    MAIN,
    CONTROL,
    LAST
}

public enum EAlarmType
{
    SIDE_TEXT,              // ȭ�� ���̵� �ؽ�Ʈ �˶�
    LAST
}

public class UIManager : MonoBehaviour
{                                                         // �� ĵ����
    public RectTransform GetCanvasTransform(ECanvasType _canvas)            // ĵ���� Transform ��ȯ
    {
        if (!PlayManager.IsPlaying) { Debug.LogError("ĵ���� ����"); return null; }
        return PlayManager.GetCanvas(_canvas).GetComponent<RectTransform>();
    }


    // UI�� �˶� ������ -> UI�� �����̳� ��ȣ�ۿ� ����, �˶��� �ѹ� ���� �� (��� ���� ��Ȯ���� ������ ������ �������� �������� �� ���Ƽ� ������)

    [SerializeField]
    private GameObject[] m_alarmPrefabs = new GameObject[(int)EAlarmType.LAST];                     // �˶� �������
    public GameObject GetAlarmPrefab(EAlarmType _alarm) { return m_alarmPrefabs[(int)_alarm]; }

    public GameObject CreateAlarm(ECanvasType _canvas, EAlarmType _alarm)                           // �ش� ĵ������ �˶� ����
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



    // UI �̹��� Sprite
    [SerializeField]
    private Sprite[] m_monsterSprites = new Sprite[(int)EMonsterName.LAST];
    [SerializeField]
    private Sprite[] m_weaponSprites = new Sprite[(int)EWeaponName.LAST];
    [SerializeField]
    private Sprite[] m_patternSprites = new Sprite[(int)EPatternName.LAST];
    [SerializeField]
    private Sprite[] m_throwItemSprites = new Sprite[(int)EThrowItemName.LAST];
    [SerializeField]
    private Sprite[] m_otherItemSprites = new Sprite[(int)EOtherItemName.LAST];
    [SerializeField]
    private Sprite[] m_skillSprites = new Sprite[(int)ESkillName.LAST];

    public Sprite GetMonsterSprite(EMonsterName _monster)
    {
        return m_monsterSprites[(int)_monster];
    }

    public Sprite GetItemSprite(SItem _item)
    {
        int idx = _item.Idx;
        return _item.Type switch
        {
            EItemType.WEAPON => m_weaponSprites[idx],
            EItemType.PATTERN => m_patternSprites[idx],
            EItemType.THROW => m_throwItemSprites[idx],
            EItemType.OTHERS => m_otherItemSprites[idx],

            _ => null
        };
    }

    public Sprite GetSkillSprite(ESkillName _skill)
    {
        return m_skillSprites[(int)_skill];
    }
}