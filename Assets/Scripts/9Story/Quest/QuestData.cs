using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData : MonoBehaviour
{
    public string m_title;             
    public int m_id;                    
    public EQuestType m_type;           
    public EQuestStatus m_status;       
    public string m_description;        
    public int m_nextQuest;             

    public string m_questObject;             // 퀘스트 수행 시 필요한 오브젝트
    public int m_questRequireObjectCount;    // 수행에 필요한 오브젝트 수
    public int m_questObjectCount;           // 현재 수집 / 사냥한 오브젝트 수 

    public string m_reward;                 
    public int m_rewardNum;

    private GameObject String2Data(string _name)
    {
        return null;
    }

    public void SetQuestData(QuestScriptable _scriptable)
    {
        m_title = _scriptable.Title;
        m_id = _scriptable.Id;
        m_type = _scriptable.Type;
        m_description = _scriptable.Description;
        m_nextQuest = _scriptable.NextQuest;
        m_questObject = _scriptable.QuestObject;
        m_questObjectCount = _scriptable.QuestObjectCount;
        m_reward = _scriptable.Reward;
        m_rewardNum = _scriptable.RewardNum;
    }
}
