using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScalerScript : MonoBehaviour, IScrollHandler
{
    private Vector3 m_initialScale;

    [SerializeField]
    private float m_zoomSpeed = 0.1f;
    [SerializeField]
    private float m_maxZoom = 10f;
    private RectTransform m_transform;

    private void Awake()
    {
        m_initialScale = transform.localScale;
    }

   public void OnScroll(PointerEventData eventData)
    {
        var delta = Vector3.one * (eventData.scrollDelta.y * m_zoomSpeed);
        var desiredScale = transform.localScale + delta;

        desiredScale = ClampDesiredScale(desiredScale);

        transform.localScale = desiredScale;
    }

    private Vector3 ClampDesiredScale(Vector3 desiredScale)
    {
        desiredScale = Vector3.Max(m_initialScale, desiredScale);
        desiredScale = Vector3.Min(m_initialScale * m_maxZoom, desiredScale);
        return desiredScale;
    }
}
