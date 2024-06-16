using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class QuestProgressScript : MonoBehaviour
{
    public QuestScriptable RequireQuest(string _id) 
    {
        for (int i = 0; i < PlayManager.CurQuestList.Count; i++)
        {
            if (PlayManager.QuestList[i].Id == _id && PlayManager.QuestList[i].Status == EQuestStatus.AVAILABLE)
                return PlayManager.QuestList[i];
        }
        return null;
    }

    public void Activeuest(QuestObject _npcQuest)
    {
        // Ȱ��ȭ�� ����Ʈ
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

    // ����Ʈ ����
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

    // ����Ʈ ����
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

    // ����Ʈ ����(���, ����)
    public void DoObjectQuest(string _obj, int _amount)
    {
        for (int i = 0; i < PlayManager.CurQuestList.Count; i++)
        {
            if (PlayManager.CurQuestList[i].QuestObject == _obj && PlayManager.CurQuestList[i].Status == EQuestStatus.ACCEPTED)
            {
                PlayManager.CurQuestList[i].CurQuestObjectCount += _amount;
                Debug.Log($"{_obj} {PlayManager.CurQuestList[i].CurQuestObjectCount} / {PlayManager.CurQuestList[i].QuestObjectCount}");

                if (PlayManager.CurQuestList[i].CurQuestObjectCount >= PlayManager.CurQuestList[i].QuestObjectCount)
                {
                    string clearQuest = PlayManager.CurQuestList[i].Id;
                    ClearQuest(clearQuest);
                }
            }
        }
    }

    // ����Ʈ ����(�ð� ����)
    public void DoTimeLimitQuest(string _id)
    {
        for (int i = 0; i < PlayManager.CurQuestList.Count; i++)
        {
            if (PlayManager.CurQuestList[i].Id == _id && PlayManager.CurQuestList[i].Status == EQuestStatus.ACCEPTED)
            {
                QuestScriptable curQuest = PlayManager.CurQuestList[i];
                StartCoroutine(QuestTimer(curQuest, PlayManager.CurQuestList[i].TimeLimit));
            }
        }
    }

    // ����Ʈ Ŭ����
    public void ClearQuest(string _id)
    {
        for (int i = 0; i < PlayManager.CurQuestList.Count; i++)
        {
            if (PlayManager.CurQuestList[i].Id == _id && PlayManager.CurQuestList[i].Status == EQuestStatus.ACCEPTED)
            {
                PlayManager.CurQuestList[i].Status = EQuestStatus.COMPLETE;
                ShowQuestClearImg(i);
            }
        }
    }

    // ����Ʈ �Ϸ�
    public void CompleteQuest(string _id)
    {
        for (int i = 0; i < PlayManager.CurQuestList.Count; i++)
        {
            if (PlayManager.CurQuestList[i].Id == _id && PlayManager.CurQuestList[i].Status == EQuestStatus.COMPLETE)
            {
                int rewardNum = PlayManager.CurQuestList[i].RewardNum;

                PlayManager.CurQuestList[i].Status = EQuestStatus.DONE;

                // ����
                switch (PlayManager.CurQuestList[i].Reward)
                {
                    case ERewardName.SOUL:
                        PlayManager.AddSoul(rewardNum);
                        break;
                    case ERewardName.STAT:
                        PlayManager.AddStatPoint(rewardNum);
                        break;
                    case ERewardName.ITEM:
                        // ������ �߰�
                        break;
                    default:
                        break;
                }
                PlayManager.CurQuestList.Remove(PlayManager.CurQuestList[i]);
            }
        }
        // CheckChainQuest(_id);
    }

    // ���� ����Ʈ Ȯ��
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

    private void ShowQuestClearImg(int _idx)
    {
        Debug.Log("����Ʈ Ŭ����!");
        PlayManager.ClearImg[_idx % 4].gameObject.SetActive(true);
    }

    private IEnumerator QuestTimer(QuestScriptable _curQuest, float _time)
    {
        while (_time > 0)
        {
            _time -= Time.deltaTime;

            string minutes = Mathf.Floor(_time / 60).ToString("00");
            string seconds = (_time % 60).ToString("00");
            
            Debug.Log(string.Format("{0}:{1}", minutes, seconds));

            if (PlayManager.CheckQuestCompleted("Q002"))
            {
                // ����Ʈ �Ϸ� ����
                ClearQuest(_curQuest.Id);
                StopCoroutine("QuestTimer");
                yield break; 
            }

            if (!PlayManager.CheckQuestCompleted("Q002") && _time <= 0)
            {
                // ����Ʈ ����
                PlayManager.CurQuestList.Remove(_curQuest);
                _curQuest.Status = EQuestStatus.AVAILABLE;
                StopCoroutine("QuestTimer");
                yield break; 
            }
            yield return null;
        }
    }
}
