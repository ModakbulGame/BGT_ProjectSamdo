using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OasisNPC : NPCScript
{
    [SerializeField]
    private EOasisPointName m_pointName;
    [SerializeField]
    private Transform m_respawnSpot;

    public EOasisPointName PointName { get { return m_pointName; } }
    public Vector3 RespawnPoint { get { return m_respawnSpot.position; } }

    public void SetPoint(EOasisPointName _point) { m_pointName = _point; }

    public override void StartInteract()
    {
        PlayManager.OpenOasisUI(this);
        base.StartInteract();
    }
}
