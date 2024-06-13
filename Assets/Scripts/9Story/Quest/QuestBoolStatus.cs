using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoolStatus : MonoBehaviour
{
    public void SetQuestStartObjectStatus(string _start)
    {
        for(int i = 0; i < PlayManager.QuestList.Count; i++)
        {
            if (PlayManager.QuestList[i].Status == EQuestStatus.AVAILABLE && PlayManager.QuestList[i].StartObject == _start)
            {
                for (int j = 0; j < PlayManager.NPCList.Length; j++)
                {
                    if(PlayManager.QuestList[i].StartObject == PlayManager.NPCList[j].NPCName) 
                        PlayManager.NPCList[j].StartQuest();
                }
            }
        }
    }

    public void SetQuestEndObjectStatus(string _end)
    {
        for (int i = 0; i < PlayManager.QuestList.Count; i++)
        {
            if (PlayManager.QuestList[i].Status == EQuestStatus.ACCEPTED && PlayManager.QuestList[i].EndObject == _end)
            {
                for (int j = 0; j < PlayManager.NPCList.Length; j++)
                {
                    if (PlayManager.QuestList[i].EndObject == PlayManager.NPCList[j].NPCName)
                        PlayManager.NPCList[j].EndQuest();
                    Debug.Log(PlayManager.NPCList[j].IsQuestEnded);
                }
            }
        }
    }

    public bool CheckRequiredQuestObject(string _name)
    {
        for (int i = 0; i < PlayManager.CurQuestList.Count; i++)
        {
            if (PlayManager.CurQuestList[i].Status == EQuestStatus.ACCEPTED && PlayManager.CurQuestList[i].QuestObject == _name)
                return true;
        }
        return false;
    }
}
