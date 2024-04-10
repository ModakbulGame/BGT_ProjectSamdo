using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum EOasisFunctionName
{
    CHECKPOINT,
    TRANSPORT,
    TRADE,
    RESET,
    REST,
    LAST
}

public interface IOasisUI
{
    public void OpenUI(OasisUIScript _parent);
    public void CloseUI();
}

public class OasisUIScript : MonoBehaviour
{
    private OasisNPC m_npc;
    public void SetNPC(OasisNPC _npc) { m_npc = _npc; }

    [SerializeField]
    private GameObject[] m_uiPrefabs = new GameObject[(int)EOasisFunctionName.LAST];


    private Button[] m_btns;

    private bool Opened { get; set; }                       // 열린 적 있는지 (처음 열리는지 확인용)

    private bool ButtonClicked { get; set; }

    public void OpenUI()
    {
        gameObject.SetActive(true);
        if (!Opened) { SetComps(); }

        ButtonClicked = false;
    }
    public void OpenUI(OasisNPC _npc)
    {
        SetNPC(_npc);
        OpenUI();
    }

    public void CloseUI()                       // 닫기
    {
        m_npc.StopInteract();
        gameObject.SetActive(false);
    } 


    private void ClickButton(EOasisFunctionName _function)
    {
        if(ButtonClicked == true) { return; }

        GameObject ui = Instantiate(m_uiPrefabs[(int)_function], GameManager.GetCanvasTransform(ECanvasType.CONTROL));
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
        Opened = true;
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

    private void Awake()
    {

    }

    private void Start()
    {
        if (!Opened) { SetComps(); OpenUI(); }
    }
}
