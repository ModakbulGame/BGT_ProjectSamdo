using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarmulakAttackScript : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;

    public EMonsterState StateEnum => EMonsterState.ATTACK;

    private MarmulakScript Marmulak { get { return (MarmulakScript)m_monster; } }

    private EMarmulakAttack CurAttack { get { return Marmulak.CurAttack; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if(Marmulak == null) { m_monster = _monster; }

        Marmulak.StartAttack();
        
        if (CurAttack == EMarmulakAttack.ROAR)
        {

        }
    }

    public void Proceed()
    {
        if (CurAttack == EMarmulakAttack.NORMAL)
        {
            if (!Marmulak.HasTarget)
            {
                Marmulak.ChangeState(EMonsterState.IDLE);
                return;
            }

            Marmulak.LookTarget();
        }
        else if (CurAttack == EMarmulakAttack.ROAR)
        {

        }
    }
}
