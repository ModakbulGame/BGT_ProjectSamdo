using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScriptable : ScriptableObject
{
    public uint             Idx;
    public EMonsterName     MonsterEnum;
    public string           ID;
    public EMonsterType     MonsterType;
    public string           MonsterName;
    public int              MaxHP;
    public float            Attack;
    public float            MoveSpeed;
    public float            ApproachSpeed;
    public float            ViewAngle;
    public float            ViewRange;
    public float            EngageRange;
    public float            ReturnRange;
    public float            AttackRange;
    public float            AttackSpeed;
    public float            ApproachDelay;
    public float            FenceRange;
    public string           Description;
    public DropInfo         DropInfo;

    private EMonsterType String2Type(string _data)
    {
        return _data switch
        {
            "NORMAL" => EMonsterType.NORMAL,
            "ELITE" => EMonsterType.ELITE,
            "BOSS" => EMonsterType.BOSS,

            _ => EMonsterType.LAST
        };
    }

    public void SetMonsterScriptable(uint _idx, string[] _data, DropInfo _drop)
    {
        Idx =           _idx;
        MonsterEnum =   (EMonsterName)_idx;
        ID =            _data[(int)EMonsterAttribue.ID];
        MonsterType =   String2Type(_data[(int)EMonsterAttribue.TYPE]);
        MonsterName =   _data[(int)EMonsterAttribue.NAME];
        int.TryParse(   _data[(int)EMonsterAttribue.MAX_HP],            out MaxHP);
        float.TryParse( _data[(int)EMonsterAttribue.DAMAGE],            out Attack);
        float.TryParse( _data[(int)EMonsterAttribue.ROAMING_SPEED],     out MoveSpeed);
        float.TryParse( _data[(int)EMonsterAttribue.APPROACH_SPEED],    out ApproachSpeed);
        float.TryParse( _data[(int)EMonsterAttribue.VIEW_ANGLE],        out ViewAngle);
        float.TryParse( _data[(int)EMonsterAttribue.VIEW_RANGE],        out ViewRange);
        float.TryParse( _data[(int)EMonsterAttribue.ENGAGE_RANGE],      out EngageRange);
        float.TryParse( _data[(int)EMonsterAttribue.RETURN_RANGE],      out ReturnRange);
        float.TryParse( _data[(int)EMonsterAttribue.ATTACK_RANGE],      out AttackRange);
        float.TryParse( _data[(int)EMonsterAttribue.ATTACK_SPEED],      out AttackSpeed);
        float.TryParse( _data[(int)EMonsterAttribue.APPROACH_DELAY],    out ApproachDelay);
        float.TryParse( _data[(int)EMonsterAttribue.FENCE_RANGE],       out FenceRange);
        Description =   _data[(int)EMonsterAttribue.DESCRIPTION];

        DropInfo =      _drop;
    }
}
