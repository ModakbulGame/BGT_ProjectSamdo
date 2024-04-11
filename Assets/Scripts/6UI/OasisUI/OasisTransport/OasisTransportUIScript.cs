using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OasisTransportUIScript : MonoBehaviour, IOasisUI
{
    private OasisUIScript m_parent;
    public void OpenUI(OasisUIScript _parent) { m_parent = _parent; SetComps(); }

    private Button m_transportBtn;
    private OasisPointUIScript[] m_oasisPoints;
    private Transform[] m_oasisTransforms;

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
        Debug.Log(CurDestination + "로 이동!");
        CloseUI();
        // FadeOut
        MoveToOasis(CurDestination);
        // FadeIn
    }

    public void MoveToOasis(EMapPointName _point)
    {
        Transform destOasis = m_parent.OasisTransform;  // 맵과 화톳불의 위치가 동기화 되면 바꿀 예정

        if (PlayManager.GetDistToPlayer(destOasis.position) <= 2.5f)  // 상호 작용 거리 내 화톳불이 있으면 그 곳으로는 이동 불가, 없을 시 이동 가능; 임시 조건식이라 더 정확하게 수정할 필요가 있음...
        {
            Debug.Log("현재 위치한 화톳불입니다!");
            return;
        }
        else
        {
            Debug.Log(destOasis.position);
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
