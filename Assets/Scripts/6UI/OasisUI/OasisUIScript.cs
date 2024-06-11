using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum EOasisFunctionName
{
    REST,
    TRANSPORT,
    TRADE,
    RESET,
    LAST
}

public interface IOasisUI
{
    public void OpenUI(OasisUIScript _parent);
    public void CloseUI();
}

public class OasisUIScript : MonoBehaviour
{
    private OasisNPC m_oasis;
    public void SetNPC(OasisNPC _oasis) { m_oasis = _oasis; }

    public OasisNPC Oasis { get { return m_oasis; } }

    [SerializeField]
    private GameObject[] m_uis = new GameObject[(int)EOasisFunctionName.LAST];


    private Button[] m_btns;

    private bool IsCompsSet { get; set; }                       // ���� �� �ִ��� (ó�� �������� Ȯ�ο�)

    private bool ButtonClicked { get; set; }


    public void OpenUI(OasisNPC _npc)
    {
        gameObject.SetActive(true);
        if (!IsCompsSet) { SetComps(); }

        ButtonClicked = false;

        SetNPC(_npc);
    }

    public void CloseUI()                       // �ݱ�
    {
        m_oasis.StopInteract();
        gameObject.SetActive(false);
    } 


    private void ClickButton(EOasisFunctionName _function)
    {
        if(ButtonClicked == true) { return; }

        GameObject ui = m_uis[(int)_function];
        ui.GetComponent<IOasisUI>().OpenUI(this);
        ButtonClicked = true;
    }
    public void FunctionDone()
    {
        ButtonClicked = false;
    }


    private void SetComps()
    {
        m_btns = GetComponentsInChildren<Button>();
        SetBtns();
        IsCompsSet = true;
    }
    private void SetBtns()
    {
        for (int i = 0; i<(int)EOasisFunctionName.LAST; i++)
        {
            EOasisFunctionName function = (EOasisFunctionName)i;
            m_btns[i].onClick.AddListener(delegate { ClickButton(function); });
        }
        m_btns[(int)EOasisFunctionName.LAST].onClick.AddListener(CloseUI);
    }
}
