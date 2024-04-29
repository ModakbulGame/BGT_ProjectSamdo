using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.FALL; } }

    public void ChangeTo(PlayerController _player)
    {
        if (m_player == null) { m_player = _player; }


    }

    private void FallMove()                         // ���� ���� ���� + ���� ����Ű �Է¿� ���� ���� �̵�
    {

    }

    public void Proceed()
    {

    }

    public void FixedProceed()
    {
        FallMove();
    }
}
