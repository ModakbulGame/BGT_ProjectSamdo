using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkurrabyScript : MonsterScript
{
    private const float SkurrabyFirePower = 18;

    [SerializeField]
    private GameObject m_skurrabyExplode;

    public override bool CanPurify => Firing;

    public bool Flying { get; private set; }
    private bool Firing { get; set; }

    private Vector2 FlyDirection { get { if (!HasTarget) { return Vector2.zero; }
            return ((CurTarget.Position2 + CurTarget.Velocity2 / 2) - Position2).normalized; } }


    public void SkurrabySpawned(Vector2 _dir, ObjectScript _obj)
    {
        IsSpawned = false;
        CurTarget = _obj;
        m_rigid.velocity = 5 * new Vector3(_dir.x, 1, _dir.y);
        StartCoroutine(SpawnDelay());
    }
    public override IEnumerator WaitSpawned()
    {
        while (!IsSpawned) { yield return null; }
        m_aiPath.enabled = true;
        if (CurTarget != null) { ChangeState(EMonsterState.APPROACH); }
        else { ChangeState(EMonsterState.ROAMING); }
    }
    private IEnumerator SpawnDelay() { yield return new WaitForSeconds(1.5f); IsSpawned = true; }




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
        Flying = true;
    }

    public void CheckFlyDone()
    {
        if(!Flying) { return; }
        if(m_rigid.velocity.magnitude < 8) { Flying = false; Firing = false; }
    }

    public void ExplodeSkurraby()
    {
        m_skurrabyExplode.SetActive(true);
        m_skurrabyExplode.transform.SetParent(null);
        MonsterSkillScript attack = m_skurrabyExplode.GetComponent<MonsterSkillScript>();
        attack.SetDamage(this, 10, 1);
        attack.SetReturnTransform(transform);
        IsDead = true;
        DestroyMonster();
    }


    private void OnTriggerEnter(Collider _other)
    {
        if(!Flying || IsDead) { return; }
        if (_other.gameObject.layer == ValueDefine.HITTABLE_LAYER_IDX)
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
