using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class PlayerSkillScript : ObjectAttackScript
{
    [SerializeField]
    protected SkillScriptable m_scriptable;
    public bool IsScriptableSet { get { return m_scriptable != null; } }
    public void SetScriptable(SkillScriptable _scriptable) { m_scriptable = _scriptable; SetInfo(); }

    [SerializeField]
    private float m_lastTime = 10;

    protected readonly float[] Damages = new float[2];

    private PlayerController Player { get { return (PlayerController)m_attacker; } }

    public override ECCType CCType { get { return SkillManager.IDToCC(m_scriptable.ID); } }

    public virtual void SetSkill(PlayerController _player, float _attack, float _magic)
    {
        m_attacker = _player;
        Damages[0] = _attack;
        Damages[1] = _magic;
    }
    public float ResultDamage { get { return m_scriptable.Attack * Damages[0] + m_scriptable.Magic * Damages[1]; } }



    private void OnTriggerEnter(Collider _other)
    {
        IHittable hittable = _other.GetComponentInParent<IHittable>();
        hittable ??= _other.GetComponentInChildren<IHittable>();
        if(hittable == null) { return; }
        Vector3 point = _other.ClosestPoint(transform.position);
        GiveDamage(hittable, point);
        CollideTaret();
    }
    public override void GiveDamage(IHittable _hittable, Vector3 _point)
    {
        if (CheckHit(_hittable)) { return; }
        HitData hit = new(Player, ResultDamage, _point, CCType);
        _hittable.GetHit(hit);
        AddHitObject(_hittable);
    }

    public virtual void CollideTaret()
    {
        // 물체와 충돌
        Destroy(gameObject);
    }


    private void SetInfo()
    {

    }

    public override void Start()
    {
        AttackOn();
        Destroy(gameObject, m_lastTime);
    }
}
