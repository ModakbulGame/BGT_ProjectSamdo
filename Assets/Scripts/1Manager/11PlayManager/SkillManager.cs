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
    private GameObject[] m_skillPrefabs = new GameObject[(int)ESkillName.LAST];

    public GameObject[] SkillArrays { get { return m_skillPrefabs; } }


    public GameObject GetSkillObj(ESkillName _skill)
    {
        return PoolManager.GetObject(m_skillPrefabs[(int)_skill]);
    }


    private readonly ESkillName[] m_skillSlot = new ESkillName[ValueDefine.MAX_SKILL_SLOT]
    { ESkillName.BLADE_BASIC,ESkillName.BUFF,ESkillName.WEAPON_POISON};              // 임시 설정
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
        return _code[2] switch
        {
            ValueDefine.SLOW_CODE => ECCType.SLOW,
            ValueDefine.STUN_CODE => ECCType.STUN,
            ValueDefine.POISON_CODE => ECCType.POISON,
            ValueDefine.BLEED_CODE => ECCType.BLEED,
            ValueDefine.STAGGER_CODE => ECCType.STAGGER,
            ValueDefine.AIRBORNE_CODE => ECCType.AIRBORNE,
            ValueDefine.KNOCKBACK_CODE => ECCType.KNOCKBACK,
            _ => ECCType.NONE
        };
    }



    public void SetManager()
    {
        for (int i = 0; i<(int)ESkillName.LAST; i++)
        {
            m_skillInfo[i] = new SkillInfo(GameManager.GetSkillRawData((ESkillName)i));
            if (i < (int)ESkillName.LAST) { m_skillInfo[i].ObtainSkill(); }             // 임시 설정
        }
    }
}
