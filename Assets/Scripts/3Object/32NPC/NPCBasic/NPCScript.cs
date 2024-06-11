using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour, IInteractable
{
    [SerializeField]
    private NPCScriptable m_scriptable;

    public string NPCID { get { return m_scriptable.ID; } }
    public string NPCName { get { return m_scriptable.NPCName; } }
    public string[] NPCDialogues { get { return m_scriptable.Dialogues; } }


    public EInteractType InteractType { get { return EInteractType.NPC; } }
    public virtual string InfoTxt { get { return "��ȭ"; } }            // ��ȣ�ۿ� UI�� ��� �� => ���� ��Ȳ�� ���� �ٲ�� ��� ���ǹ� �߰� 


    public virtual void StartInteract()
    {
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
    }

    public virtual void StopInteract()
    {
        PlayManager.StopPlayerInteract();
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
    }
}
