using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AroundSkillScript: PlayerSkillScript
{
    [SerializeField]
    private float m_damgeTimeGet = 0.5f;
    private Dictionary<IHittable, float> m_hitTimeCount = new();

    public override void GiveDamage(IHittable _hittable, Vector3 _point)
    {
        base.GiveDamage(_hittable, _point);
        if (m_hitTimeCount.ContainsKey(_hittable)) 
            return;
        m_hitTimeCount[_hittable] = m_damgeTimeGet;
    }

    private void OnTriggerStay(Collider _other)
    {
        CheckSkillTrigger(_other);
    }

    private void CheckTime()
    {
        List<IHittable> removeList = new();
        foreach(var key in m_hitTimeCount.Keys.ToList())
        {
            float hitTime = m_hitTimeCount[key];
            hitTime -= Time.deltaTime;
            m_hitTimeCount[key] = hitTime;
            if (hitTime <= 0)
            {
                removeList.Add(key);
            }
        }
        for (int i = 0; i<removeList.Count; i++)
        {
            m_hitObjects.Remove(removeList[i]);
            m_hitTimeCount.Remove(removeList[i]);
        }
    }

    public override void ReleaseTopool()
    {
        base.ReleaseTopool();
        m_hitTimeCount.Clear();
    }

    private void Update()
    {
        CheckTime();
    }
}
