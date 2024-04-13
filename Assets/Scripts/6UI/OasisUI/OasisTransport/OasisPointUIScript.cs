using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class OasisPointUIScript : MonoBehaviour
{
    private OasisTransportUIScript m_parent;
    public OasisTransportUIScript Parent { get { return m_parent; } }
    public void SetParent(OasisTransportUIScript _parent, EMapPointName _point) { m_parent = _parent; PointName = _point; SetComps(); }

    private Image m_img;
    private Button m_btn;
    private Image m_mapImg;

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

        // index -> 0 : left, 1 : right, 2 : bottom, 3 : top
        Vector2 mapArea = new Vector2(Vector3.Distance(PlayManager.NormalizeObjects[0].position, PlayManager.NormalizeObjects[1].position), 
            Vector3.Distance(PlayManager.NormalizeObjects[2].position, PlayManager.NormalizeObjects[3].position));

        Vector2 oasisBtnPos = new Vector2(Vector3.Distance(PlayManager.NormalizeObjects[0].position, new Vector3(m_parent.Parent.OasisTransform.position.x, 0f, 0f)),
            Vector3.Distance(PlayManager.NormalizeObjects[2].position, new Vector3(0f, 0f, m_parent.Parent.OasisTransform.position.z)));
        Vector2 oasisImgPos = oasisBtnPos;

        Vector2 normalBtnPos = new Vector2(oasisBtnPos.x / mapArea.x, oasisBtnPos.y / mapArea.y);
        Vector2 normalImgPos = normalBtnPos;    

        btnRectTransform.anchoredPosition = new Vector2(m_mapImg.rectTransform.sizeDelta.x * normalBtnPos.x, m_mapImg.rectTransform.sizeDelta.y * normalBtnPos.y);
        m_img.rectTransform.anchoredPosition = new Vector2(m_mapImg.rectTransform.sizeDelta.x * normalImgPos.x, m_mapImg.rectTransform.sizeDelta.y * normalImgPos.y);
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
