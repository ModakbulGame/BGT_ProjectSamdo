using UnityEngine;

public enum EControlMode            // ���� ���
{
    THIRD_PERSON,           // 3��Ī
    UI_CONTROL              // UI ����
}

public class InputManager : MonoBehaviour
{
    private InputSystem m_inputSystem;                                                                  // ��ü Input System
    public InputSystem.PlayerActions PlayerInputs { get { return m_inputSystem.Player; } }              // �÷��̾� ���� Input System
    public InputSystem.UIControlActions UIControlInputs { get { return m_inputSystem.UIControl; } }     // UI ���� Input System

    public EControlMode CurControlMode { get; private set; } = EControlMode.UI_CONTROL;     // ���� ���� ���

    private static float m_mouseSensitive = 1;                                              // ���콺 �ΰ���
    public static float MouseSensitive { get { return m_mouseSensitive; } }


    public void SetMouseSensitive(float _sensitive)                                         // ���콺 �ΰ��� ����
    {
        m_mouseSensitive = _sensitive;
        PlayManager.SetCameraSensitive(_sensitive);
    }

    public void SetControlMode(EControlMode _mode)                                          // ���� ��� ����
    {
        CurControlMode = _mode;
        switch (_mode)
        {
            case EControlMode.THIRD_PERSON:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                PlayerInputs.Enable();
                UIControlInputs.Disable();
                break;
            case EControlMode.UI_CONTROL:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                UIControlInputs.Enable();
                PlayerInputs.Disable();
                break;
        }

        if (PlayManager.IsPlaying) { PlayManager.SetCameraMode(_mode); ; }
    }


    public void SetManager()
    {
        SetInputActions();
    }

    private void SetInputActions()
    {
        m_inputSystem = new();
    }

    private void Update()
    {
        if (!PlayManager.IsPlaying) { return; }
        if (CurControlMode == EControlMode.THIRD_PERSON)            // �÷��̾� ���� ����� ��
        {
            if (PlayerInputs.OpenPlayUI.triggered)              // Tab ������
            {
                PlayManager.TogglePlayerUI(true);           // PlayerUI ����
                PlayManager.ResetPlayer();
                SetControlMode(EControlMode.UI_CONTROL);    // UI ���� ����
            }
            else if (PlayerInputs.OpenOptionUI.triggered)       // Escape ������
            {
                PlayManager.ToggleOptionUI(true);
                SetControlMode(EControlMode.UI_CONTROL);    // UI ���� ����
            }
            else if (PlayerInputs.OpenMapUI.triggered)
            {
                // �� ���ݱ�
                PlayManager.ToggleMapUI();
            }
            else if (PlayerInputs.OpenQuestUI.triggered)
            {
                // ����Ʈ â ����
                PlayManager.ToggleQuestUI(true);
            }
        }
        else if (CurControlMode == EControlMode.UI_CONTROL)         // UI ���� ����� ��
        {
            if (UIControlInputs.CloseUI.triggered)              // Escape ������
            {
                if (PlayManager.IsPlayerUIOpen)
                {
                    PlayManager.TogglePlayerUI(false);          // PlayerUI �ݱ�
                    return;
                }
                if (PlayManager.IsOptionOpen)
                {
                    PlayManager.ToggleOptionUI(false);
                    return;
                }
            }
        }
    }
}
