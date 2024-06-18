using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class QuestInfo
{
    public EQuestEnum QuestName;
    public EQuestStatus Status;
    public float QuestProgress;
    public float QuestTimeCount;
    public void SetQuestStatus(EQuestStatus _status) { Status = _status; }
    public void SetQuestProgress(float _prog) { QuestProgress = _prog; }
    public QuestScriptable QuestData { get { return GameManager.GetQeustData(QuestName); } }
    public float QuestTarget { get { return QuestData.QuestObjectCount; } }
    public float QuestTimeLimit { get { return QuestData.TimeLimit; } }
    private void SetInfo(EQuestEnum _name, EQuestStatus _status, float _progress, float _time)
    {
        QuestName = _name;
        SetQuestStatus(_status);
        SetQuestProgress(_progress);
        QuestTimeCount = _time;
    }
    public QuestInfo(EQuestEnum _name) { SetInfo(_name, EQuestStatus.NOT_AVAILABLE, 0, 0); }
    public QuestInfo(QuestInfo _other) { SetInfo(_other.QuestName, _other.Status, _other.QuestProgress, 0); }
}

public class QuestManager : MonoBehaviour, IHaveData
{
    private QuestInfo[] m_questInfoList;
    public List<QuestInfo> QuestInfoList { get { return m_questInfoList.ToList(); } }

    private void InitQuestInfos()
    {
        m_questInfoList  = new QuestInfo[(int)EQuestEnum.LAST];
        for(int i=0;i<(int)EQuestEnum.LAST;i++)
        {
            m_questInfoList[i] = new((EQuestEnum)i);
        }
    }

    public void SetQuestStatus(EQuestEnum _quest, EQuestStatus _status) 
    {
        m_questInfoList[(int)_quest].SetQuestStatus(_status);
    }

    public void SetQuestProgress(EQuestEnum _quest, float _prog)
    {
        QuestInfo info = m_questInfoList[(int)_quest];
        info.SetQuestProgress(_prog);
        if(_prog == info.QuestTarget) { SetQuestStatus(_quest, EQuestStatus.COMPLETE); }
    }


    [SerializeField]
    private List<QuestScriptable> m_questList;
    public List<QuestScriptable> QuestList { get { return m_questList; } }
    public List<QuestScriptable> CurQuestList { get; private set; }

    private QuestProgressScript m_questProgress;
    private QuestBoolStatus m_questBoolStatus;

    // 퀘스트 수행에 필요한 메소드
    public void AcceptQuest(string _id) { /* m_questProgress.AcceptQuest(_id); */}
    public void GiveUpQuest(string _id) { /* m_questProgress.GiveUpQuest(_id); */}
    public void ClearQuest(string _id) { /* m_questProgress.ClearQuest(_id); */}   
    public void CompleteQuest(string _id) { /* m_questProgress.CompleteQuest(_id); */}
    public void DoObjectQuest(string _obj, int _amount) { /* m_questProgress.DoObjectQuest(_obj, _amount); */}
    
    // 이름 일치 여부 확인
    public void SetQuestStartObjectStatus(string _start) { /* m_questBoolStatus.SetQuestStartObjectStatus(_start); */}
    public void SetQuestEndObjectStatus(string _id) { /* m_questBoolStatus.SetQuestEndObjectStatus(_id); */}
    public bool CheckQuestCompleted(string _id) { return false; /* return m_questBoolStatus.CheckQuestCompleted(_id); */}
    public bool CheckRequiredQuestObject(string _name) { return false; /* return m_questBoolStatus.CheckRequiredQuestObject(_name); */}



    public void LoadData()
    {
        GameManager.RegisterData(this);
        if (PlayManager.IsNewData) { InitQuestInfos(); return; }

        SaveData data = PlayManager.CurSaveData;

        m_questInfoList = new QuestInfo[(int)EQuestEnum.LAST];
        for (int i = 0; i<(int)EQuestEnum.LAST; i++)
        {
            m_questInfoList[i] = new(data.QuestInfos[i]);
        }
    }

    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        for (int i = 0; i<(int)EQuestEnum.LAST; i++)
        {
            data.QuestInfos[i] = new(m_questInfoList[i]);
        }
    }


    public void SetManager()
    {
        CurQuestList = new List<QuestScriptable>();
        m_questProgress = gameObject.AddComponent<QuestProgressScript>();
        m_questBoolStatus = gameObject.AddComponent<QuestBoolStatus>();
    }
}
