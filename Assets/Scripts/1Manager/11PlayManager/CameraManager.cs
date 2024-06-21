using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private PlayerCameraScript m_playerCamera;                                  // ���� ī�޶�
    [SerializeField]
    private Camera m_uiCamera;
    public CinemachineFreeLook PlayerFreeLook { get { return m_playerCamera.PlayerFreeLook; } }

    public float CameraRotation { get { return transform.eulerAngles.y; } }     // ī�޶� �¿� ����
    public float CameraAngle { get { return -transform.eulerAngles.x; } }       // ī�޶� ���Ʒ� ����

    public void SetCameraMode(EControlMode _mode)                               // ���� ��� ���� ����
    {
        if (_mode == EControlMode.THIRD_PERSON) { m_playerCamera.SetThirdPerson(); }
        else if (_mode == EControlMode.UI_CONTROL) { m_playerCamera.SetUIControl(); }
    }
    public void SetNPCView() { m_playerCamera.SetNPCView(); }

    public void SetCameraSensitive(float _sensitive)                            // ī�޶� �ΰ��� ���� ����
    {
        m_playerCamera.SetCameraSensitive(_sensitive);
    }

    public void LooseFocus()
    {
        m_playerCamera.LooseFocus();
    }

    public void SwitchToCamera(CinemachineFreeLook _targetCamera)
    {

    }
}
