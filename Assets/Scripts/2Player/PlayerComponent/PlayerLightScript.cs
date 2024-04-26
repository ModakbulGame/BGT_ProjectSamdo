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

public class PlayerLightScript : MonoBehaviour            // �ӽ� �Һ� ������Ʈ
{
    private static PlayerLightScript Inst;

    private ParticleSystem[] m_effects;

    private const float MIN_SIZE = 0;                            // �ּ� ũ��
    private const float MAX_SIZE = 100;                          // �ִ� ũ��
    private const float CHANGE_TIME = 5;

    
    public static float CurSize { get; private set; }
    public static ELightState CurState { get; private set; }    // �Һ��� ���� ���� (static)


    public void LightOn()
    {
        m_effects[0].Play();
        CurState = ELightState.CHANGE;
        CurSize = MIN_SIZE;
        StartCoroutine(ChangeSize(true));
    }
    public void LightOff()
    {
        m_effects[1].Play();
        CurState = ELightState.CHANGE;
        CurSize = MAX_SIZE;
        StartCoroutine(ChangeSize(false));
    }

    private IEnumerator ChangeSize(bool _on)
    {
        float change = (MAX_SIZE - MIN_SIZE) / CHANGE_TIME;
        if (!_on) { change *= -1.5f; }
        while ((_on && CurSize < MAX_SIZE) || (!_on && CurSize > MIN_SIZE))
        {
            CurSize += change * Time.deltaTime;
            yield return null;
        }
        if (_on) { CurSize = MAX_SIZE; CurState = ELightState.ON; }
        else { CurSize = MIN_SIZE; CurState = ELightState.OFF; }
    }



    public void SetComps()
    {
        if(Inst != null) { Destroy(Inst.gameObject); }
        Inst = this;
        m_effects = GetComponentsInChildren<ParticleSystem>();
    }

    private void Awake()
    {
        SetComps();
    }
}
