using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class S306 : ParabolaExplodeScript
{
    [SerializeField]
    private GameObject clonePrefab;
    private void CloneSelf()
    {
        GameObject clone = GameManager.GetPowerObj(m_scriptable.PowerEnum);
        clone.transform.position = transform.position;
        Vector3 cloneDirection = Quaternion.Euler(0, 15, 0) * transform.forward;
        clone.GetComponent<Rigidbody>().velocity = cloneDirection * m_scriptable.MoveSpeed;
    }

    public override void FixedUpdate()
    {
        Vector3 originalDirection= Quaternion.Euler(0,-15,0) * transform.forward;
        gameObject.GetComponent<Rigidbody>().velocity = originalDirection * m_scriptable.MoveSpeed;
        base.FixedUpdate();
    }

}