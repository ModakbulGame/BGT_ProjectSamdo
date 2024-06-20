using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum EUumAttackType
{
    NORMAL1 = 0,
    NORMAL2 = 2,

    NORMAL3 = 5,

    SKILL2 = 6,
    SKILL1 = 7,

    LAST
}

public class UumScript : AnimatedAttackMonster
{
    public override bool CanPurify => SkillTimeCount[0] > 0 || SkillTimeCount[1] > 0;

    [SerializeField]
    private VisualEffect m_headFire;

    private readonly float PurifyTime = 5;

    public override int SkillNum => 2;

    private readonly float NarrowAttackMultiplier = 1.5f;

    private readonly List<ObjectAttackScript> AttackObjects = new();

    public override void StartAttack()
    {
        AttackIdx = Random.Range(0, 5);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        AttackObjects.Clear();
        base.StartAttack();
    }
    public override void CreateAttack()
    {
        EUumAttackType type = EUumAttackType.LAST;
        if(AttackIdx == 2) { type = EUumAttackType.NORMAL3; }
        else if(AttackIdx == 3) { type = EUumAttackType.SKILL2; }
        else if (AttackIdx == 4) { type = EUumAttackType.SKILL1; }

        float attack = Attack;
        if(type == EUumAttackType.NORMAL3) { attack *= NarrowAttackMultiplier; }
        AttackObject = m_normalAttacks[(int)type].GetComponent<ObjectAttackScript>();
        AttackObject.SetAttack(this, attack);
        AttackObject.AttackOn();
    }
    public override void AttackTriggerOn()
    {
        switch (AttackIdx)
        {
            case 0:
            case 2:
            case 3:
                for(int i=(int)EUumAttackType.NORMAL1;i<=(int)EUumAttackType.NORMAL2;i++)
                {
                    AttackObjects.Add(m_normalAttacks[i].GetComponent<ObjectAttackScript>());
                    AttackObjects[i].SetAttack(this, Attack);
                    if(AttackIdx == 3) { AttackObjects[i].SetCCType(ECCType.KNOCKBACK); }
                }
                break;
            case 1:
            case 4:
                for (int i = (int)EUumAttackType.NORMAL2; i<(int)EUumAttackType.NORMAL3; i++)
                {
                    int idx = i - (int)EUumAttackType.NORMAL2;
                    AttackObjects.Add(m_normalAttacks[i].GetComponent<ObjectAttackScript>());
                    AttackObjects[idx].SetAttack(this, Attack);
                    if (AttackIdx == 4) { AttackObjects[idx].SetCCType(ECCType.KNOCKBACK); }
                }
                break;
        }
        foreach (ObjectAttackScript attack in AttackObjects)
        {
            attack.AttackOn();
        }
    }
    public override void AttackTriggerOff()
    {
        foreach (ObjectAttackScript attack in AttackObjects)
        {
            attack.AttackOff();
        }
        AttackObject?.AttackOff();
    }
    public override void AttackDone()
    {
        foreach (ObjectAttackScript attack in AttackObjects)
        {
            attack.ResetCCType();
        }
        base.AttackDone();
        if(AttackIdx == 3 || AttackIdx == 4) { SkillTimeCount[AttackIdx - 3] = PurifyTime; }
    }
    public override void LookTarget()
    {
        if(AttackIdx == 3 || AttackIdx == 4) { return; }
        base.LookTarget();
    }


    public override void StartDissolve()
    {
        base.StartDissolve();
        m_headFire.Stop();
    }

    public override void ProcCooltime()
    {
        base.ProcCooltime();
        if (SkillTimeCount[0] > 0) { SkillTimeCount[0] -= Time.deltaTime; }
        if (SkillTimeCount[1] > 0) { SkillTimeCount[1] -= Time.deltaTime; }
    }
}
