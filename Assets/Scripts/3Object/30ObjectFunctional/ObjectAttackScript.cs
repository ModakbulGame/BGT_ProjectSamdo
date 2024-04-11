using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class ObjectAttackScript : MonoBehaviour
{
    protected ObjectScript m_attacker;
    public ObjectScript Attacker { get { return m_attacker; } }

    public virtual bool IsAttacking { get; protected set; }     // ���� ��
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

    public virtual void AttackOn()                       // ���� ����
    {
        IsAttacking = true;
    }
    public void AddHitObject(IHittable _object)         // ��Ʈ ���� (�ߺ� ��Ʈ ����)
    {
        m_hitObjects.Add(_object);
        if (m_attacker.IsMonster && _object.IsPlayer)
        {
            if (PlayManager.IsPlayerGuarding)
            {
                ((MonsterScript)m_attacker).HitGuardingPlayer();
                Debug.Log("�����̴�!");
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
    public virtual void AttackOff()                        // ���� �ߴ�
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
