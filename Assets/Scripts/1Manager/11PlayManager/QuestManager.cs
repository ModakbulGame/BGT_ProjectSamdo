using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour, IHaveData
{
    // Äù½ºÆ®
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
        Debug.Log($"{_reward.Type} {_reward.Amount}¸¸Å­ È¹µæ!");

        // ¸¸¾à ´ÜÀÏ º¸»ó¸¸ ÁøÇàµÈ´Ù¸é
        switch(_reward.Type)
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
                // ¾ÆÀÌÅÛ È¹µæ
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
