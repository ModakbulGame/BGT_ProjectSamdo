using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrogantAttackScript : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;

    public EMonsterState StateEnum => EMonsterState.ATTACK;

    private ArrogantScript Arrogant { get { return (ArrogantScript)m_monster; } }

    private EArrogantAttack CurAttack { get { return Arrogant.CurAttack; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if(Arrogant == null) { m_monster = _monster; }

        Arrogant.StartAttack();
    }

    public void Proceed()
    {
        if (CurAttack == EArrogantAttack.RIGHT_SWING)
        {
            if (!Arrogant.HasTarget)
            {
                Arrogant.ChangeState(EMonsterState.IDLE);
                return;
            }

            Arrogant.LookTarget();
        }
        else if (CurAttack == EArrogantAttack.SMASH)
        {

        }
    }
}
