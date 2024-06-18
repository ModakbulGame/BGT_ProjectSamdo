using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour, IHaveData
{
    private QuestInfo[] m_questInfoList;
    public List<QuestInfo> QuestInfoList { get { return m_questInfoList.ToList(); } }

    private void InitQuestInfos()
    {
        m_questInfoList  = new QuestInfo[(int)EQuestName.LAST];
        for(int i=0;i<(int)EQuestName.LAST;i++)
        {
            m_questInfoList[i] = new((EQuestName)i);
        }
    }

    public void SetQuestStatus(EQuestName _quest, EQuestState _status) 
    {
        m_questInfoList[(int)_quest].SetQuestStatus(_status);
    }

    public void SetQuestProgress(EQuestName _quest, float _prog)
    {
        QuestInfo info = m_questInfoList[(int)_quest];
        info.SetQuestProgress(_prog);
        if(_prog == info.QuestContent.Amount) { SetQuestStatus(_quest, EQuestState.COMPLETE); }
    }


    public void CheckMonsterKill(EMonsterName _monster)
    {
        foreach (QuestInfo quest in m_questInfoList)
        {
            EQuestType type = quest.QuestContent.Type;
            if(type != EQuestType.KILL) { return; }
        }
    }
    public void CheckMonsterPurify(EMonsterName _monster)
    {
        foreach (QuestInfo quest in m_questInfoList)
        {
            EQuestType type = quest.QuestContent.Type;
            if (type != EQuestType.PURIFY) { return; }
        }

    }





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

        m_questInfoList = new QuestInfo[(int)EQuestName.LAST];
        for (int i = 0; i<(int)EQuestName.LAST; i++)
        {
            m_questInfoList[i] = new(data.QuestInfos[i]);
        }
    }

    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        for (int i = 0; i<(int)EQuestName.LAST; i++)
        {
            data.QuestInfos[i] = new(m_questInfoList[i]);
        }
    }


    public static EQuestName String2Enum(string _id)
    {
        if(_id == "") { return EQuestName.LAST; }
        int.TryParse(_id[1..], out int idx);
        return (EQuestName)idx;
    }


    public void SetManager()
    {
        m_questProgress = gameObject.AddComponent<QuestProgressScript>();
        m_questBoolStatus = gameObject.AddComponent<QuestBoolStatus>();
        LoadData();
    }
}
