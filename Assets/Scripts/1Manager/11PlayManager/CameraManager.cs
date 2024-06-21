using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private PlayerCameraScript m_playerCamera;                                  // 현재 카메라
    [SerializeField]
    private Camera m_uiCamera;
    public CinemachineFreeLook PlayerFreeLook { get { return m_playerCamera.PlayerFreeLook; } }

    public float CameraRotation { get { return transform.eulerAngles.y; } }     // 카메라가 좌우 각도
    public float CameraAngle { get { return -transform.eulerAngles.x; } }       // 카메라 위아래 각도

    public void SetCameraMode(EControlMode _mode)                               // 조작 모드 전달 받음
    {
        if (_mode == EControlMode.THIRD_PERSON) { m_playerCamera.SetThirdPerson(); }
        else if (_mode == EControlMode.UI_CONTROL) { m_playerCamera.SetUIControl(); }
    }
    public void SetNPCView() { m_playerCamera.SetNPCView(); }

    public void SetCameraSensitive(float _sensitive)                            // 카메라 민감도 전달 받음
    {
        m_playerCamera.SetCameraSensitive(_sensitive);
    }

    public void LooseFocus()
    {
        m_playerCamera.LooseFocus();
    }
    public void SetCameraDepth()
    {
        Camera playerCameraComponent = m_playerCamera.GetComponent<Camera>();
        playerCameraComponent.depth = 0;                            // 기본 depth 설정
        m_uiCamera.clearFlags = CameraClearFlags.Depth;             // 이전 카메라의 깊이 버퍼만 유지
        m_uiCamera.depth = 1;                                       // PlayerCamera보다 높은 depth 설정
    }
    public void SwitchToCamera(CinemachineFreeLook _targetCamera)
    {

    }

    private void Awake()
    {
        SetCameraDepth();
    }
}
