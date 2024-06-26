using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSummonPower : SummonPowerScript
{
    private float AdjustTimeCount { get; set; }
    private bool IsAdjusted { get { return AdjustTimeCount > 0; } }


    [SerializeField]
    private EAdjType m_adjustType = EAdjType.DAMAGE;
    [SerializeField]
    private float m_adjustAmount = 1;
    [SerializeField]
    private float m_adjustLastTime = 5;
    [SerializeField]
    private float m_adjustResetTime = 1;

    private PlayerController CurPlayer { get; set; }

    public override void OnTriggerStay(Collider _other)
    {
        if(!IsAttacking || IsAdjusted) { return; }
        PlayerController player = _other.GetComponentInParent<PlayerController>();
        if (player == null || player.IsDead) { return; }
        PlayerAdjustStart(player);
    }

    private void OnTriggerExit(Collider _other)
    {
        PlayerController player = _other.GetComponentInParent<PlayerController>();
        if (player == null || player.IsDead) { return; }
        PlayerAdjustDone(player);
    }


    private void PlayerAdjustStart(PlayerController _player)
    {
        if (CurPlayer == null) { CurPlayer = _player; }
        if (_player.CheckAdjusted(this)) { _player.ModifyAdjust(this, m_adjustLastTime); }
        else { _player.GetAdjust(new(m_adjustType, m_adjustAmount, m_adjustLastTime), this); }
        AdjustTimeCount = m_adjustResetTime;
    }

    private void PlayerAdjustDone(PlayerController _player)
    {
        _player.ModifyAdjust(this, m_adjustLastTime);
        CurPlayer = null;
    }

    public override void ReleaseToPool()
    {
        if (CurPlayer != null) { PlayerAdjustDone(CurPlayer); }
        base.ReleaseToPool();
    }


    public override void PowerCreated()
    {
        base.PowerCreated();
        AdjustTimeCount = 0;
    }


    private void Update()
    {
        if (IsAdjusted) { AdjustTimeCount -= Time.deltaTime; }
    }

}
