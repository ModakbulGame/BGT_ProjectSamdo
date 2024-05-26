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
    private bool m_isQuestExisted;  // ��Ʈ�� ���� ����Ʈ�� �����ϴ� npc�� ��쿡�� ����ó���� ���� ����
    public bool IsQuestExisted { get { return m_isQuestExisted; } }    
    protected Transform m_npcTransform;
    public string[] m_npcDialogue;

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
