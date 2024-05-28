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

    public GameObject m_questObject;
    public string m_questRequireObject;
    public int m_questObjectCount;       
 

    public GameObject m_reward;                 
    public int m_rewardNum;

    private GameObject String2Obj()
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
        // m_questObject = _scriptable.QuestObject;
        m_questRequireObject = _scriptable.QuestObject;
        // m_reward = _scriptable.Reward;
        m_rewardNum = _scriptable.RewardNum;
    }
}
