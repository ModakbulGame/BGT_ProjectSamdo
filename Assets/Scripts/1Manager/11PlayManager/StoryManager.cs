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

    public void AcceptQuest(string _id) { m_questStatus.AcceptQuest(_id); }
    public void GiveUpQuest(string _id) { m_questStatus.GiveUpQuest(_id); }
    public void CompleteQuest(string _id) { m_questStatus.CompleteQuest(_id); }
   

    public void SetManager()
    {
        CurQuestList = new List<QuestScriptable>();
        m_questStatus = gameObject.AddComponent<QuestStatusScript>();
    }
}
