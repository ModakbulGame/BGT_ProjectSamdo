using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData : MonoBehaviour
{
    public string m_title;              // ����Ʈ �̸�
    public int m_id;                    // ����Ʈ ID
    public EQuestType m_type;           // ����Ʈ ����
    public EQuestStatus m_status;       // ���� ����Ʈ ����
    public string m_description;        // ����Ʈ ����
    public int m_nextQuest;             // ���� ����Ʈ ID
        
    public GameObject m_questObject;    // ����Ʈ �� �ʿ��� ������Ʈ
    public int m_questObjectCount;      // �ʿ��� ������Ʈ�� ���� ��
    public int m_questRequireObject;    // ����Ʈ �Ϸῡ �ʿ��� ������Ʈ ��

    public int m_reward;                // ����
}
