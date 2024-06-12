using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class OasisPointUIScript : MonoBehaviour
{
    private OasisTransportUIScript m_parent;
    public void SetParent(OasisTransportUIScript _parent) { m_parent = _parent; }

    private RectTransform m_rect;
    private Image m_img;
    private Button m_btn;

    [SerializeField]
    private EOasisPointName m_pointName;
    private EOasisPointName PointName { get { return m_pointName; } set { m_pointName = value; } }

    private readonly Color IdleColor = new(246/255f, 187/255f, 187/255f);
    private readonly Color CurColor = new(38/255f, 167/255f, 245/255f);
    private readonly Color SelectColor = new(1, 0, 0);

    public void SetDestination()
    {
        if(m_parent.CurOasisName == PointName) { return; }
        m_parent.SetDestination(PointName);
        m_img.color = SelectColor;
    }

    public void ResetDestination()
    {
        m_img.color = IdleColor;
    }

    private void SetPosition(RectTransform _rect)
    {
        float width = PlayManager.MapWidth, height = PlayManager.MapHeight;

        Vector2 pos = PlayManager.OasisList[(int)PointName].Position2;
        Vector2 mapSize = _rect.sizeDelta;

        m_rect.anchoredPosition = new(mapSize.x * pos.x / width, mapSize.y * pos.y / height);
    }

    private void SetBtns()
    {
        m_btn.onClick.AddListener(SetDestination);
    }
    public void SetComps(EOasisPointName _oasis, RectTransform _rect)
    {
        m_rect = GetComponent<RectTransform>();
        m_img = GetComponent<Image>();
        m_btn = GetComponent<Button>();
        PointName = _oasis;
        SetBtns();
        SetPosition(_rect);
        if (_oasis == m_parent.CurOasisName) { m_img.color = CurColor; }
        else { ResetDestination(); }
    }
}
