using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatScaledFloat
{
    // 1개의 float값과 1개의 Stat을 받아 둘이 곱해 return 해주는 구조체
    public float defaultValue;
    public StatManager scaleStat;

    public float GetValue(Stats stats)
    {
        if(scaleStat&&stats.TryGetStat(scaleStat,out var stat))
            return defaultValue * (1+stat.Value);
        else
            return defaultValue;
    }
}
