using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float m_magnitude = 1.0f; // ���޽� ������ Ȯ���ϱ� ���� �⺻ ���� �����մϴ�.
    private CinemachineImpulseSource m_impulseSource;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("in");
        CameraShaking(m_magnitude);
        Debug.Log("out");
    }

    public void CameraShaking(float magnitude)
    {
        Debug.Log("in2");
        m_impulseSource.GenerateImpulse(magnitude);
        Debug.Log("out2");
    }

    public void SetImpulseInfo()
    {
        m_impulseSource.m_ImpulseDefinition = new CinemachineImpulseDefinition(); // ImpulseDefinition �ʱ�ȭ
        Debug.Log("ImpulseDefinition set: " + (m_impulseSource.m_ImpulseDefinition != null));

        m_impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
        m_impulseSource.m_ImpulseDefinition.m_AmplitudeGain = 1.0f; // �߰��� ���޽� ������ �����մϴ�.
        m_impulseSource.m_ImpulseDefinition.m_FrequencyGain = 1.0f; // �߰��� ���ļ� ������ �����մϴ�.

        m_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = 0.05f;    // ���� �ð�
        m_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 0.15f;   // ���� �ð�
        m_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = 0.3f;      // ���� �ð�

        m_impulseSource.m_DefaultVelocity = new Vector3(1.0f, 1.0f, 1.0f); // ���޽��� ����� ũ�⸦ ����� Ű��ϴ�.
    }

    private void Start()
    {
        m_impulseSource = gameObject.AddComponent<CinemachineImpulseSource>();
        Debug.Log("CinemachineImpulseSource added: " + (m_impulseSource != null));
        SetImpulseInfo();
    }
}
