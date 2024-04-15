using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponInfo
{
    public EWeaponType WeaponType;
    public EHitType HitType;
    public string WeaponName;
    public FRange Attack;
    public FRange Magic;
    public float AttackSpeed;
    public void SetInfo(WeaponScriptable _scriptable)
    {
        WeaponType = DataManager.IDToWeaponType(_scriptable.ID);
        if(WeaponType == EWeaponType.SCEPTER) { HitType = EHitType.BLOW; }
        else { HitType = EHitType.SLASH; }
        WeaponName = _scriptable.ItemName;
        Attack = _scriptable.Attack;
        Magic = _scriptable.Magic;
        AttackSpeed = _scriptable.AttackSpeed;
    }
}

public class WeaponScript : AnimateAttackScript
{
    [SerializeField]
    private WeaponInfo m_weaponInfo = new();
    public EWeaponType WeaponType { get { return m_weaponInfo.WeaponType; } }
    public EHitType HitType { get { return m_weaponInfo.HitType; } }
    public string WeaponName { get { return m_weaponInfo.WeaponName; } }
    public FRange WeaponAttack { get { return m_weaponInfo.Attack; } }
    public FRange WeaponMagic { get { return m_weaponInfo.Magic; } }
    public float WeaponAttackSpeed { get { return m_weaponInfo.AttackSpeed; } }
    public void SetWeaponInfo(WeaponScriptable _scriptable) { m_weaponInfo.SetInfo(_scriptable); }


    [SerializeField]
    private WeaponScriptable m_scriptable;      // 정보
    public bool IsScriptableSet { get { return m_scriptable != null; } }
    public EWeaponName WeaponEnum { get { return (EWeaponName)m_scriptable.Idx; } }

    private PlayerController Player { get { return (PlayerController)m_attacker; } }

    public override float Damage => Player.Attack;


    public override void CreateHitEffect(IHittable _hittable, Vector3 _pos)
    {
        if (_hittable.IsMonster && _pos != Vector3.zero)
        {
            EEffectName effectName = HitType == EHitType.SLASH ? EEffectName.HIT_SLASH : EEffectName.HIT_BLOW;
            GameObject effect = GameManager.GetEffectObj(effectName);
            effect.transform.position = _pos;
        }
    }


    public void SetScriptable(WeaponScriptable _scriptable)
    { 
        m_scriptable = _scriptable; 
        SetInfo();
    }
    private void SetInfo()
    {
        m_weaponInfo.SetInfo(m_scriptable);
    }
}
