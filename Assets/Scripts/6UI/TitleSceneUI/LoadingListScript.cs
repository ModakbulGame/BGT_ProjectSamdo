using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingListScript : MonoBehaviour
{
    private LoadingListElm[] m_elms;

    [SerializeField]
    private Button m_closeBtn;

    private bool IsCompsSet { get; set; }
    
    public void OpenUI()
    {
        gameObject.SetActive(true);
        if (!IsCompsSet) { SetComps(); }
        List<SaveData> data = GameManager.GameData;
        for (int i = 0; i<ValueDefine.MAX_SAVE; i++)
        {
            if (i >= data.Count) { m_elms[i].EmptySlot(i); continue; }
            m_elms[i].LoadData(i, data[i]);
        }
    }

    public void LoadGame(int _idx)
    {
        GameManager.LoadGame(_idx);
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }


    private void SetBtns()
    {
        m_closeBtn.onClick.AddListener(CloseUI);
    }

    private void SetComps()
    {
        m_elms = GetComponentsInChildren<LoadingListElm>();
        if(m_elms.Length != ValueDefine.MAX_SAVE) { Debug.Log("로딩 슬롯 개수 다름"); return; }
        foreach(LoadingListElm elm in m_elms) { elm.SetParent(this); elm.SetComps(); }
        IsCompsSet = true;
        SetBtns();
    }
}
