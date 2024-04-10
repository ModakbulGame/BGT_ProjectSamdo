using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRoamingState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState CurMonsterState { get { return EMonsterState.ROAMING; } }

    private bool IsMoving { get; set; }
    private bool IsRotating { get; set; }
    private float TargetRotation { get; set; }
    private float ResetCount { get; set; }


    public void ChangeTo(MonsterScript _monster)
    {
        if (m_monster == null) { m_monster = _monster; }

        m_monster.StartRoaming();
        StartMove();
    }

    private void StartMove()
    {
        Vector3 destination;
        do
        {
            Vector3 offset = new(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            destination = transform.position + offset;
        } while (m_monster.OutOfRange(destination));
        m_monster.SetDestination(destination);
        IsMoving = true;
        IsRotating = false;
    }

    private void RandomRotate()
    {
        IsRotating = true;
        float rot = m_monster.Rotation;
        TargetRotation = rot + Random.Range(-60f, 60f);
        RotateTo();
    }

    private void RotateTo()
    {
        m_monster.SlowRotate(TargetRotation);
    }

    private void PauseRoaming()
    {
        IsMoving = false;
        IsRotating = false;
        ResetCount = Random.Range(3f, 10f);
    }

    public void Proceed()
    {
        m_monster.FindTarget();

        if (m_monster.HasTarget)
        {
            m_monster.ChangeState(EMonsterState.APPROACH);
            return;
        }

/*        if (m_monster.OverRoamingNum)
        {
            m_monster.ChangeState(EMonsterState.IDLE);
            return;
        }*/

        if(!IsMoving) 
        { 
            ResetCount -= Time.deltaTime;
            if(IsRotating) { RotateTo(); }
            if(!IsRotating && ResetCount > 1 && Random.Range(0,1f) < 0.001f) { RandomRotate(); }
        }

        if (IsMoving && m_monster.Arrived)
        {
            PauseRoaming();
        }
        else if (!IsMoving && ResetCount <= 0)
        {
            StartMove();
        }
    }
}