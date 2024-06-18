using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class QuestProgressScript : MonoBehaviour
{
    /*
    // ����Ʈ ����
    public void AcceptQuest(string _id)
    {
        for (int i = 0; i < PlayManager.QuestList.Count; i++)
        {
            if (PlayManager.QuestList[i].Id == _id && PlayManager.QuestList[i].Status == EQuestStatus.AVAILABLE)
            {
                QuestScriptable curQuest = PlayManager.QuestList[i];
                curQuest.Status = EQuestStatus.ACCEPTED;
                PlayManager.CurQuestList.Add(curQuest);

                if (curQuest.Types.Length > 1 && curQuest.Types[1] == EQuestType.TIMELIMIT) // �ð� ���� ����Ʈ�� ��� Types�� ���̰� 2���̹Ƿ� �˻�
                    StartCoroutine(QuestTimer(curQuest, curQuest.TimeLimit));               // �ð� ���� ����Ʈ�� ��� �������ڸ��� Ÿ�̸� ����   
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
                HideQuestClearImg(i);
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

    private IEnumerator QuestTimer(QuestScriptable _curQuest, float _time)
    {
        while (_time > 0)
        {
            _time -= Time.deltaTime;

            string minutes = Mathf.Floor(_time / 60).ToString("00");
            string seconds = (_time % 60).ToString("00");

            Debug.Log(string.Format("{0}:{1}", minutes, seconds));

            if (PlayManager.CheckQuestCompleted("Q003"))
            {
                // ����Ʈ �Ϸ� ����
                ClearQuest(_curQuest.Id);
                StopCoroutine("QuestTimer");
                yield break;
            }

            if (!PlayManager.CheckQuestCompleted("Q003") && _time <= 0)
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

                // ����Ʈ ���â ǥ�� �缳��
                PlayManager.CurQuestList.Remove(PlayManager.CurQuestList[i]);
                HideQuestClearImg(i);
            }
        }
        ActivateChainQuest(_id);
    }

    // ���� ����Ʈ Ȯ��
    private void ActivateChainQuest(string _id)
    {
        for (int i = 0; i < PlayManager.QuestList.Count; i++)
        {
            if (PlayManager.QuestList[i].Id == _id && PlayManager.QuestList[i].Status == EQuestStatus.COMPLETE)
            {
                string nextQuestID = PlayManager.QuestList[i].NextQuest;

                for(int j = 0; j < PlayManager.QuestList.Count; j++)
                {
                    if (PlayManager.QuestList[j].Id == nextQuestID && PlayManager.QuestList[j].Status == EQuestStatus.NOT_AVAILABLE)
                        PlayManager.QuestList[j].Status = EQuestStatus.AVAILABLE;
                }
            }
        }
    }

    private void ShowQuestClearImg(int _idx)
    {
        Debug.Log("����Ʈ Ŭ����!");
        PlayManager.ClearImg[_idx % 4].gameObject.SetActive(true);
    }

    private void HideQuestClearImg(int _idx)
    {
        PlayManager.ExpressCurQuestInfo();
        PlayManager.ClearImg[_idx % 4].gameObject.SetActive(false);
    }
*/
}
