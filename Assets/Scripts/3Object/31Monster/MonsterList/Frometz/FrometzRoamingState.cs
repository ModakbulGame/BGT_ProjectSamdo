using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrometzRoamingState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum { get { return EMonsterState.ROAMING; } }

    private FrometzScript Frometz { get { return (FrometzScript)m_monster; } }


    public void ChangeTo(MonsterScript _monster)
    {
        if(m_monster == null) { m_monster = _monster; }

        Frometz.StartRoaming();
    }

    public void Proceed()
    {
        m_monster.FindTarget();

        if (m_monster.HasTarget)
        {
            m_monster.ChangeState(EMonsterState.ATTACK);
            return;
        }
    }
}
