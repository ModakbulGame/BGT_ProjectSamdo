using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : MonoBehaviour, IPlayerState
{
    private PlayerController m_player;
    public EPlayerState StateEnum { get { return EPlayerState.ROLL; } }

    private Vector2 RollDirection { get; set; }
    private float TimeCount { get; set; }

    public void ChangeTo(PlayerController _player)
    {
        if(m_player == null) { m_player = _player; }

        RollDirection = m_player.JumpRollDirection;
        if(RollDirection == Vector2.zero) { RollDirection = Vector2.up; }

        m_player.RollAction();
        TimeCount = PlayerController.ROLLING_TIME;
    }

    public void Proceed()
    {
        TimeCount -= Time.deltaTime;
        if(TimeCount <= 0) { m_player.JumpRollDone(); return; }
    }

    public void FixedProceed()
    {
        m_player.GroundMove(RollDirection, PlayerController.ROLL_MULTIPLIER);
        m_player.RotateDirection(RollDirection);
    }
}
