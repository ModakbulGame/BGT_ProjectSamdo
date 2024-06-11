using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCScript : NPCScript
{
    [SerializeField]
    private bool m_isQuestStarted;  // ��Ʈ�� ���� ����Ʈ�� �����ϴ� npc�� ��쿡�� ����ó���� ���� ����
    [SerializeField]
    private bool m_isQuestEnded;    // �굵

    public bool IsQuestStarted { get { return m_isQuestStarted; } }
    public bool IsQuestEnded { get { return m_isQuestEnded; } }

    public override void StartInteract()
    {
        PlayManager.OpenNPCUI(this);
        base.StartInteract();
    }
}
