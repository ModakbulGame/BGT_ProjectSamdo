using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OasisNPC : NPCScript
{
    public override void StartInteract()
    {
        PlayManager.OpenOasisUI(this);
        base.StartInteract();
    }
}
