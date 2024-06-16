using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour, IHaveData
{
    private PlayerStatInfo PlayerStatInfo { get { return PlayManager.PlayerStatInfo; } }

    public readonly static int InitialStatPoint = 2;
    public int LeftStatPoint { get; private set; }
    public int UsedStatPoint { get; private set; }


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
                UsedStatPoint += _point[i];
            }
        }
        PlayManager.ApplyPlayerStat();
        if (LeftStatPoint < 0)
            Debug.LogError("스탯 오버 사용");
    }

    public void ResetStat()
    {
        LeftStatPoint += UsedStatPoint;
        UsedStatPoint = 0;

        PlayManager.ApplyStatReset();
    }


    public void LoadData()
    {
        GameManager.RegisterData(this);
        if (PlayManager.IsNewData) { LeftStatPoint = InitialStatPoint; return; }

        SaveData data = PlayManager.CurSaveData;

        LeftStatPoint = data.LeftStatPoint;
        UsedStatPoint = data.UsedStatPoint;
    }
    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        data.LeftStatPoint = LeftStatPoint;
        data.UsedStatPoint = UsedStatPoint;
    }

    public void SetManager()
    {
        LoadData();
    }
}
