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

    private void FallMove()                         // 기존 점프 방향 + 이후 방향키 입력에 따른 공중 이동
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
