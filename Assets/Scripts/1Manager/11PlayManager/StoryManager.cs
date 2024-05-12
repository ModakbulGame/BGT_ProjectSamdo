using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ϴ��� �� ���� �ۼ� ��
public class StoryManager : MonoBehaviour
{
    public List<QuestData> m_questList = new List<QuestData>();
    public List<QuestData> m_curQuestList = new List<QuestData>();

    // ����Ʈ ����
    public void AcceptQuest(int _id)
    {
        for(int i = 0; i < m_questList.Count; i++)
        {
            if (m_questList[i].m_id == _id && m_questList[i].m_status == EQuestStatus.AVAILABLE)
            {
                m_curQuestList.Add(m_questList[i]);
                m_questList[i].m_status = EQuestStatus.ACCEPTED;
            }
        }
    }

    // ����Ʈ ����
    public void GiveUpQuest(int _id)
    {
        for (int i = 0; i < m_questList.Count; i++)
        {
            if (m_curQuestList[i].m_id == _id && m_curQuestList[i].m_status == EQuestStatus.ACCEPTED)
            {
                m_curQuestList[i].m_status = EQuestStatus.AVAILABLE;
                m_curQuestList[i].m_questObjectCount = 0;
                m_curQuestList.Remove(m_curQuestList[i]);
            }
        }
    }

    // ����Ʈ Ŭ����
    public void CompleteQuest(int _id)
    {
        for(int i = 0; i < m_questList.Count; i++)
        {
            if (m_curQuestList[i].m_id == _id && m_curQuestList[i].m_status == EQuestStatus.COMPLETE)
            {
                m_curQuestList[i].m_status = EQuestStatus.DONE;
                m_curQuestList.Remove(m_curQuestList[i]);
            }
        }
    }

    // ����Ʈ ����
    public void AddQuestItem(GameObject _questobj, int _amount)
    {
        for(int i = 0; i < m_curQuestList.Count; i++)
        {
            if(m_curQuestList[i].m_status == EQuestStatus.ACCEPTED)
            {
                if (m_curQuestList[i].m_questObject == _questobj)
                {
                    m_curQuestList[i].m_questObjectCount += _amount;
                }
                if(m_curQuestList[i].m_questObjectCount >= m_curQuestList[i].m_questRequireObject)
                {
                    m_curQuestList[i].m_status = EQuestStatus.COMPLETE;
                }
            }
        }
    }

    // ���� ����Ʈ ���� ��ȯ
    public bool RequestAvailableQuest(int _id)
    {
        for (int i = 0; i < m_questList.Count; i++)
        {
            if (m_questList[i].m_id == _id && m_questList[i].m_status == EQuestStatus.AVAILABLE)
            {
                return true;
            }
        }
        return false;
    }

    public bool RequestAcceptedQuest(int _id)
    {
        for(int i = 0; i < m_questList.Count; i++)
        {
            if (m_questList[i].m_id == _id && m_questList[i].m_status == EQuestStatus.ACCEPTED)
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
            if (m_questList[i].m_id == _id && m_questList[i].m_status == EQuestStatus.COMPLETE)
            {
                return true;
            }
        }
        return false;
    }
}
