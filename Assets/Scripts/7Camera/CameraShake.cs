using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float m_magnitude;
    private CinemachineImpulseSource m_impulseSource;

    // 테스트용
    private void OnTriggerEnter(Collider other)
    {
        CameraShaking(m_magnitude);
    }

    public void CameraShaking(float magnitude)
    {
        m_impulseSource.GenerateImpulse(m_magnitude);
    }

    private void Start()
    {
        m_impulseSource = transform.GetComponent<CinemachineImpulseSource>();
    }
}
