using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private PlayerStatInfo PlayerStatInfo { get { return PlayManager.PlayerStatInfo; } }

    public int LeftStatPoint { get; private set; } = 2;


    public void AddStatPoint(int _add)
    {
        LeftStatPoint += _add;
        PlayManager.UpdateInfoUI();
    }


    public void UpgradeStat(int[] _point)
    {
        for (int i = 0; i<(int)EStatInfoName.LAST; i++)
        {
            if (_point[i] > 0)
            {
                PlayerStatInfo.UpgradeStat((EStatInfoName)i, _point[i]);
                LeftStatPoint -= _point[i];
            }
        }
        if (LeftStatPoint < 0)
            Debug.LogError("스탯 오버 사용");
    }


}
