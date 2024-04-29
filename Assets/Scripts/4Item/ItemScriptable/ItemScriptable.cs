using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScriptable : ScriptableObject
{
    public uint     Idx;
    public string   ID;
    public string   ItemName;
    public string   Description;
    public float    DropRate;
    public int      ItemPrice;
    // 테이블이 완성되면 완성될 예정

    public virtual void SetItemScriptable(uint _idx, string[] _data)
    {
        Idx =           _idx;
        ID =            _data[(int)EItemAttribute.ID];
        ItemName =      _data[(int)EItemAttribute.NAME];
        Description =   _data[(int)EItemAttribute.DESCRIPTION];
        float.TryParse( _data[(int)EItemAttribute.DROP_RATE],   out DropRate);
        int.TryParse(   _data[(int)EItemAttribute.PRICE],       out ItemPrice);
    }
}
