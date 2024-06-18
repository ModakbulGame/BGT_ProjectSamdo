using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestSideBarScript : BaseUI
{
    private RectTransform m_rect;

    private QuestBarElmScript[] m_elms;


    private readonly float ElmWidth = 448;
    private readonly float ElmHeight = 96;
    private readonly float ElmSpace = 8;

    public override void UpdateUI()
    {
        List<QuestInfo> curList = new();
        List<QuestInfo> infos = PlayManager.QuestInfoList;
        foreach(QuestInfo info in infos) 
        {
            if(info.Status == EQuestStatus.ACCEPTED || info.Status == EQuestStatus.COMPLETE)
            {
                curList.Add(info);
            }
        }
        if(curList.Count > 0) { ApplyInfos(curList); }
        else { CloseUI(); }
    }

    private void ApplyInfos(List<QuestInfo> _infos)
    {
        int count = _infos.Count;
        SetBoxSize(count);
        for (int i = 0; i<ValueDefine.MAX_QUEST_NUM; i++)
        {
            if(i >= count) { m_elms[i].HideElm(); continue; }
            m_elms[i].SetElm(_infos[i]);
        }
    }
    private void SetBoxSize(int _count)
    {
        float width = ElmWidth + ElmSpace * 2;
        float height = ElmHeight * _count + ElmSpace * (_count + 4);
        m_rect.sizeDelta = new(width, height);
    }



    public override void SetComps()
    {
        base.SetComps();
        m_rect = GetComponent<RectTransform>();
        m_elms = GetComponentsInChildren<QuestBarElmScript>();
        if (m_elms.Length != ValueDefine.MAX_QUEST_NUM) { Debug.Log("퀘스트 UI 개수 다름"); return; }
        foreach(QuestBarElmScript elm in m_elms) { elm.SetComps(); }
    }
}
