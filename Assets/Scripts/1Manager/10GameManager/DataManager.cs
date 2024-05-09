using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    private DataList m_dataList;


    // 몬스터 데이터
    public MonsterScriptable GetMonsterData(EMonsterName _monster) { return m_dataList.GetMonsterData(_monster); }

    // 아이템 데이터
    public ItemScriptable GetItemData(SItem _item)
    {
        return _item.Type switch
        {
            EItemType.WEAPON => m_dataList.GetWeaponData((EWeaponName)_item.Idx),
            EItemType.PATTERN => m_dataList.GetPatternData((EPatternName)_item.Idx),
            EItemType.THROW => m_dataList.GetThrowItemData((EThrowItemName)_item.Idx),
            EItemType.OTHERS => m_dataList.GetOtherItemData((EOtherItemName)_item.Idx),

            _ => null
        };
    }

    // 스킬 데이터
    public SkillScriptable GetSkillData(ESkillName _skill) { return m_dataList.GetSkillData(_skill); }



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

            _ => EAdjType.LAST
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