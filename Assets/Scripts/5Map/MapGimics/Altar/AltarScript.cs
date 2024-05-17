using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarScript : NPCScript
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartInteract()
    {
        // PlayManager.OpenAlterUI(this);
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
    }

    public override void StopInteract()
    {
        base.StopInteract();
    }
}
