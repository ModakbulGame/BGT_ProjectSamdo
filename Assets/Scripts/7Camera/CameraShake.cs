using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float m_magnitude;
    private CinemachineImpulseSource m_impulseSource;

    // �׽�Ʈ��
    private void OnTriggerEnter(Collider other)
    {
        CameraShaking(m_magnitude);
    }

    public void CameraShaking(float magnitude)
    {
        m_impulseSource.GenerateImpulse(m_magnitude);
    }

    public void SetImpulseInfo()
    {
        m_impulseSource.m_ImpulseDefinition = new CinemachineImpulseDefinition(); // ImpulseDefinition �ʱ�ȭ
        Debug.Log("ImpulseDefinition set: " + (m_impulseSource.m_ImpulseDefinition != null));

        m_impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;

        m_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = 0.05f;    // ���� �ð�
        m_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 0.15f;   // ���� �ð�
        m_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = 0.3f;      // ���� �ð�

        m_impulseSource.m_DefaultVelocity = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void Start()
    {
        // m_impulseSource = transform.GetComponent<CinemachineImpulseSource>();
        m_impulseSource = gameObject.AddComponent<CinemachineImpulseSource>();
        Debug.Log("CinemachineImpulseSource added: " + (m_impulseSource != null));
        SetImpulseInfo();
    }
}
