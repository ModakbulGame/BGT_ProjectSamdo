using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class S306 : ParabolaExplodeScript
{
    public override bool DistanceCheck(float _distanceMoved)
    {
        return _distanceMoved > m_scriptable.CastingRange * 2;
    }
}