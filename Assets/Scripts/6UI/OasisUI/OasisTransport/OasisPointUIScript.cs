using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class OasisPointUIScript : MonoBehaviour
{
    public OasisTransportUIScript m_parent;
    public void SetParent(OasisTransportUIScript _parent, EMapPointName _point) { m_parent = _parent; PointName = _point; SetComps(); }

    private Image m_mapImg;
    private Image m_img;
    private Button m_btn;

    private EMapPointName PointName { get; set; }

    private readonly Color IdleColor = new(246/255f, 187/255f, 187/255f);
    private readonly Color SelectColor = new(1, 0, 0);

    public void SetDestination()
    {
        m_parent.SetDestination(PointName);
        m_img.color = SelectColor;
    }

    public void ResetDestination()
    {
        m_img.color = IdleColor;
    } 

    private void NormalizeOasisLocation()
    {
        RectTransform btnRectTransform = m_btn.transform.parent.GetComponent<RectTransform>();

        Vector2 normalImgPos = PlayManager.NormalizeLocation(PlayManager.MapOasis[(int)PointName].transform);
        Vector2 normalBtnPos = normalImgPos; 

        m_img.rectTransform.anchoredPosition = new Vector2(m_mapImg.rectTransform.sizeDelta.x * normalImgPos.x, m_mapImg.rectTransform.sizeDelta.y * normalImgPos.y);
        btnRectTransform.anchoredPosition = new Vector2(m_mapImg.rectTransform.sizeDelta.x * normalBtnPos.x, m_mapImg.rectTransform.sizeDelta.y * normalBtnPos.y);
    }
    
    private void SetBtns()
    {
        m_btn.onClick.AddListener(SetDestination);
    }
    private void SetComps()
    {
        m_img = GetComponent<Image>();
        m_btn = GetComponent<Button>();
        m_mapImg = transform.parent.GetComponent<Image>();
        SetBtns();
        NormalizeOasisLocation();
        ResetDestination();
    }
}
