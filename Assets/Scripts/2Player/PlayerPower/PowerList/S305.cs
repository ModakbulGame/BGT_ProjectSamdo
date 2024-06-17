using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S305 : ParabolaExplodeScript
{
    public override bool DistanceCheck(float _distanceMoved)
    {
        return _distanceMoved > m_scriptable.CastingRange * 2;
    }
}
