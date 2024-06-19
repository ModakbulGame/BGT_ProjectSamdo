using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleBoxScript : ObjectScript, IHittable
{
    public override bool IsPlayer => false;

    public override bool IsMonster => true;

    public override void GetHit(HitData _hit)
    {
        PlayManager.MonsterKilled(EMonsterName.LAST, EMonsterDeathType.BY_PLAYER);
        IsDead = true;
        Destroy(gameObject);
    }

    public override void Awake() { }
    public override void Start() { }
}
