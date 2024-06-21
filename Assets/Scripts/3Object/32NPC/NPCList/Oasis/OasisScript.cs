using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OasisNPC : NPCScript
{
    [SerializeField]
    private EOasisName m_pointName;
    [SerializeField]
    private Transform m_respawnSpot;

    public EOasisName PointName { get { return m_pointName; } }
    public Vector3 RespawnPoint { get { return m_respawnSpot.position; } }

    public void SetOasis(uint _idx, NPCScriptable _scriptable)
    {
        m_pointName = (EOasisName)_idx;
        SetScriptable(_scriptable);
    }

    public override void NPCInteraction()
    {
        PlayManager.OpenOasisUI(this);
    }
}
