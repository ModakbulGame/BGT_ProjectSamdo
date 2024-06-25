using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class S503 : PlayerPowerScript
{
    public bool IsBuff { get; private set; }

    private float BuffTimeCount { get; set; }

    private bool IsPlayerIn { get; set; }
    private bool PrevPlayerIn { get; set; }

    private PlayerController Player { get; set; }


    [SerializeField]
    private float m_effectRadius = 8;
    [SerializeField]
    private float m_buffTime = 5;
    [SerializeField]
    private float m_buffLastTime = 10;
    [SerializeField]
    private float m_buffGap = 1;


    public void StartBuff()
    {
        IsBuff = true;
    }

    public void EndBuff()
    {
        IsBuff = true;
    }


    private void SetPlayerIn()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, m_effectRadius, ValueDefine.HITTABLE_PLAYER_LAYER);
        foreach (Collider col in cols)
        {
            PlayerController player = col.GetComponentInParent<PlayerController>();
            if (player == null || player.IsDead) { return; }
            Player = player;
            IsPlayerIn = true;
        }



        PrevPlayerIn = IsPlayerIn;
        Player = null;
    }
    private void CheckNBuff()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, m_effectRadius, ValueDefine.HITTABLE_PLAYER_LAYER);
        foreach (Collider col in cols)
        {
            PlayerController player = col.GetComponentInParent<PlayerController>();
            if(player == null || player.IsDead) { return; }
            player.GetAdj(new(EAdjType.MAX_HP, 10, m_buffGap));
            BuffTimeCount = m_buffGap;
            IsPlayerIn = true;
        }
    }

    private void Update()
    {
        SetPlayerIn();
        if (BuffTimeCount <= 0) { CheckNBuff(); }
        else { BuffTimeCount -= Time.deltaTime; }
    }
}
