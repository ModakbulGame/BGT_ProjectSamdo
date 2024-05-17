using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 일단은 한 곳에 작성 중
public class StoryManager : MonoBehaviour
{
    public List<QuestData> QuestList { get; private set; }
    public List<QuestData> CurQuestList { get; private set; }

    public void QuestRequest(QuestObject _npcQuest)
    {
        if(_npcQuest.availableQuestIDs.Count > 0)
        {
            for(int i = 0; i < QuestList.Count; i++)
            {
                for(int j = 0; j < _npcQuest.availableQuestIDs.Count; j++)
                {
                    if (QuestList[i].m_id == _npcQuest.availableQuestIDs[j] && QuestList[i].m_status == EQuestStatus.AVAILABLE)
                    {
                        Debug.Log("Quest ID: " + _npcQuest.availableQuestIDs[j] + QuestList[i].m_status);
                        AcceptQuest(_npcQuest.availableQuestIDs[j]);
                    }
                }
            }
        }

        // 활성화된 퀘스트
        for(int i = 0; i < CurQuestList.Count; i++)
        {
            for (int j = 0; j < _npcQuest.receivableQuestIDs.Count; j++)
            {
                if (CurQuestList[i].m_id == _npcQuest.receivableQuestIDs[j] && CurQuestList[i].m_status == EQuestStatus.ACCEPTED
                    || CurQuestList[i].m_status == EQuestStatus.COMPLETE)
                {
                    Debug.Log("Quest ID: " + _npcQuest.receivableQuestIDs[j] + " is " + CurQuestList[i].m_status);
                    CompleteQuest(_npcQuest.receivableQuestIDs[j]);
                }
            }
        }
    }

    // 퀘스트 수락
    public void AcceptQuest(int _id)
    {
        for(int i = 0; i < QuestList.Count; i++)
        {
            if (QuestList[i].m_id == _id && QuestList[i].m_status == EQuestStatus.AVAILABLE)
            {
                CurQuestList.Add(QuestList[i]);
                QuestList[i].m_status = EQuestStatus.ACCEPTED;
            }
        }
    }

    // 퀘스트 포기
    public void GiveUpQuest(int _id)
    {
        for (int i = 0; i < QuestList.Count; i++)
        {
            if (CurQuestList[i].m_id == _id && CurQuestList[i].m_status == EQuestStatus.ACCEPTED)
            {
                CurQuestList[i].m_status = EQuestStatus.AVAILABLE;
                CurQuestList[i].m_questObjectCount = 0;
                CurQuestList.Remove(CurQuestList[i]);
            }
        }
    }

    // 퀘스트 클리어
    public void CompleteQuest(int _id)
    {
        for(int i = 0; i < QuestList.Count; i++)
        {
            if (CurQuestList[i].m_id == _id && CurQuestList[i].m_status == EQuestStatus.COMPLETE)
            {
                CurQuestList[i].m_status = EQuestStatus.DONE;
                CurQuestList.Remove(CurQuestList[i]);
            }
            // 보상


        }
        CheckChainQuest(_id);
    }

    // 연관 퀘스트 확인
    private void CheckChainQuest(int _questID)
    {
        int tempID = 0;
        for(int i = 0; i < QuestList.Count; i++)
        {
            if (QuestList[i].m_id == _questID && QuestList[i].m_nextQuest > 0)
            {
                tempID = QuestList[i].m_nextQuest;
            }
        }

        if(tempID > 0)
        {
            for(int i = 0; i < QuestList.Count; i++)
            {
                if (QuestList[i].m_id == tempID && QuestList[i].m_status == EQuestStatus.NOT_AVAILABLE)
                {
                    QuestList[i].m_status = EQuestStatus.AVAILABLE;
                }
            }
        }
    }

    // 퀘스트 수행
    public void AddQuestItem(GameObject _questobj, int _amount)
    {
        for(int i = 0; i < CurQuestList.Count; i++)
        {
            if(CurQuestList[i].m_status == EQuestStatus.ACCEPTED)
            {
                if (CurQuestList[i].m_questObject == _questobj)
                {
                    CurQuestList[i].m_questObjectCount += _amount;
                }
                if(CurQuestList[i].m_questObjectCount >= CurQuestList[i].m_questRequireObject)
                {
                    CurQuestList[i].m_status = EQuestStatus.COMPLETE;
                }
            }
        }
    }

    public void SetManager()
    {
        QuestList = new List<QuestData>();
        CurQuestList = new List<QuestData>();
    }
}
