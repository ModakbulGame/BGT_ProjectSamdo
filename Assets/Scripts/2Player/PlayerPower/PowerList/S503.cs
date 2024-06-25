using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S503 : PlayerPowerScript
{
    [SerializeField]
    private float m_buffTimeGet;
    private Dictionary<IHittable, float> m_buffTimeCount = new();

    private void GiveBuff(PlayerController _player)
    {
        if (m_buffTimeCount.ContainsKey(_player))
            return;
        m_buffTimeCount[_player] = m_buffTimeGet;
    }
}
