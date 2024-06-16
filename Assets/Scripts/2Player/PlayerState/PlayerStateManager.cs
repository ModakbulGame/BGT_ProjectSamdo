using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerState
{
    IDLE,
    MOVE,
    JUMP,
    FALL,
    ATTACK,
    GUARD,
    Power,
    ROLL,
    THROW,
    HIT,
    DIE,
    LAST
}

public interface IPlayerState
{
    public EPlayerState StateEnum { get; }                  // ���¿� �����ϴ� Enum (�ʿ����� �𸣰���, ���߿� �Ⱦ��� ���ֵ� ��)
    public void ChangeTo(PlayerController _player);         // ���·� ��ȯ
    public void Proceed();                                  // ���� ������ �� Update���� ����
    public void FixedProceed();                             // Fixed Update���� ���� (���� ����)
}

public class PlayerStateManager
{
    private readonly PlayerController m_player;             // �����ڸ� ���� �ִ� �÷��̾� Ŭ����


    public IPlayerState CurState { get; private set; }      // ���� ����
    public void ChangeState(IPlayerState _state) { CurState = _state; CurState.ChangeTo(m_player); }    // ���� ��ȯ


    public PlayerStateManager(PlayerController _player) { m_player = _player; }     // ������
}
