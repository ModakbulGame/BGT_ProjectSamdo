using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct FRange
{
    public float Min;
    public float Max;
    public float Num { get { return UnityEngine.Random.Range(Min, Max); } }
    public FRange(float _min, float _max) { Min = _min; Max = _max; }
}

public struct BufferObject
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Size;
}

public struct HitData
{
    public ObjectScript Attacker;
    public float Damage;
    public Vector3 Point;
    public ECCType[] CCList;
    public EPowerProperty Property;
    public bool IsNull { get { return Attacker == null; } }
    public static HitData Null { get { return new(null, -1, Vector3.zero); } }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point) : this(_attacker, _damage, _point, ECCType.NONE) { }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, EPowerProperty _prop) : this(_attacker, _damage, _point, ECCType.NONE, _prop) { }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, ECCType _cc) : this(_attacker, _damage, _point, _cc, EPowerProperty.LAST) { }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, ECCType _cc, EPowerProperty _prop)
        : this(_attacker, _damage, _point, new ECCType[] { _cc }, _prop) { }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, ECCType[] _cc) : this(_attacker, _damage, _point, _cc, EPowerProperty.LAST) { }
    public HitData(ObjectScript _attacker, float _damage, Vector3 _point, ECCType[] _cc, EPowerProperty _prop)
    {
        Attacker = _attacker;
        Damage = _damage;
        Point = _point;
        CCList = new ECCType[_cc.Length];
        for (int i = 0; i<_cc.Length; i++) { CCList[i] = _cc[i]; }
        Property = _prop;
    }
}