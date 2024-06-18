using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataList : MonoBehaviour
{
    [SerializeField]
    private WeaponScriptable[] m_weapons = new WeaponScriptable[(int)EWeaponName.LAST];
    [SerializeField]
    private PatternScriptable[] m_patterns = new PatternScriptable[(int)EPatternName.LAST];
    [SerializeField]
    private ThrowItemScriptable[] m_throwItems = new ThrowItemScriptable[(int)EThrowItemName.LAST];
    [SerializeField]
    private OtherItemScriptable[] m_otherItems = new OtherItemScriptable[(int)EOtherItemName.LAST];
    [SerializeField]
    private NPCScriptable[] m_npcs = new NPCScriptable[(int)EnpcEnum.LAST];

    public WeaponScriptable GetWeaponData(EWeaponName _weapon) { return m_weapons[(int)_weapon]; }
    public PatternScriptable GetPatternData(EPatternName _pattern) { return m_patterns[(int)_pattern]; }
    public ThrowItemScriptable GetThrowItemData(EThrowItemName _throw) { return m_throwItems[(int)_throw]; }
    public OtherItemScriptable GetOtherItemData(EOtherItemName _other) { return m_otherItems[(int)_other]; }
    public NPCScriptable GetNPCData(EnpcEnum _npc) { return m_npcs[(int)_npc]; }


    public void SetItemData(List<ItemScriptable>[] _data)
    {
        SetWeaponData(_data[0].ToArray());
        SetPatternData(_data[1].ToArray());
        SetThrowItemData(_data[2].ToArray());
        SetOtherItemData(_data[3].ToArray());
    }
    private void SetWeaponData(ItemScriptable[] _data) { m_weapons = new WeaponScriptable[_data.Length]; for (int i = 0; i<(_data.Length); i++) { m_weapons[i] = (WeaponScriptable)_data[i]; } }
    private void SetPatternData(ItemScriptable[] _data) { m_patterns = new PatternScriptable[_data.Length]; for (int i = 0; i<(_data.Length); i++) { m_patterns[i] = (PatternScriptable)_data[i]; } }
    private void SetThrowItemData(ItemScriptable[] _data) { m_throwItems = new ThrowItemScriptable[_data.Length]; for (int i = 0; i<(_data.Length); i++) { m_throwItems[i] = (ThrowItemScriptable)_data[i]; } }
    private void SetOtherItemData(ItemScriptable[] _data) { m_otherItems = new OtherItemScriptable[_data.Length]; for (int i = 0; i<(_data.Length); i++) { m_otherItems[i] = (OtherItemScriptable)_data[i]; } }
    public void SetNPCData(NPCScriptable[] _data) { m_npcs = new NPCScriptable[_data.Length]; for (int i = 0; i < (_data.Length); i++) { m_npcs[i] = _data[i]; } }
}
