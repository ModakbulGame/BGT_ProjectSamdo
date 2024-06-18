using MalbersAnimations.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForceManager : MonoBehaviour, IHaveData
{
    // 플레이어 능력치
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
        for (int i = 0; i<(int)EStatName.LAST; i++)
        {
            if (_point[i] > 0)
            {
                PlayerStatInfo.UpgradeStat((EStatName)i, _point[i]);
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


    // 플레이어 권능
    private readonly bool[] m_powerObtained = new bool[(int)EPowerName.LAST];
    public bool[] PowerObtained { get { return m_powerObtained; } }


    private readonly EPowerName[] m_powerSlot = new EPowerName[ValueDefine.MAX_POWER_SLOT] { EPowerName.LAST, EPowerName.LAST, EPowerName.LAST };
    public EPowerName[] PowerSlot { get { return m_powerSlot; } }

    public void RegisterPowerSlot(EPowerName _skill, int _idx) { m_powerSlot[_idx] = _skill; }
    public void ObtainPower(EPowerName _skill)
    {
        int idx = (int)_skill;
        if (PowerObtained[idx]) { Debug.Log("이미 획득한 스킬"); return; }
        PowerObtained[idx] = true;
    }


    public void LoadData()
    {
        GameManager.RegisterData(this);
        if (PlayManager.IsNewData) { LeftStatPoint = InitialStatPoint; return; }

        SaveData data = PlayManager.CurSaveData;

        LeftStatPoint = data.LeftStatPoint;
        UsedStatPoint = data.UsedStatPoint;

        for (int i = 0; i<ValueDefine.MAX_POWER_SLOT; i++)
        {
            m_powerSlot[i] = data.PowerSlot[i];
        }
        for (int i = 0; i<(int)EPowerName.LAST; i++)
        {
            m_powerObtained[i] = data.PowerObtained[i];
        }
    }
    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        data.LeftStatPoint = LeftStatPoint;
        data.UsedStatPoint = UsedStatPoint;

        for (int i = 0; i<ValueDefine.MAX_POWER_SLOT; i++)
        {
            data.PowerSlot[i] = m_powerSlot[i];
        }
        for (int i = 0; i<(int)EPowerName.LAST; i++)
        {
            data.PowerObtained[i] = m_powerObtained[i];
        }
    }


    public static EStatName String2Stat(string _data)
    {
        return _data switch
        {
            "HEALTH" => EStatName.HEALTH,
            "ENDURE" => EStatName.ENDURE,
            "STRENGTH" => EStatName.STRENGTH,
            "INTELLECT" => EStatName.INTELLECT,
            "RAPID" => EStatName.RAPID,
            "MENTAL" => EStatName.MENTAL,
            _ => EStatName.LAST
        };
    }


    public void SetManager()
    {
        LoadData();
    }
}
