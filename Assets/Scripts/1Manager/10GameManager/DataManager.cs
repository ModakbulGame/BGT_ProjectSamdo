using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    private DataList m_dataList;


    // ���� ������
    public MonsterScriptable GetMonsterData(EMonsterName _monster) { return m_dataList.GetMonsterData(_monster); }

    // ������ ������
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

    // ��ų ������
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

    public void SetManager()
    {

    }
}