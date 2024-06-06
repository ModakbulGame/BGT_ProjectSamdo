using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCInfo : ObjectBaseInfo
{
    public int NPCID;
    public float UIOffset = 5;
    private void SetInfo(int _id)
    {
        NPCID = _id;
    }
}

public class NPCScript : MonoBehaviour, IInteractable
{
    [SerializeField]
    private NPCInfo m_npcInfo;
    [SerializeField]
    private NPCScriptable m_scriptable;
    public string NPCName { get { return m_npcInfo.ObjectName; } }
    public int NPCID { get { return m_npcInfo.NPCID; } }
    public void SetScriptable(NPCScriptable _scriptable) { m_scriptable = _scriptable; SetInfo(); }

    [SerializeField]
    private bool m_isQuestStarted;  // ��Ʈ�� ���� ����Ʈ�� �����ϴ� npc�� ��쿡�� ����ó���� ���� ����
    [SerializeField]
    private bool m_isQuestEnded;  // �굵

    public bool IsQuestStarted { get { return m_isQuestStarted; } }   
    public bool IsQuestEnded { get { return m_isQuestEnded; } }

    protected Transform m_npcTransform;
    public string[] m_npcDialogue;

    public bool InteractableRotation 
    {
        get
        {
            Vector3 forward = transform.forward;                                // ������Ʈ�� ���� ����
            Vector3 toTarget = PlayManager.PlayerPos - transform.position;      // �÷��̾� ��ġ���� ����

            toTarget.Normalize();
            float angle = Vector3.SignedAngle(forward, toTarget, Vector3.up);
            return angle >= -60f && angle <= 60f;
        }
    }

    public EInteractType InteractType { get { return EInteractType.NPC; } }
    public bool Interactions { get { return gameObject.CompareTag(ValueDefine.NPC_TAG); } }
    public float UIOffset { get { return m_npcInfo.UIOffset; } }        // ��ȣ�ۿ� UI ��� ����
    public virtual string InfoTxt { get { return "��ȭ"; } }    // ��ȣ�ۿ� UI�� ��� �� => ���� ��Ȳ�� ���� �ٲ�� ��� ���ǹ� �߰� 

    private void SetInfo()
    {

    }

    

    public virtual void StartInteract() 
    {
        PlayManager.OpenNPCUI(this);
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
    }

    public virtual void StopInteract() 
    {
        PlayManager.StopPlayerInteract();
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
    }
}
