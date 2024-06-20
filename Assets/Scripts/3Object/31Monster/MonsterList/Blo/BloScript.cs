using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloScript : MonsterScript
{
    public override bool CanPurify => !AbsorbedSoul;

    protected bool AbsorbedSoul { get; set; }

    private MonsterSkillScript SkillObj { get { return (MonsterSkillScript)SkillList[0]; } }

    [SerializeField]
    private float m_rushSpeed = 6;
    public virtual int AbsorbAmount { get { return 2; } }
    [SerializeField]
    private float m_rushDamage = 10;

    private bool RushDone { get; set; }

    private BloRushState m_rushState;

    public bool IsRushing { get; private set; }

    public override void ApproachTarget()   // 공격 시작
    {
        if (!RushDone) { RushBlo(); return; }
        base.ApproachTarget();
    }
    private void RushBlo()
    {
        StopMove();
        m_stateManager.ChangeState(m_rushState);
    }

    public override void CreateAttack()
    {
        base.AttackTriggerOn(0);
    }
    public override void AttackDone()
    {
        m_anim.SetBool("IS_RUSHING", false);
        if (!RushDone) { RushDone = true; }
        base.AttackDone();
    }
    public void SetRush(bool _start)
    {
        m_anim.SetBool("IS_RUSHING", _start);
        if (!_start) { AttackDone(); AttackTriggerOff(); }
        IsRushing = _start;
    }
    public void RushToTarget()
    {
        Vector3 dir = transform.forward;
        Vector3 rot = (dir + (CurTarget.Position - Position).normalized * 0.1f).normalized;
        Vector2 rot2 = new(rot.x, rot.z);
        RotateTo(rot2);
        m_rigid.velocity = m_rushSpeed * dir.normalized;
    }


    public override void AttackTriggerOn()
    {
        if (IsRushing)
        {
            SkillObj.gameObject.SetActive(true);
            SkillObj.SetDamage(this, m_rushDamage, -1);
            SkillObj.AttackOn();
        }
        else
        {
            base.AttackTriggerOn();
        }
    }
    public override void AttackTriggerOff()
    {
        if (IsRushing)
        {
            SkillObj.gameObject.SetActive(false);
            SkillObj.AttackOff();
        }
        else
        {
            base.AttackTriggerOff();
        }
    }


    public override void AttackedPlayer(HitData _hit)
    {
        if (!IsRushing) { return; }
        if (!AbsorbedSoul) { AbsorbSoul(); }
        SetRush(false);
    }
    private void AbsorbSoul()
    {
        int soul = PlayManager.SoulNum;
        int absorb = FunctionDefine.Min(AbsorbAmount, soul);
        PlayManager.LooseSoul(absorb, true);
        AbsorbedSoul = true;
    }


    public override void OnSpawned()
    {
        base.OnSpawned();
        RushDone = false;
        AbsorbedSoul = false;
    }

    public override void SetStates()
    {
        base.SetStates();
        m_rushState = gameObject.AddComponent<BloRushState>();
    }
}
