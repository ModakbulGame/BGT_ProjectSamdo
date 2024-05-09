using Cinemachine;
using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    private CinemachineFreeLook m_cameraDetail;
    public CinemachineFreeLook PlayerFreeLook { get { return m_cameraDetail; } }

    private const float XMoveMultiplier = 120;              // �ΰ��� 1�� X ������
    private const float YMoveMultiplier = 1.5f;                // �ΰ��� 1�� Y ������

    private EControlMode CurCameraMode { get; set; }        // ���� ���� ���
    private float MouseSensitive { get; set; }              // ���콺 �ΰ���

    public void SetThirdPerson()                            // 3��Ī ��� ����
    {
        CurCameraMode = EControlMode.THIRD_PERSON;
        m_cameraDetail.m_YAxis.Value = 0.5f;
        m_cameraDetail.m_XAxis.Value = 180;
        SetCinemachineSpeed(MouseSensitive);
    }
    public void SetCameraSensitive(float _sensitive)        // ���콺 �ΰ��� ����
    {
        MouseSensitive = _sensitive;
        if (CurCameraMode == EControlMode.THIRD_PERSON)
        {
            SetCinemachineSpeed(MouseSensitive);
        }
    }
    public void SetUIControl()                              // UI ���� ��� ����
    {
        CurCameraMode = EControlMode.UI_CONTROL;
        m_cameraDetail.m_YAxis.Value = 0.2f;
        m_cameraDetail.m_XAxis.Value = 330;
        SetCinemachineSpeed(0);
    }

    private void SetCinemachineSpeed(float _speed)          // ���� �ΰ��� ���� �Լ�
    {
        m_cameraDetail.m_XAxis.m_MaxSpeed = XMoveMultiplier * _speed;
        m_cameraDetail.m_YAxis.m_MaxSpeed = YMoveMultiplier * _speed;
    }


    private void SetComps()
    {
        m_cameraDetail = GetComponent<CinemachineFreeLook>();
    }

    private void Awake()
    {
        SetComps();
    }
}
