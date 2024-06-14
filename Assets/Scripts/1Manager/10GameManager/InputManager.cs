using UnityEngine;

public enum EControlMode            // 조작 모드
{
    THIRD_PERSON,           // 3인칭
    UI_CONTROL              // UI 조작
}

public class InputManager : MonoBehaviour
{
    private InputSystem m_inputSystem;                                                                  // 전체 Input System
    public InputSystem.PlayerActions PlayerInputs { get { return m_inputSystem.Player; } }              // 플레이어 조작 Input System
    public InputSystem.UIControlActions UIControlInputs { get { return m_inputSystem.UIControl; } }     // UI 조작 Input System

    public EControlMode CurControlMode { get; private set; } = EControlMode.UI_CONTROL;     // 현재 조작 모드

    private static float m_mouseSensitive = 1;                                              // 마우스 민감도
    public static float MouseSensitive { get { return m_mouseSensitive; } }


    public void SetMouseSensitive(float _sensitive)                                         // 마우스 민감도 설정
    {
        m_mouseSensitive = _sensitive;
        PlayManager.SetCameraSensitive(_sensitive);
    }

    public void SetControlMode(EControlMode _mode)                                          // 조작 모드 설정
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

        if (GameManager.IsInGame) { PlayManager.SetCameraMode(_mode); ; }
    }

    private void InitSetting()
    {
        SetControlMode(EControlMode.UI_CONTROL);
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
        if (!GameManager.IsInGame) { return; }
        if (CurControlMode == EControlMode.THIRD_PERSON)            // 플레이어 조작 모드일 때
        {
            if (PlayerInputs.OpenPlayUI.triggered)              // Tab 누르면
            {
                if (PlayManager.IsPlaying)                  // Play Manager가 실행 중이면
                {
                    PlayManager.OpenPlayerUI();                 // PlayerUI 열기
                    PlayManager.ResetPlayer();
                    SetControlMode(EControlMode.UI_CONTROL);    // UI 조작 모드로
                }
            }
            else if (PlayerInputs.OpenOptionUI.triggered)       // Escape 누르면
            {
                // 설정 창 열기
            }
            else if(PlayerInputs.OpenMapUI.triggered)
            {
                if(PlayManager.IsPlaying)
                {
                    // 맵 여닫기
                    PlayManager.ToggleMapUI();
                }
            }
            else if(PlayerInputs.OpenQuestUI.triggered)
            {
                if (PlayManager.IsPlaying)
                {
                    // 퀘스트 창 열기
                    PlayManager.ToggleQuestUI();
                }
            }
        }
        else if (CurControlMode == EControlMode.UI_CONTROL)         // UI 조작 모드일 때
        {
            if (UIControlInputs.CloseUI.triggered)              // Escape 누르면
            {
                if (PlayManager.IsPlaying)                  // Play Manager가 실행 중이면
                {
                    PlayManager.ClosePlayerUI();                // PlayerUI 닫기(임시)
                    SetControlMode(EControlMode.THIRD_PERSON);  // 플레이어 조작 모드로
                }
            }
            else if(UIControlInputs.UIInteract.triggered)       // NPCDialogueScript의 Update를 여기서 진행시키고자 했음   
            {
                if (PlayManager.IsPlaying && PlayManager.IsDialogueOpend)
                {
                    PlayManager.ShowNextDialogue();
                }
            }
        }
    }
}
