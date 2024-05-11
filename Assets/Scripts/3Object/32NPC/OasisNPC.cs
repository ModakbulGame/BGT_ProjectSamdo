using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OasisNPC : NPCScript
{
    public Transform OasisTransform { get { return m_npcTransform; }  }

    public override void StartInteract()
    {
        PlayManager.OpenOasisUI(this);
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
    }

    public override void StopInteract()
    {
        PlayManager.StopPlayerInteract();
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
    }
}
