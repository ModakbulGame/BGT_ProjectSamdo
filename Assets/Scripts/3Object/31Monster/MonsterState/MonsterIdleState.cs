using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterIdleState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState CurMonsterState { get { return EMonsterState.IDLE; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if (m_monster == null) { m_monster = _monster; }
        m_monster.StartIdle();
    }

    public void Proceed()
    {
        m_monster.FindTarget();

        if (m_monster.HasTarget)
        {
            m_monster.ChangeState(EMonsterState.APPROACH);
            return;
        }
        if (m_monster.ShouldRoam)
        {
            m_monster.ChangeState(EMonsterState.ROAMING);
            return;
        }
    }
}