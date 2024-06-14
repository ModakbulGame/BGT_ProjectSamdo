using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Button m_newBtn;
    [SerializeField]
    private Button m_loadBtn;
    [SerializeField]
    private LoadingListScript m_loadingList;


    private void StartGame()
    {
        GameManager.StartGame();
    }

    private void OpenLoad()
    {
        m_loadingList.OpenUI();
    }

    private void SetLoadData()
    {
        SaveData[] data = GameManager.GameData;
        bool hasData = false;
        for (int i = 0; i<ValueDefine.MAX_SAVE; i++)
        {
            if (data[i] != null) { hasData = true; break; }
        }
        m_loadBtn.interactable = hasData;
    }

    private void SetBtns()
    {
        m_newBtn.onClick.AddListener(StartGame);
        m_loadBtn.onClick.AddListener(OpenLoad);
    }

    private void SetComps()
    {
        SetBtns();
    }

    private void Awake()
    {
        SetComps();
    }
    private void Start()
    {
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
        SetLoadData();
    }
}
