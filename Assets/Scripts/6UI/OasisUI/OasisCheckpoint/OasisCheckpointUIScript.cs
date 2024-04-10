using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OasisCheckpointUIScript : MonoBehaviour, IOasisUI
{
    private OasisUIScript m_parent;
    public void OpenUI(OasisUIScript _parent) { m_parent = _parent; SetComps(); }


    private void SaveCheckpoint()
    {
        Debug.Log("체크포인트 저장");
        CloseUI();
    }

    private void CancelUI()
    {
        CloseUI();
    }

    public void CloseUI()
    {
        m_parent.FunctionDone();
        Destroy(gameObject);
    }


    private void SetComps()
    {
        Button[] btns = GetComponentsInChildren<Button>();
        btns[0].onClick.AddListener(SaveCheckpoint);
        btns[1].onClick.AddListener(CancelUI);
    }
}
