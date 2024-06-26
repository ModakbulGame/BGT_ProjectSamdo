using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S102 : MeleePowerScript
{
    public override void SetPower(PlayerController _player, float _attack, float _magic)
    {
        base.SetPower(_player, _attack, _magic);
        PowerEffect.EffectOn(transform, m_lastTime);
    }
}
