using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkurrabyScript : MonsterScript
{
    private const float SkurrabyFirePower = 18;

    [SerializeField]
    private GameObject m_skurrabyExplode;

    public bool Flying { get; private set; }

    private Vector2 FlyDirection { get { if (!HasTarget) { return Vector2.zero; }
            return ((CurTarget.Position2 + CurTarget.Velocity2 / 2) - Position2).normalized; } }

    public void FireSkurraby()
    {
        if(!HasTarget) { ChangeState(EMonsterState.IDLE); return; }
        Vector2 fireDir = SkurrabyFirePower * FlyDirection;
        m_rigid.velocity = new(fireDir.x, 5, fireDir.y);
        m_anim.SetTrigger("FIRE");
        Flying = true;
    }

    public void CheckFlyDone()
    {
        if(!Flying) { return; }
        if(m_rigid.velocity.magnitude < 8) { Flying = false; }
    }

    public void ExplodeSkurraby()
    {
        GameObject explode = Instantiate(m_skurrabyExplode, transform.position, Quaternion.identity);
        MonsterSkillScript attack = explode.GetComponent<MonsterSkillScript>();
        attack.SetDamage(this, 10, 1);
        IsDead = true;
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision _collision)
    {
        if(!Flying) { return; }
        if (_collision.gameObject.CompareTag("Player"))
        {
            ExplodeSkurraby();
        }
    }
    public override void GetHit(HitData _hit)    // ¸ÂÀ½
    {
        if (Flying) { _hit.Damage = CurHP; base.GetHit(_hit); }
        else { base.GetHit( _hit); }
    }

    public override void SetStates()
    {
        base.SetStates();
        ReplaceState(EMonsterState.ATTACK, gameObject.AddComponent<SkurrabyAttackState>());
    }
}
