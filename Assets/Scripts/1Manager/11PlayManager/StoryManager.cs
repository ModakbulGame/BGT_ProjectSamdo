using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ϴ��� �� ���� �ۼ� ��
public class StoryManager : MonoBehaviour
{
    public List<QuestScriptable> QuestList { get; private set; }
    public List<QuestScriptable> CurQuestList { get; private set; }

    [SerializeField]
    private QuestScriptable[] m_QuestScriptable;
    public QuestScriptable[] QuestData{ get { return m_QuestScriptable; } }




    // �Ʒ��� �Ƹ� ������Ʈ�� �и� ������ ��

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

        // Ȱ��ȭ�� ����Ʈ
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

    // ����Ʈ ����
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

    // ����Ʈ ����
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

    // ����Ʈ Ŭ����
    public void CompleteQuest(int _id)
    {
        for(int i = 0; i < QuestList.Count; i++)
        {
            if (CurQuestList[i].Id == _id && CurQuestList[i].Status == EQuestStatus.COMPLETE)
            {
                CurQuestList[i].Status = EQuestStatus.DONE;
                CurQuestList.Remove(CurQuestList[i]);
            }
            // ����


        }
        CheckChainQuest(_id);
    }

    // ���� ����Ʈ Ȯ��
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

    // ����Ʈ ����
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
