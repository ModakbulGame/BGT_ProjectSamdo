using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 일단은 한 곳에 작성 중
public class StoryManager : MonoBehaviour
{
    public List<QuestScriptable> QuestList { get; private set; }
    public List<QuestScriptable> CurQuestList { get; private set; }

    [SerializeField]
    private QuestScriptable[] m_QuestScriptable;
    public QuestScriptable[] QuestData{ get { return m_QuestScriptable; } }




    // 아래는 아마 컴포넌트로 분리 진행할 듯

    public void QuestRequest(QuestObject _npcQuest)
    {
        if(_npcQuest.availableQuestIDs.Count > 0)
        {
            for(int i = 0; i < QuestList.Count; i++)
            {
                for(int j = 0; j < _npcQuest.availableQuestIDs.Count; j++)
                {
                    if (QuestList[i].Id == _npcQuest.availableQuestIDs[j] && QuestList[i].Status == EQuestStatus.AVAILABLE)
                    {
                        Debug.Log("Quest ID: " + _npcQuest.availableQuestIDs[j] + QuestList[i].Status);
                        // AcceptQuest(_npcQuest.availableQuestIDs[j]);
                        // QuestUIScript.QuestAvailable = true;
                        // QuestUIScript.m_availableQuest.Add(QuestList[i]);
                    }
                }
            }
        }

        // 활성화된 퀘스트
        for(int i = 0; i < CurQuestList.Count; i++)
        {
            for (int j = 0; j < _npcQuest.receivableQuestIDs.Count; j++)
            {
                if (CurQuestList[i].Id == _npcQuest.receivableQuestIDs[j] && CurQuestList[i].Status == EQuestStatus.ACCEPTED
                    || CurQuestList[i].Status == EQuestStatus.COMPLETE)
                {
                    Debug.Log("Quest ID: " + _npcQuest.receivableQuestIDs[j] + " is " + CurQuestList[i].Status);
                    // CompleteQuest(_npcQuest.receivableQuestIDs[j]);
                    // QuestUIScript.QuestRunning = true;
                    // QuestUIScript.m_activeQuest.Add(QuestList[i]);
                }
            }
        }
    }

    // 퀘스트 수락
    public void AcceptQuest(int _id)
    {
        for(int i = 0; i < QuestList.Count; i++)
        {
            if (QuestList[i].Id == _id && QuestList[i].Status == EQuestStatus.AVAILABLE)
            {
                CurQuestList.Add(QuestList[i]);
                QuestList[i].Status = EQuestStatus.ACCEPTED;
            }
        }
    }

    // 퀘스트 포기
    public void GiveUpQuest(int _id)
    {
        for (int i = 0; i < QuestList.Count; i++)
        {
            if (CurQuestList[i].Id == _id && CurQuestList[i].Status == EQuestStatus.ACCEPTED)
            {
                CurQuestList[i].Status = EQuestStatus.AVAILABLE;
                CurQuestList[i].CurQuestObjectCount = 0;
                CurQuestList.Remove(CurQuestList[i]);
            }
        }
    }

    // 퀘스트 클리어
    public void CompleteQuest(int _id)
    {
        for(int i = 0; i < QuestList.Count; i++)
        {
            if (CurQuestList[i].Id == _id && CurQuestList[i].Status == EQuestStatus.COMPLETE)
            {
                CurQuestList[i].Status = EQuestStatus.DONE;
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
            if (QuestList[i].Id == _questID && QuestList[i].NextQuest > 0)
            {
                tempID = QuestList[i].NextQuest;
            }
        }

        if(tempID > 0)
        {
            for(int i = 0; i < QuestList.Count; i++)
            {
                if (QuestList[i].Id == tempID && QuestList[i].Status == EQuestStatus.NOT_AVAILABLE)
                {
                    QuestList[i].Status = EQuestStatus.AVAILABLE;
                }
            }
        }
    }

    // 퀘스트 수행
    public void AddQuestItem(string _questobj, int _amount)
    {
/*        for(int i = 0; i < CurQuestList.Count; i++)
        {
            if(CurQuestList[i].Status == EQuestStatus.ACCEPTED)
            {
                if (CurQuestList[i].QuestObject == _questobj)
                {
                    CurQuestList[i].CurQuestObjectCount += _amount;
                }
                if(CurQuestList[i].CurQuestObjectCount >= CurQuestList[i].QuestObjectCount)
                {
                    CurQuestList[i].Status = EQuestStatus.COMPLETE;
                }
            }
        }*/
    }

    public void SetManager()
    {
        QuestList = new List<QuestScriptable>();
        CurQuestList = new List<QuestScriptable>();
    }
}
