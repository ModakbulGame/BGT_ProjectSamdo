using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScriptable : ScriptableObject
{
    public uint         Idx;
    public string       ID;
    public string       ItemName;
    public string       Description;
    public float        DropRate;
    public int          ItemPrice;
    public GameObject   ItemPrefab;
    public Sprite       ItemIcon;

    public virtual void SetItemScriptable(uint _idx, string[] _data, GameObject _prefab)
    {
        Idx =           _idx;
        ID =            _data[(int)EItemAttribute.ID];
        ItemName =      _data[(int)EItemAttribute.NAME];
        Description =   _data[(int)EItemAttribute.DESCRIPTION];
        float.TryParse( _data[(int)EItemAttribute.DROP_RATE],   out DropRate);
        int.TryParse(   _data[(int)EItemAttribute.PRICE],       out ItemPrice);
        ItemPrefab =    _prefab;
    }
}
