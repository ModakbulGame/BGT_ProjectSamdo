using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S304 : ParabolaExplodeScript
{
    public override bool DistanceCheck(float _distanceMoved)
    {
        return _distanceMoved > m_scriptable.CastingRange * 2;
    }
}
