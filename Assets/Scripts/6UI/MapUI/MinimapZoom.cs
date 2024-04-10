using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MinimapZoom : MonoBehaviour
{
    [SerializeField]
    private Camera m_minimapCamera;
    [SerializeField]
    private float m_zoomMin = 1;  // 카메라의 orthograpicSize 최소 크기
    [SerializeField]
    private float m_zoomMax = 30;  // 카메라의 orthograpicSize 최대 크기
    [SerializeField]
    private float m_zoomStepSize = 1;  // 1회 줌 시 증가 혹은 감소되는 수치
    [SerializeField]
    private TextMeshProUGUI textMapName;

    private void Awake()
    {
        // 맵 이름을 현재 scene 이름으로 설정
        textMapName.text = SceneManager.GetActiveScene().name;
    }

   public void ZoomIn()
    {
        // 카메라에 보이는 사물 크기 확대
        m_minimapCamera.orthographicSize = Mathf.Max(m_minimapCamera.orthographicSize - m_zoomStepSize, m_zoomMin);
    }

    public void ZoomOut()
    {
        // 카메라에 보이는 사물 크기 축소
        m_minimapCamera.orthographicSize = Mathf.Min(m_minimapCamera.orthographicSize + m_zoomStepSize, m_zoomMax);
    }

}
