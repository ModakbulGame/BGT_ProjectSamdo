using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData : MonoBehaviour
{
    public string m_title;              // 퀘스트 이름
    public int m_id;                    // 퀘스트 ID
    public EQuestType m_type;           // 퀘스트 종류
    public EQuestStatus m_status;       // 현재 퀘스트 상태
    public string m_description;        // 퀘스트 설명
    public int m_nextQuest;             // 다음 퀘스트 ID
        
    public GameObject m_questObject;    // 퀘스트 시 필요한 오브젝트
    public int m_questObjectCount;      // 필요한 오브젝트의 현재 수
    public int m_questRequireObject;    // 퀘스트 완료에 필요한 오브젝트 수

    public int m_reward;                // 보상
}
