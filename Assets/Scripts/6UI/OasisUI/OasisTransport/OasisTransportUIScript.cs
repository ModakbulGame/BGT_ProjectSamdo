using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OasisTransportUIScript : MonoBehaviour, IOasisUI
{
    private OasisUIScript m_parent;
    public OasisUIScript Parent { get { return m_parent; } }
    public void OpenUI(OasisUIScript _parent) { m_parent = _parent; SetComps(); }

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
        Debug.Log(CurDestination + "�� �̵�!");
        CloseUI();
        // FadeOut
        MoveToOasis(CurDestination);
        // FadeIn
    }

    public void MoveToOasis(EMapPointName _point)
    {
        Transform destOasis = m_oasisPoints[(int)_point].Parent.Parent.transform;
        Debug.Log(destOasis.position);

        if (PlayManager.GetDistToPlayer(destOasis.position) <= 2.5f)  // ��ȣ �ۿ� �Ÿ� �� ȭ����� ������ �� �����δ� �̵� �Ұ�, ���� �� �̵� ����
        {
            Debug.Log("���� ��ġ�� ȭ����Դϴ�!");
            return;
        }
        else
        {
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
