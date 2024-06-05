using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapUIScript : MonoBehaviour
{                           
    private GameObject[] m_mapOasis;  // 맵 UI에 표시되는 오브젝트와 이미지들
    [SerializeField]
    private GameObject m_mapOasisImage;

    [SerializeField]
    private Image m_mapPlayerImage;
    [SerializeField]
    private Transform m_targetPlayer;
    [SerializeField]
    private Image m_mapImage;

    private TextMeshProUGUI m_mapName;
    private Vector2 m_mapArea;
    private bool m_isMapUIToggle = false;

    private float m_zoomIncrement = 0.1f;       // 확대 축소를 위한 변수
    private float m_currentZoom = 1f;
    private float m_maxIncrement = 2.0f;
    private float m_minIncrement = 0.4f;


    public void ToggleMapUI()
    {
        if (!m_isMapUIToggle)
        {
            gameObject.SetActive(true);
            m_isMapUIToggle = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            gameObject.SetActive(false);
            m_isMapUIToggle = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ZoomIn()
    {
        if (m_currentZoom < m_maxIncrement)
        {
            m_mapImage.rectTransform.localScale = new Vector3(m_currentZoom, m_currentZoom, 1f);
            m_currentZoom += m_zoomIncrement;
        }
    }

    public void ZoomOut()
    {
        if (m_currentZoom > m_minIncrement)
        {
            m_mapImage.rectTransform.localScale = new Vector3(m_currentZoom, m_currentZoom, 1f);
            m_currentZoom -= m_zoomIncrement;
        }
    }

    private void SynchronizePlayerLocation()
    {
        // 카메라처럼 따라다니는 것을 묘사하기 위해 맵 이미지를 플레이어 이동방향의 반대방향으로 움직이는 것으로 구현
        m_mapImage.rectTransform.anchoredPosition = new Vector2(m_mapImage.rectTransform.sizeDelta.x * PlayManager.NormalizeLocation(m_targetPlayer).x * -1,
             m_mapImage.rectTransform.sizeDelta.y * PlayManager.NormalizeLocation(m_targetPlayer).y * -1);
    }

    private void SynchronizeOasisLocation()
    {
        for (uint i = 0; i < m_mapOasis.Length; i++)
        {
            GameObject OasisImage = Instantiate(m_mapOasisImage, Vector3.zero, Quaternion.identity, m_mapImage.transform);
            Image mapOasisImage = OasisImage.GetComponent<Image>();
            Transform mapOasisTransform = m_mapOasis[i].transform;

            mapOasisImage.rectTransform.anchoredPosition = new Vector2(m_mapImage.rectTransform.sizeDelta.x * PlayManager.NormalizeLocation(mapOasisTransform).x, 
                m_mapImage.rectTransform.sizeDelta.y * PlayManager.NormalizeLocation(mapOasisTransform).y);
        }
    }

    private void SetComps()
    {
        m_mapOasis = PlayManager.MapOasis;

        SynchronizeOasisLocation();

        m_mapName = GetComponentInChildren<TextMeshProUGUI>();
        m_mapName.text = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        SynchronizePlayerLocation();
    }

    private void Start()
    {
        SetComps();
    }
}
