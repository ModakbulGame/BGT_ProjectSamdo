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

        PlayManager.SetCameraMode(_mode);
    }

    private void InitSetting()                              // �ӽ� �ʱ� ����
    {
        SetControlMode(EControlMode.THIRD_PERSON);
        SetMouseSensitive(1);
    }


    public void SetManager()
    {
        SetInputActions();
    }

    private void SetInputActions()
    {
        m_inputSystem = new();
    }

    private void Start()
    {
        InitSetting();
    }

    private void Update()
    {
/*        if (PlayerInputs.OpenOptionUI.triggered)          // �ð� ������ �ʿ��� ���
        {
            if(Time.timeScale == 0) { Time.timeScale = 1; }
            else { Time.timeScale = 0; }
        }*/

        if (CurControlMode == EControlMode.THIRD_PERSON)            // �÷��̾� ���� ����� ��
        {
            if (PlayerInputs.OpenPlayUI.triggered)              // Tab ������
            {
                if (PlayManager.IsPlaying)                  // Play Manager�� ���� ���̸�
                {
                    PlayManager.OpenPlayerUI();                 // PlayerUI ����
                    SetControlMode(EControlMode.UI_CONTROL);    // UI ���� ����
                }
            }
            else if (PlayerInputs.OpenOptionUI.triggered)       // Escape ������
            {
                // ���� â ����
            }
            else if(PlayerInputs.OpenMapUI.triggered)
            {
                if(PlayManager.IsPlaying)
                {
                    // �� ���ݱ�
                    PlayManager.ToggleMapUI();
                }

            }
        }
        else if (CurControlMode == EControlMode.UI_CONTROL)         // UI ���� ����� ��
        {
            if (UIControlInputs.CloseUI.triggered)              // Escape ������
            {
                if (PlayManager.IsPlaying)                  // Play Manager�� ���� ���̸�
                {
                    PlayManager.ClosePlayerUI();                // PlayerUI �ݱ�(�ӽ�)
                    SetControlMode(EControlMode.THIRD_PERSON);  // �÷��̾� ���� ����
                }
            }
        }
    }
}
