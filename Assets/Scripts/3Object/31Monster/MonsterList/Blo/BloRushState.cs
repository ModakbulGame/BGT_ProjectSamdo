using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloRushState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;

    public EMonsterState CurMonsterState => EMonsterState.ATTACK;

    private BloScript Blo { get { return (BloScript)m_monster; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if(m_monster == null) { m_monster = _monster; }

        Blo.StartRush();
    }

    private void RushToTarget()
    {
        Blo.RushToTarget();
    }

    public void Proceed()
    {
        RushToTarget();
    }
}
