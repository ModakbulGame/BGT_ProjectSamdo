using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private readonly SaveData[] m_gameData = new SaveData[ValueDefine.MAX_SAVE];

    public SaveData[] GameData { get { return m_gameData; } }

    private void LoadGameData()
    {
        for (int i = 0; i<ValueDefine.MAX_SAVE; i++)
        {

        }
    }

    private readonly List<IHaveData> m_dataList = new();
    public void RegisterData(IHaveData _data) 
    {
        m_dataList.Add(_data);
        _data.LoadData();
    }
    public void SaveData(EOasisPointName _oasis)
    {
        PlayManager.CurSaveData.OasisPoint = _oasis;
        foreach (IHaveData data in m_dataList)
        {
            data.SaveData();
        }
    }
    public void ClearData()
    {
        m_dataList.Clear();
    }



    // NPC µ¥ÀÌÅÍ
    //public NPCScriptable GetNPCData(EnpcName _npc) { return m_dataList.GetNPCData(_npc); }


    public static EItemType IDToItemType(string _id)
    {
        return _id[0] switch
        {
            ValueDefine.WEAPON_CODE => EItemType.WEAPON,
            ValueDefine.PATTERN_CODE => EItemType.PATTERN,
            ValueDefine.THROW_ITEM_CODE => EItemType.THROW,
            ValueDefine.OTHER_ITEM_CODE => EItemType.OTHERS,
            _ => EItemType.LAST
        };
    }

    public static EWeaponType IDToWeaponType(string _id)
    {
        return _id[1] switch
        {
            ValueDefine.BLADE_CODE => EWeaponType.BLADE,
            ValueDefine.SWORD_CODE => EWeaponType.SWORD,
            ValueDefine.SCEPTER_CODE => EWeaponType.SCEPTER,
            _ => EWeaponType.LAST
        };
    }

    public static EAdjType String2Adj(string _data)
    {
        return _data switch
        {
            "Damage" => EAdjType.DAMAGE,
            "Attack" => EAdjType.ATTACK,
            "Magic" => EAdjType.MAGIC,
            "MoveSpeed" => EAdjType.MOVE_SPEED,
            "WeaponCC" => EAdjType.WEAPON_CC,

            _ => EAdjType.LAST
        };
    }

    public static ECCType String2CC(string _data)
    {
        return _data switch
        {
            "Fatigue" => ECCType.FATIGUE,
            "Stun" => ECCType.STUN,
            "Melancholy" => ECCType.MELANCHOLY,
            "Extortion" => ECCType.EXTORTION,
            "Airborne" => ECCType.AIRBORNE,
            "Knockback" => ECCType.KNOCKBACK,
            "Weakness" => ECCType.WEAKNESS,
            "Bind" => ECCType.BIND,
            "Void" => ECCType.VOID,
            "Oblivion" => ECCType.OBLIVION,
            "Blind" => ECCType.BLIND,

            _ => ECCType.LAST
        };
    }

    public static ESkillProperty[] String2Properties(string _data)
    {
        if(_data == "") { return new ESkillProperty[0]; }
        string[] datas = _data.Split('/');
        ESkillProperty[] props = new ESkillProperty[datas.Length];
        for (int i = 0; i<datas.Length; i++)
        {
            props[i] = String2Property(datas[i]);
        }
        return props;
    }
    private static ESkillProperty String2Property(string _data)
    {
        return _data switch
        {
            "Slash" => ESkillProperty.SLASH,
            "Hit" => ESkillProperty.HIT,
            "Explosion" => ESkillProperty.EXPLOSION,
            "Shockwave" => ESkillProperty.SHOCKWAVE,
            "Fog" => ESkillProperty.FOG,
            "Totem" => ESkillProperty.TOTEM,
            "Light" => ESkillProperty.LIGHT,
            "Soul" => ESkillProperty.SOUL,

            _ => ESkillProperty.LAST
        };
    }

    public void SetManager()
    {
        LoadGameData();
    }
}