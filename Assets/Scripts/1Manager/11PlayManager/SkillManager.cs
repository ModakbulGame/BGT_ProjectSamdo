using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillInfo
{
    public SkillScriptable SkillData { get; private set; }
    public string SkillName { get { return SkillData.SkillName; } }
    public string SkillDescription { get { return SkillData.Description; } }
    public ECastType CastType { get { return SkillData.CastType; } }
    public ESkillProperty[] SkillProps { get { return SkillData.SkillProps; } }
    public float SkillCooltime { get { return SkillData.Cooltime; } }
    public float SkillRadius { get { return SkillData.HitRadius; } }
    public float SkillCastRange { get{ return SkillData.CastingRange; } } 
    public bool HideWeapon { get { return SkillData.HideWeapon; } }
    public bool ShowCastingEffect { get { return SkillData.ShowCastingEffect; } }
    public bool Obtained { get; private set; }

    public void ObtainSkill() { Obtained = true; }
    public SkillInfo(SkillScriptable _scriptable)
    {
        SkillData = _scriptable;
        Obtained = false;
    }
}

public class SkillManager : MonoBehaviour
{
    private readonly SkillInfo[] m_skillInfo = new SkillInfo[(int)ESkillName.LAST];
    public SkillInfo GetSkillInfo(ESkillName _skill) { return m_skillInfo[(int)_skill]; }

    [SerializeField]
    private SkillScriptable[] m_skillData;

    public GameObject[] SkillArrays { get {
            GameObject[] array = new GameObject[m_skillData.Length];
            for(int i = 0; i<array.Length; i++) { array[i] = m_skillData[i].SkillPrefab; }
            return array; } }


    public SkillScriptable GetSkillData(ESkillName _skill)
    {
        return m_skillData[(int)_skill];
    }
    public GameObject GetSkillObj(ESkillName _skill)
    {
        return PoolManager.GetObject(m_skillData[(int)_skill].SkillPrefab);
    }

    public void SetSkillData(List<SkillScriptable> _data)
    {
        m_skillData = new SkillScriptable[_data.Count];
        for(int i = 0; i<m_skillData.Length; i++) { m_skillData[i] = _data[i]; }
    }


    private readonly ESkillName[] m_skillSlot = new ESkillName[ValueDefine.MAX_SKILL_SLOT]
    { ESkillName.RANGED_PARAB1,ESkillName.RANGED_KNOCKBACK1,ESkillName.BUFF_FATIGUE};              // 임시 설정
    public ESkillName[] SkillSlot { get { return m_skillSlot; } }

    public void RegisterSkillSlot(ESkillName _skill, int _idx) { m_skillSlot[_idx] = _skill; }
    public void ObtainSkill(ESkillName _skill)
    {
        if (m_skillInfo[(int)_skill].Obtained) { Debug.Log("이미 획득한 스킬"); return; }
        m_skillInfo[(int)_skill].ObtainSkill();
    }
    

    public static ECCType IDToCC(string _code)
    {
        char c2 = _code[1];
        if(c2 != ValueDefine.MELEE_CC_CODE && c2 != ValueDefine.RANGED_CC_CODE && c2 != ValueDefine.AROUND_CC_CODE)
        { return ECCType.NONE; }
        return (ECCType)(_code[2] - '0');
    }



    public void SetManager()
    {
        for (int i = 0; i<(int)ESkillName.LAST; i++)
        {
            m_skillInfo[i] = new SkillInfo(GameManager.GetSkillData((ESkillName)i));
            if (i < (int)ESkillName.LAST) { m_skillInfo[i].ObtainSkill(); }             // 임시 설정
        }
    }
}
