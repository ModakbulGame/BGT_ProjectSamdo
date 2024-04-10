using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatManagerOverride
{
    [SerializeField]
    private StatManager stat;
    [SerializeField]
    private bool isUseOverride;
    [SerializeField]
    private float overrideDefaultValue;

    public StatManagerOverride(StatManager stat)
        => this.stat = stat;

    public StatManager CreateStat()
    {
        var newStat = stat.Clone() as StatManager;
        if (isUseOverride)
            newStat.DefaultValue = overrideDefaultValue;
        return newStat;
    }
}
