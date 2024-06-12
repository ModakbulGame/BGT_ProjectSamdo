using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoolStatus : MonoBehaviour
{
    public void SetQuestStartObjectStatus(string _start)
    {
        for(int i = 0; i < PlayManager.QuestList.Count; i++)
        {
            if (PlayManager.QuestList[i].Status == EQuestStatus.ACCEPTED && PlayManager.CurQuestList[i].StartObject == _start)
            {
                for(int j = 0; j < PlayManager.NPCList.Length; j++)
                {
                    PlayManager.NPCList[j].StartQuest();
                }
            }
        }
    }

    public void SetQuestEndObjectStatus(string _end)
    {
        for (int i = 0; i < PlayManager.QuestList.Count; i++)
        {
            if (PlayManager.QuestList[i].Status == EQuestStatus.ACCEPTED && PlayManager.CurQuestList[i].EndObject == _end)
            {
                for (int j = 0; j < PlayManager.NPCList.Length; j++)
                {
                    PlayManager.NPCList[j].EndQuest();
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
