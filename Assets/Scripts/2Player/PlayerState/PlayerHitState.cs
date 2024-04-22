using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.THROW; } }

    private const float HitDelay = 0.667f;
    private float TimeCount { get; set; }

    public void ChangeTo(PlayerController _player)
    {
        if(m_player == null) { m_player = _player; }

        m_player.StopMove();
        m_player.HitAnimation();
        TimeCount = HitDelay;
    }

    public void Proceed()
    {
        TimeCount -= Time.deltaTime;
        if (TimeCount <= 0)
        {
            m_player.ChangeState(EPlayerState.IDLE);
        }
    }

    public void FixedProceed()
    {

    }
}
