using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathUIScript : BaseUI
{
    [SerializeField]
    private Button m_restartBtn;


    private void RestartGame()
    {
        PlayManager.RestartGame();
    }


    private void SetBtns()
    {
        m_restartBtn.onClick.AddListener(RestartGame);
    }
    public override void SetComps()
    {
        base.SetComps();
        SetBtns();
    }
}
