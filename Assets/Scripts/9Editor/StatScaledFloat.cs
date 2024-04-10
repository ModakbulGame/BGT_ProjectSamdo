using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatScaledFloat
{
    // 1���� float���� 1���� Stat�� �޾� ���� ���� return ���ִ� ����ü
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
