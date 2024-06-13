using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkurrabyScript : MonsterScript
{
    private const float SkurrabyFirePower = 18;

    public override float ObjectHeight => 1;

    [SerializeField]
    private GameObject m_skurrabyExplode;

    public override bool CanPurify => Firing;

    public bool IsFlying { get; private set; }
    private bool Firing { get; set; }

    private Vector2 FlyDirection { get { if (!HasTarget) { return Vector2.zero; }
            return ((CurTarget.Position2 + CurTarget.Velocity2 / 2) - Position2).normalized; } }


    public override void ReleaseToPool()
    {
        FlyDone();
        base.ReleaseToPool();
    }

    public void ResetSkurraby()
    {
        IsFlying = false;
        Firing = false;
        IsSpawned = false;
        IsDead = false;
        CurHP = MaxHP;
    }


    public void SkurrabySpawned(Vector2 _dir, ObjectScript _obj)
    {
        CurTarget = _obj;
        m_rigid.velocity = 5 * new Vector3(_dir.x, 1, _dir.y);
    }
    public override IEnumerator WaitSpawned()
    {
        yield return new WaitForSeconds(1.5f);
        m_aiPath.enabled = true;
        IsSpawned = true;
        if (CurTarget != null) { ChangeState(EMonsterState.APPROACH); }
        else { ChangeState(EMonsterState.IDLE); }
    }

    public void ReadyFire()
    {
        Firing = true;
    }
    public void FireSkurraby()
    {
        if(!HasTarget) { ChangeState(EMonsterState.IDLE); return; }
        Vector2 fireDir = SkurrabyFirePower * FlyDirection;
        m_rigid.velocity = new(fireDir.x, 5, fireDir.y);
        m_anim.SetTrigger("FIRE");
        IsFlying = true;
    }

    public void CheckFlyDone()
    {
        if(!IsFlying) { return; }
        if(IsGrounded) { FlyDone(); }
    }
    private void FlyDone()
    {
        IsFlying = false; Firing = false;
    }


    public void ExplodeSkurraby()
    {
        FlyDone();
        m_skurrabyExplode.SetActive(true);
        m_skurrabyExplode.transform.SetParent(null);
        MonsterSkillScript attack = m_skurrabyExplode.GetComponent<MonsterSkillScript>();
        attack.SetDamage(this, 10, 1);
        attack.SetReturnTransform(transform);
        IsDead = true;
        DestroyMonster();
    }
    public override void StartAttack()
    {
        StopMove();
    }

    private readonly float CollisionRadius = 0.75f;

    private void CheckFlyCollision()
    {
        if(!IsFlying || IsDead) { return; }
        Collider[] colliders = Physics.OverlapSphere(Position + Vector3.up * ObjectHeight / 2, CollisionRadius, ValueDefine.HITTABLE_LAYER);
        foreach (Collider col in colliders)
        {
            IHittable hit = col.GetComponentInParent<IHittable>();
            hit ??= col.GetComponentInChildren<IHittable>();
            if(hit == null || hit.IsMonster) { continue; }
            ExplodeSkurraby();
            return;
        }
    }

    public override void GetHit(HitData _hit)    // ¸ÂÀ½
    {
        if (IsFlying) { _hit.Damage = CurHP; base.GetHit(_hit); }
        else { base.GetHit( _hit); }
    }

    public override void SetStates()
    {
        base.SetStates();
        ReplaceState(EMonsterState.ATTACK, gameObject.AddComponent<SkurrabyAttackState>());
    }

    public override void FixedUpdate()
    {
        CheckFlyCollision();
    }
}
