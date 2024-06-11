using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OasisNPC : NPCScript
{
    [SerializeField]
    private Transform m_respawnSpot;

    public Vector3 RespawnPoint { get { return m_respawnSpot.position; } }

    public override void StartInteract()
    {
        PlayManager.OpenOasisUI(this);
        base.StartInteract();
    }
}
