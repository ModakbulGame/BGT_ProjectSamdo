using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class MonsterScript
{
    // ��ã��
    protected AIPath m_aiPath;
    public bool Arrived { get { return !m_aiPath.pathPending && m_aiPath.hasPath && m_aiPath.reachedEndOfPath; } }


    // Ȱ�� ����
    [SerializeField]
    private MonsterSpawnPoint m_spawnPoint;         // Ȱ�� ������
    public void SetSpawnPoint(MonsterSpawnPoint _point) { m_spawnPoint = _point; _point.AddMonster(this); }     // ������ ����
    public bool HasPoint { get { return m_spawnPoint != null; } }                       // ������ ���� ����
    public Vector3 SpawnPosition { get { if (!HasPoint) { return new(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity); } return m_spawnPoint.SpawnPosition; } }
    public float SpawnFenceRange { get { if (!HasPoint) { return -1; } return m_spawnPoint.FenceRange; } }
    public bool OutOfRange(Vector3 _position) { if(!HasPoint) { return false; } return Vector3.Distance(SpawnPosition, _position) > SpawnFenceRange; }


    // �⺻ ������
    public override float CurSpeed { get { return m_aiPath.maxSpeed; } protected set { m_aiPath.maxSpeed = value; } }

    public virtual void SetDestination(Vector3 _destination)
    {
        m_aiPath.destination = _destination;
        m_aiPath.SearchPath();
    }

    public override void StopMove()                  // ������ �ʱ�ȭ
    {
        base.StopMove();
        m_aiPath.maxSpeed = 0;
        m_aiPath.destination = Vector3.positiveInfinity;
    }

    public virtual void LookTarget()
    {
        if(CurTarget == null) { return; }

        Vector2 dir = (CurTarget.Position2 - Position2);
        RotateTo(dir);
    }
}
