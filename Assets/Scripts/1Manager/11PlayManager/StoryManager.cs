using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 일단은 한 곳에 작성 중
public class StoryManager : MonoBehaviour
{
    [SerializeField]
    private List<QuestScriptable> m_questList;
    public List<QuestScriptable> QuestList { get { return m_questList; } }
    public List<QuestScriptable> CurQuestList { get; private set; }

    private QuestStatusScript m_questStatus;
    private QuestBoolStatus m_questBoolStatus;

    // 퀘스트 수행에 필요한 메소드
    public void AcceptQuest(string _id) { m_questStatus.AcceptQuest(_id); }
    public void GiveUpQuest(string _id) { m_questStatus.GiveUpQuest(_id); }
    public void ClearQuest(string _id) { m_questStatus.ClearQuest(_id); }   
    public void CompleteQuest(string _id) { m_questStatus.CompleteQuest(_id); }
    public void DoObjectQuest(string _obj, int _amount) { m_questStatus.DoObjectQuest(_obj, _amount); }
    
    // 이름 일치 여부 확인
    public void SetQuestStartObjectStatus(string _start) { m_questBoolStatus.SetQuestStartObjectStatus(_start); }
    public void SetQuestEndObjectStatus(string _end) { m_questBoolStatus.SetQuestEndObjectStatus(_end); }
    public bool CheckRequiredQuestObject(string _name) { return m_questBoolStatus.CheckRequiredQuestObject(_name); }


    public void SetManager()
    {
        CurQuestList = new List<QuestScriptable>();
        m_questStatus = gameObject.AddComponent<QuestStatusScript>();
        m_questBoolStatus = gameObject.AddComponent<QuestBoolStatus>();
    }
}
