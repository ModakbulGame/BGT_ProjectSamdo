using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class ObjectAttackScript : MonoBehaviour
{
    protected ObjectScript m_attacker;
    public ObjectScript Attacker { get { return m_attacker; } }

    public virtual bool IsAttacking { get; protected set; }     // 공격 중
    protected readonly List<IHittable> m_hitObjects = new();
    public bool CheckHit(IHittable _object) { return m_hitObjects.Contains(_object); }

    [SerializeField]
    protected ECCType m_ccType = ECCType.NONE;

    public float Damage { get; private set; } = 5;
    public virtual ECCType CCType
    {
        get { return m_ccType; }
    }


    public void SetAttack(ObjectScript _attacker, float _damage) { m_attacker = _attacker; Damage = _damage; }

    public virtual void AttackOn()                       // 공격 시작
    {
        IsAttacking = true;
    }
    public void AddHitObject(IHittable _object)         // 히트 판정 (중복 히트 방지)
    {
        m_hitObjects.Add(_object);
        if (m_attacker.IsMonster && _object.IsPlayer)
        {
            if (PlayManager.IsPlayerGuarding)
            {
                ((MonsterScript)m_attacker).HitGuardingPlayer();
                Debug.Log("지금이니!");
            }
        }
    }
    public virtual void GiveDamage(IHittable _hittable, Vector3 _point)
    {
        if (CheckHit(_hittable)) { return; }
        HitData hit = new(Attacker, Damage, _point, CCType);
        _hittable.GetHit(hit);
        AddHitObject(_hittable);
    }
    public virtual void AttackOff()                        // 공격 중단
    {
        IsAttacking = false;
        m_hitObjects.Clear();
    }


    public virtual void Start()
    {
        m_attacker = GetComponentInParent<ObjectScript>();
        AttackOff();
    }
}
