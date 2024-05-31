using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoolStatus : MonoBehaviour
{
    private List<QuestScriptable> m_questList = PlayManager.QuestList;

    // 현재 퀘스트 상태 반환
    public bool RequestAvailableQuest(int _id)
    {
        for (int i = 0; i < m_questList.Count; i++)
        {
            if (m_questList[i].Id.Equals(_id) && m_questList[i].Status == EQuestStatus.AVAILABLE)
            {
                return true;
            }
        }
        return false;
    }

    public bool RequestAcceptedQuest(int _id)
    {
        for (int i = 0; i < m_questList.Count; i++)
        {
            if (m_questList[i].Id.Equals(_id) && m_questList[i].Status == EQuestStatus.ACCEPTED)
            {
                return true;
            }
        }
        return false;
    }

    public bool RequestCompleteQuest(int _id)
    {
        for (int i = 0; i < m_questList.Count; i++)
        {
            if (m_questList[i].Id.Equals(_id) && m_questList[i].Status == EQuestStatus.COMPLETE)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckAvailableQuests(QuestObject _npcQuest)
    {
        for (int i = 0; i < m_questList.Count; i++)
        {
            for (int j = 0; j < _npcQuest.availableQuestIDs.Count; j++)
            {
                if (m_questList[i].Id.Equals(_npcQuest.availableQuestIDs[j]) && m_questList[i].Status == EQuestStatus.AVAILABLE)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckAcceptedQuests(QuestObject _npcQuest)
    {
        for (int i = 0; i < m_questList.Count; i++)
        {
            for (int j = 0; j < _npcQuest.availableQuestIDs.Count; j++)
            {
                if (m_questList[i].Id.Equals(_npcQuest.availableQuestIDs[j]) && m_questList[i].Status == EQuestStatus.ACCEPTED)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckCompletedQuests(QuestObject _npcQuest)
    {
        for (int i = 0; i < m_questList.Count; i++)
        {
            for (int j = 0; j < _npcQuest.availableQuestIDs.Count; j++)
            {
                if (m_questList[i].Id.Equals(_npcQuest.availableQuestIDs[j]) && m_questList[i].Status == EQuestStatus.COMPLETE)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
