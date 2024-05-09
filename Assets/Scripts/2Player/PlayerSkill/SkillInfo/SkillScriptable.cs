using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScriptable : ScriptableObject
{
    public uint Idx;
    public ESkillName SkillEnum;
    public string ID;
    public ECastType CastType;
    public ESkillProperty[] SkillProps;
    public string SkillName;
    public float Attack;
    public float Magic;
    public float MoveSpeed;
    public float CastingRange;
    public float HitRadius;
    public float PreDelay;
    public float TotalDelay;
    public float Cooltime;
    public int StaminaCost;
    public StatAdjust StatAdjust;
    public string Description;
    public int Price;

    private ECastType Name2Type(ESkillName _skillName)
    {
        if (_skillName <= ESkillName.SWORD_BASIC || _skillName == ESkillName.SAMPLE1)
            return ECastType.MELEE;
        else if (_skillName <= ESkillName.MELEE_KNOCKBACK)
            return ECastType.MELEE_CC;
        else if (_skillName == ESkillName.SCEPTER_BASIC || _skillName == ESkillName.SAMPLE2)
            return ECastType.RANGED;
        else if (_skillName <= ESkillName.RANGED_KNOCKBACk)
            return ECastType.RANGED_CC;
        else if (_skillName == ESkillName.CREATION || _skillName == ESkillName.SAMPLE3)
            return ECastType.SUMMON;
        else if (_skillName == ESkillName.AROUND)
            return ECastType.AROUND;
        else if (_skillName <= ESkillName.AROUND_KNOCKBACK)
            return ECastType.AROUND_CC;
        else if (_skillName == ESkillName.BUFF)
            return ECastType.BUFF;
        else
            return ECastType.LAST;

    }


    public void SetSkillScriptable(uint _idx, string[] _data)
    {
        Idx =               _idx;
        SkillEnum =         (ESkillName)_idx;
        ID =                _data[(int)ESkillAttribute.ID];
        CastType =          Name2Type(SkillEnum);
        SkillProps =        DataManager.String2Properties(_data[(int)ESkillAttribute.PROPERTY]);
        SkillName =         _data[(int)ESkillAttribute.NAME];
        float.TryParse(     _data[(int)ESkillAttribute.ATTACK],           out Attack);
        float.TryParse(     _data[(int)ESkillAttribute.MAGIC],            out Magic);
        float.TryParse(     _data[(int)ESkillAttribute.MOVE_SPEED],       out MoveSpeed);
        float.TryParse(     _data[(int)ESkillAttribute.CASTING_RANGE],    out CastingRange);
        float.TryParse(     _data[(int)ESkillAttribute.HIT_RADIUS],       out HitRadius);
        float.TryParse(     _data[(int)ESkillAttribute.PRE_DELAY],        out PreDelay);
        float.TryParse(     _data[(int)ESkillAttribute.TOTAL_DELAY],      out TotalDelay);
        float.TryParse(     _data[(int)ESkillAttribute.COOLTIME],         out Cooltime);
        int.TryParse(       _data[(int)ESkillAttribute.STAMINA_COST],     out StaminaCost);
        EAdjType type =     DataManager.String2Adj(_data[(int)ESkillAttribute.ADJ_TYPE]);
        float.TryParse(     _data[(int)ESkillAttribute.ADJ_AMOUNT], out float amount);
        float.TryParse(     _data[(int)ESkillAttribute.ADJ_TIME], out float time);
        StatAdjust =        new(type, amount, time);
        Description =       _data[(int)ESkillAttribute.DESCRIPTION];
        int.TryParse(       _data[(int)ESkillAttribute.PRICE],            out Price);
    }
}
