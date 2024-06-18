using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class QuestProgressScript : MonoBehaviour
{
    /*
    // 퀘스트 수락
    public void AcceptQuest(string _id)
    {
        for (int i = 0; i < PlayManager.QuestList.Count; i++)
        {
            if (PlayManager.QuestList[i].Id == _id && PlayManager.QuestList[i].Status == EQuestStatus.AVAILABLE)
            {
                QuestScriptable curQuest = PlayManager.QuestList[i];
                curQuest.Status = EQuestStatus.ACCEPTED;
                PlayManager.CurQuestList.Add(curQuest);

                if (curQuest.Types.Length > 1 && curQuest.Types[1] == EQuestType.TIMELIMIT) // 시간 제한 퀘스트인 경우 Types의 길이가 2개이므로 검사
                    StartCoroutine(QuestTimer(curQuest, curQuest.TimeLimit));               // 시간 제한 퀘스트인 경우 수락하자마자 타이머 시작   
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
                HideQuestClearImg(i);
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
                // 퀘스트 완료 수행
                ClearQuest(_curQuest.Id);
                StopCoroutine("QuestTimer");
                yield break;
            }

            if (!PlayManager.CheckQuestCompleted("Q003") && _time <= 0)
            {
                // 퀘스트 실패
                PlayManager.CurQuestList.Remove(_curQuest);
                _curQuest.Status = EQuestStatus.AVAILABLE;
                StopCoroutine("QuestTimer");
                yield break;
            }
            yield return null;
        }
    }

    // 퀘스트 클리어
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

                // 퀘스트 요약창 표시 재설정
                PlayManager.CurQuestList.Remove(PlayManager.CurQuestList[i]);
                HideQuestClearImg(i);
            }
        }
        ActivateChainQuest(_id);
    }

    // 연관 퀘스트 확인
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
        Debug.Log("퀘스트 클리어!");
        PlayManager.ClearImg[_idx % 4].gameObject.SetActive(true);
    }

    private void HideQuestClearImg(int _idx)
    {
        PlayManager.ExpressCurQuestInfo();
        PlayManager.ClearImg[_idx % 4].gameObject.SetActive(false);
    }
*/
}
