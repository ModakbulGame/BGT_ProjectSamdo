using Cinemachine;
using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    private CinemachineFreeLook m_cameraDetail;
    public CinemachineFreeLook PlayerFreeLook { get { return m_cameraDetail; } }

    private const float XMoveMultiplier = 120;              // 민감도 1당 X 움직임
    private const float YMoveMultiplier = 1.5f;                // 민감도 1당 Y 움직임

    private EControlMode CurCameraMode { get; set; }        // 현재 조작 모드
    private float MouseSensitive { get; set; }              // 마우스 민감도

    public void SetThirdPerson()                            // 3인칭 모드 설정
    {
        CurCameraMode = EControlMode.THIRD_PERSON;
        m_cameraDetail.m_YAxis.Value = 0.5f;
        m_cameraDetail.m_XAxis.Value = 180;
        SetCinemachineSpeed(MouseSensitive);
    }
    public void SetCameraSensitive(float _sensitive)        // 마우스 민감도 설정
    {
        MouseSensitive = _sensitive;
        if (CurCameraMode == EControlMode.THIRD_PERSON)
        {
            SetCinemachineSpeed(MouseSensitive);
        }
    }
    public void SetUIControl()                              // UI 조작 모드 설정
    {
        CurCameraMode = EControlMode.UI_CONTROL;
        m_cameraDetail.m_YAxis.Value = 0.2f;
        m_cameraDetail.m_XAxis.Value = 330;
        SetCinemachineSpeed(0);
    }

    private void SetCinemachineSpeed(float _speed)          // 실제 민감도 설정 함수
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
