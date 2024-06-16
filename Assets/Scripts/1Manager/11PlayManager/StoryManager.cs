using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ϴ��� �� ���� �ۼ� ��
public class StoryManager : MonoBehaviour
{
    [SerializeField]
    private List<QuestScriptable> m_questList;
    public List<QuestScriptable> QuestList { get { return m_questList; } }
    public List<QuestScriptable> CurQuestList { get; private set; }

    private QuestProgressScript m_questProgress;
    private QuestBoolStatus m_questBoolStatus;

    // ����Ʈ ���࿡ �ʿ��� �޼ҵ�
    public void AcceptQuest(string _id) { m_questProgress.AcceptQuest(_id); }
    public void GiveUpQuest(string _id) { m_questProgress.GiveUpQuest(_id); }
    public void ClearQuest(string _id) { m_questProgress.ClearQuest(_id); }   
    public void CompleteQuest(string _id) { m_questProgress.CompleteQuest(_id); }
    public void DoObjectQuest(string _obj, int _amount) { m_questProgress.DoObjectQuest(_obj, _amount); }
    
    // �̸� ��ġ ���� Ȯ��
    public void SetQuestStartObjectStatus(string _start) { m_questBoolStatus.SetQuestStartObjectStatus(_start); }
    public void SetQuestEndObjectStatus(string _id) { m_questBoolStatus.SetQuestEndObjectStatus(_id); }
    public bool CheckQuestCompleted(string _id) { return m_questBoolStatus.CheckQuestCompleted(_id); }
    public bool CheckRequiredQuestObject(string _name) { return m_questBoolStatus.CheckRequiredQuestObject(_name); }


    public void SetManager()
    {
        CurQuestList = new List<QuestScriptable>();
        m_questProgress = gameObject.AddComponent<QuestProgressScript>();
        m_questBoolStatus = gameObject.AddComponent<QuestBoolStatus>();
    }
}
