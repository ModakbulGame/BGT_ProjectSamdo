using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DataManager : MonoBehaviour
{
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
            "DAMAGE" => EAdjType.DAMAGE,
            "ATTACK" => EAdjType.ATTACK,
            "MAGIC" => EAdjType.MAGIC,
            "MOVE_SPEED" => EAdjType.MOVE_SPEED,
            "WEAPON_CC" => EAdjType.WEAPON_CC,

            _ => EAdjType.LAST
        };
    }

    public static ECCType String2CC(string _data)
    {
        return _data switch
        {
            "FATIGUE" => ECCType.FATIGUE,
            "STUN" => ECCType.STUN,
            "MELANCHOLY" => ECCType.MELANCHOLY,
            "EXTORTION" => ECCType.EXTORTION,
            "AIRBORNE" => ECCType.AIRBORNE,
            "KNOCKBACK" => ECCType.KNOCKBACK,
            "WEAKNESS" => ECCType.WEAKNESS,
            "BIND" => ECCType.BIND,
            "VOID" => ECCType.VOID,
            "OBLIVION" => ECCType.OBLIVION,
            "BLIND" => ECCType.BLIND,

            _ => ECCType.LAST
        };
    }

    public static ESkillProperty[] String2Properties(string _data)
    {
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
            "SLASH" => ESkillProperty.SLASH,
            "HIT" => ESkillProperty.HIT,
            "EXPLOSION" => ESkillProperty.EXPLOSION,
            "SHOCKWAVE" => ESkillProperty.SHOCKWAVE,
            "FOG" => ESkillProperty.FOG,
            "TOTEM" => ESkillProperty.TOTEM,
            "LIGHT" => ESkillProperty.LIGHT,
            "SOUL" => ESkillProperty.SOUL,

            _ => ESkillProperty.LAST
        };
    }

    public void SetManager()
    {

    }
}