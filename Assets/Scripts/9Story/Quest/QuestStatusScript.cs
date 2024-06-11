using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestStatusScript : MonoBehaviour
{
    public void QuestRequest(QuestObject _npcQuest)
    {
        if (_npcQuest.availableQuestIDs.Count > 0)
        {
            for (int i = 0; i < PlayManager.QuestList.Count; i++)
            {
                for (int j = 0; j < _npcQuest.availableQuestIDs.Count; j++)
                {
                    if (PlayManager.QuestList[i].Id.Equals(_npcQuest.availableQuestIDs[j]) && PlayManager.QuestList[i].Status == EQuestStatus.AVAILABLE)
                    {
                        Debug.Log("Quest ID: " + _npcQuest.availableQuestIDs[j] + PlayManager.QuestList[i].Status);
                        // AcceptQuest(_npcQuest.availableQuestIDs[j]);
                        // QuestUIScript.QuestAvailable = true;
                        // QuestUIScript.m_availableQuest.Add(PlayManager.QuestList[i]);
                    }
                }
            }
        }
    }

    public void Activeuest(QuestObject _npcQuest)
    {
        // 활성화된 퀘스트
        for (int i = 0; i < PlayManager.CurQuestList.Count; i++)
        {
            for (int j = 0; j < _npcQuest.receivableQuestIDs.Count; j++)
            {
                if (PlayManager.CurQuestList[i].Id.Equals(_npcQuest.receivableQuestIDs[j]) && PlayManager.CurQuestList[i].Status == EQuestStatus.ACCEPTED
                    || PlayManager.CurQuestList[i].Status == EQuestStatus.COMPLETE)
                {
                    Debug.Log("Quest ID: " + _npcQuest.receivableQuestIDs[j] + " is " + PlayManager.CurQuestList[i].Status);
                    // CompleteQuest(_npcQuest.receivableQuestIDs[j]);
                    // QuestUIScript.QuestRunning = true;
                    // QuestUIScript.m_activeQuest.Add(PlayManager.QuestList[i]);
                }
            }
        }
    }

    // 퀘스트 수락
    public void AcceptQuest(string _id)
    {
        for (int i = 0; i < PlayManager.QuestList.Count; i++)
        {
            if (PlayManager.QuestList[i].Id == _id && PlayManager.QuestList[i].Status == EQuestStatus.AVAILABLE)
            {
                PlayManager.QuestList[i].Status = EQuestStatus.ACCEPTED;
                PlayManager.CurQuestList.Add(PlayManager.QuestList[i]);
            }
        }
    }

    // 퀘스트 포기
    public void GiveUpQuest(string _id)
    {
        for (int i = 0; i < PlayManager.QuestList.Count; i++)
        {
            if (PlayManager.CurQuestList[i].Id == _id && PlayManager.CurQuestList[i].Status == EQuestStatus.ACCEPTED)
            {
                PlayManager.CurQuestList[i].Status = EQuestStatus.AVAILABLE;
                PlayManager.CurQuestList[i].CurQuestObjectCount = 0;
                PlayManager.CurQuestList.Remove(PlayManager.CurQuestList[i]);
            }
        }
    }

    // 퀘스트 수행(사냥, 수집)
    public void DoObjectQuest(string _obj, int _amount)
    {
        for (int i = 0; i < PlayManager.CurQuestList.Count; i++)
        {
            if (PlayManager.CurQuestList[i].QuestObject == _obj && PlayManager.CurQuestList[i].Status == EQuestStatus.ACCEPTED)
            {
                PlayManager.CurQuestList[i].CurQuestObjectCount += _amount;

                if (PlayManager.CurQuestList[i].CurQuestObjectCount >= PlayManager.CurQuestList[i].QuestObjectCount)
                {
                    PlayManager.CurQuestList[i].Status = EQuestStatus.COMPLETE;
                }
            }
        }
    }

    // 퀘스트 수행(시간 제한)
    public void DoTimeAttackQuest(string _id)
    {

    }

    // 퀘스트 클리어
    public void ClearQuest(string _id)
    {
        for (int i = 0; i < PlayManager.CurQuestList.Count; i++)
        {
            if (PlayManager.CurQuestList[i].Id == _id && PlayManager.CurQuestList[i].Status == EQuestStatus.ACCEPTED)
            {
                Debug.Log("퀘스트 클리어!");
                PlayManager.CurQuestList[i].Status = EQuestStatus.COMPLETE;

                PlayManager.ClearImg[i % 4].gameObject.SetActive(true);
                PlayManager.ShowNPCQuestUI();
            }
        }
    }

    // 퀘스트 완료
    public void CompleteQuest(string _id)
    {
        for (int i = 0; i < PlayManager.CurQuestList.Count; i++)
        {
            if (PlayManager.CurQuestList[i].Id == _id && PlayManager.CurQuestList[i].Status == EQuestStatus.COMPLETE)
            {
                int rewardNum = PlayManager.CurQuestList[i].RewardNum;

                PlayManager.CurQuestList[i].Status = EQuestStatus.DONE;
                // 보상
                switch (PlayManager.CurQuestList[i].Reward)
                {
                    case ERewardName.SOUL:
                        PlayManager.AddSoul(rewardNum);
                        break;
                    case ERewardName.STAT:
                        PlayManager.AddStatPoint(rewardNum);
                        break;
                    case ERewardName.ITEM:
                        // 아이템 추가
                        break;
                    default:
                        break;
                }
                PlayManager.CurQuestList.Remove(PlayManager.CurQuestList[i]);
            }
        }
        // CheckChainQuest(_id);
    }

    // 연관 퀘스트 확인
    private void CheckChainQuest(string _questID)
    {
        string tempID = "";
        for (int i = 0; i < PlayManager.QuestList.Count; i++)
        {
            if (PlayManager.QuestList[i].Id.Equals(_questID) && PlayManager.QuestList[i].NextQuest != "")
            {
                tempID = PlayManager.QuestList[i].NextQuest;
            }
        }

        if (tempID != "")
        {
            for (int i = 0; i < PlayManager.QuestList.Count; i++)
            {
                if (PlayManager.QuestList[i].Id == tempID && PlayManager.QuestList[i].Status == EQuestStatus.NOT_AVAILABLE)
                {
                    PlayManager.QuestList[i].Status = EQuestStatus.AVAILABLE;
                }
            }
        }
    }
}
