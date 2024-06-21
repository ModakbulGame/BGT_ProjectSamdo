using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour, IHaveData
{
    // 퀘스트
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
        if(_status == EQuestState.COMPLETE) { CompleteQuest(_quest); }
        if(_status == EQuestState.FINISH) { FinishQuest(_quest); }
        PlayManager.UpdateQuestSidebar();
    }

    public void SetQuestProgress(EQuestName _quest, float _prog)
    {
        QuestInfo info = m_questInfoList[(int)_quest];
        info.SetQuestProgress(_prog);
        if(_prog == info.QuestContent.Amount) { SetQuestStatus(_quest, EQuestState.COMPLETE); return; }
        PlayManager.UpdateQuestSidebar();
    }
    private void GiveUpQuest(EQuestName _quest)
    {
        QuestInfo info = m_questInfoList[(int)_quest];
        info.SetQuestStatus(EQuestState.UNLOCKED);
        QuestContent content = info.QuestContent;
        content.Amount = 0;

        // questinfolist에서 퀘스트 삭제 후 재정렬
        m_questInfoList[(int)_quest] = null;
        QuestInfo[] newArray = new QuestInfo[m_questInfoList.Length - 1];
        int newIndex = 0;

        for (int i = 0; i < m_questInfoList.Length; i++)
        {
            if (m_questInfoList[i] != null)
            {
                newArray[newIndex] = m_questInfoList[i];
                newIndex++;
            }
        }
        m_questInfoList = newArray;
    }
    private void CompleteQuest(EQuestName _quest)
    {
        QuestScriptable data = GameManager.GetQeustData(_quest);
        foreach (NPCDialogue dial in data.ResultDialogues)
        {
            PlayManager.UnlockDialogue(dial);
        }
    }
    private void FinishQuest(EQuestName _quest)
    {
        QuestScriptable data = GameManager.GetQeustData(_quest);
        QuestReward reward = data.Reward;
        GetReward(reward);
    }

    private void GetReward(QuestReward _reward)
    {
        Debug.Log($"{_reward.Type} {_reward.Amount}만큼 획득!");

        // 만약 단일 보상만 진행된다면
        switch (_reward.Type)
        {
            case ERewarTyoe.SOUL:
                PlayManager.AddSoul(_reward.Amount);
                break;
            case ERewarTyoe.PURIFIED:
                PlayManager.AddPurified(_reward.Amount);
                break;
            case ERewarTyoe.STAT:
                PlayManager.AddStatPoint(_reward.Amount);
                break;
            case ERewarTyoe.ITEM:
                PlayManager.AddInventoryItem(_reward.Item, _reward.Amount);
                break;
            default:
                break;
        }
    }


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
        LoadData();
    }
}
