using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OasisTransportUIScript : MonoBehaviour, IOasisUI
{
    private OasisUIScript m_parent;
    private FadeInOutGraphic m_fadeInOutGraphic;
    public void OpenUI(OasisUIScript _parent) 
    { 
        m_parent = _parent; 
        SetComps(); 
    }

    private Transform m_parentTransform;
    public Transform ParentTransform { get { return m_parentTransform; } }
    private Button m_transportBtn;
    private OasisPointUIScript[] m_oasisPoints;

    private EMapPointName CurDestination { get; set; } = EMapPointName.LAST;


    public void SetDestination(EMapPointName _point)
    {
        if(CurDestination != EMapPointName.LAST)
        {
            m_oasisPoints[(int)CurDestination].ResetDestination();
        }
        else
        {
            m_transportBtn.interactable = true;
        }
        CurDestination = _point;
    }
    
    private void TransportTo()
    {
        if(CurDestination == EMapPointName.LAST) { return; }
        CloseUI();
        // m_fadeInOutGraphic.Fade_Out();
        MoveToOasis(CurDestination);
        // m_fadeInOutGraphic.Fade_In();
    }

    public void MoveToOasis(EMapPointName _point)
    {
        Transform destOasis = PlayManager.MapOasis[(int)_point].transform;

        if (PlayManager.GetDistToPlayer(destOasis.position) <= 2.5f)  // 상호 작용 거리 내 화톳불이 있으면 그 곳으로는 이동 불가, 없을 시 이동 가능
        {
            Debug.Log("현재 위치한 화톳불입니다!");
            return;
        }
        else
        {
            Debug.Log(CurDestination + "로 이동!");
            PlayManager.TeleportPlayer(destOasis.position);
            CloseUI();
        }
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
    

    private void SetPoints()
    {
        for (int i = 0; i<(int)EMapPointName.LAST; i++)
        {
            m_oasisPoints[i].SetParent(this, (EMapPointName)i);
        }
    }
    private void SetComps()
    {
        Button[] btns = GetComponentsInChildren<Button>();
        m_transportBtn = btns[0];
        m_transportBtn.onClick.AddListener(TransportTo);
        m_transportBtn.interactable = false;
        btns[1].onClick.AddListener(CancelUI);
        m_oasisPoints = GetComponentsInChildren<OasisPointUIScript>();
        SetPoints();
    }
}
