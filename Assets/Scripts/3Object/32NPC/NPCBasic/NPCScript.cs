using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected NPCScriptable m_scriptable;
    public void SetScriptable(NPCScriptable _scriptable) { m_scriptable = _scriptable; }

    public Vector2 Position2 { get { return new(transform.position.x, transform.position.z); } }

    public SNPC NPC { get { return m_scriptable.NPC; } }
    public string NPCName { get { return m_scriptable.NPCName; } }
    public string[] NPCDialogues { get { return m_scriptable.Dialogues; } }


    public EInteractType InteractType { get { return EInteractType.NPC; } }
    public virtual string InfoTxt { get { return "대화"; } }            // 상호작용 UI에 띄울 말 => 말이 상황에 따라 바뀌는 경우 조건문 추가 


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
