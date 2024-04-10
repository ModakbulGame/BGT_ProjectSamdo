using Pathfinding.RVO;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using static UnityEngine.Rendering.DebugUI;

public enum ELightState
{
    OFF,
    CHANGE,
    ON
}

public class PlayerLightScript : MonoBehaviour            // 임시 불빛 오브젝트
{
    private static PlayerLightScript Inst;

    private ParticleSystem m_effect;

    private const float MinSize = 0;                            // 최소 크기
    private const float MaxSize = 100;                          // 최대 크기

    private static Particle[] m_particles;

    public static float CurSize { get { if (!Inst.gameObject.activeSelf) { return 0; }
            Inst.m_effect.GetParticles(m_particles);
            float max = 0;
            for(int i = 0; i<m_particles.Length;i++) { float size = m_particles[i].GetCurrentSize(Inst.m_effect); if (size > max) max = size; }
            return max;
        } }

    public static ELightState CurState { get; private set; }    // 불빛의 현재 상태 (static)


    public void LightOn()
    {
        m_effect.Play();
        CurState = ELightState.CHANGE;
        StartCoroutine(ChangeSize(true));
    }
    public void LightOff()
    {
        CurState = ELightState.CHANGE;
        StartCoroutine(ChangeSize(true));
    }

    private IEnumerator ChangeSize(bool _on)
    {
        while ((_on && CurSize < 90) || (!_on && CurSize > 20))
        {
            yield return null;
        }
        if (_on) { CurState = ELightState.ON; }
        else { CurState = ELightState.OFF; }
    }



    public void SetComps()
    {
        Inst = this;
        m_effect = GetComponent<ParticleSystem>();
        m_particles = new Particle[m_effect.main.maxParticles];
    }

    private void Awake()
    {
        SetComps();
    }

    private void Update()
    {
        Debug.Log(CurSize);
    }
}
