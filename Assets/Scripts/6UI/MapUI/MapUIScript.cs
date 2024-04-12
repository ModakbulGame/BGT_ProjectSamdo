using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class MapUIScript : MonoBehaviour
{
    public Transform m_left;
    public Transform m_right;
    public Transform m_top;
    public Transform m_bottom;

    [SerializeField]                            // 맵 UI에 표시되는 오브젝트와 이미지들
    private GameObject[] m_mapOasis;
    [SerializeField]
    private GameObject m_mapOasisImage;             
    
    [SerializeField]
    private Image m_mapPlayerImage;
    [SerializeField]
    private Transform m_targetPlayer;
    [SerializeField]
    private Image m_mapImage;
    [SerializeField]
    private CanvasRenderer m_mapCanvas;

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
        Vector2 mapArea = new Vector2(Vector3.Distance(m_left.position, m_right.position), Vector3.Distance(m_bottom.position, m_top.position));

        // 플레이어 위치 정규화
        Vector2 charPos = new Vector2(Vector3.Distance(m_left.position, new Vector3(m_targetPlayer.transform.position.x, 0f, 0f)),
            Vector3.Distance(m_bottom.position, new Vector3(0f, 0f, m_targetPlayer.transform.position.z)));
        Vector2 normalPos = new Vector2(charPos.x / mapArea.x, charPos.y / mapArea.y);

        m_mapPlayerImage.rectTransform.anchoredPosition = new Vector2(m_mapImage.rectTransform.sizeDelta.x * normalPos.x, m_mapImage.rectTransform.sizeDelta.y * normalPos.y);
    }

    private void SynchronizeOasisLocation()
    {
        Vector2 mapArea = new Vector2(Vector3.Distance(m_left.position, m_right.position), Vector3.Distance(m_bottom.position, m_top.position));

        for (uint i = 0; i < m_mapOasis.Length; i++)
        {
            GameObject OasisImage = Instantiate(m_mapOasisImage, Vector3.zero, Quaternion.identity, m_mapCanvas.transform);
            Image mapOasisImage = OasisImage.GetComponent<Image>();

            // 화톳불 위치 정규화
            Vector2 oasisPos = new Vector2(Vector3.Distance(m_left.position, new Vector3(m_mapOasis[i].transform.position.x, 0f, 0f)),
                Vector3.Distance(m_bottom.position, new Vector3(0f, 0f, m_mapOasis[i].transform.position.z)));
            Vector2 oasisNormalPos = new Vector2(oasisPos.x / mapArea.x, oasisPos.y / mapArea.y);

            mapOasisImage.rectTransform.anchoredPosition = new Vector2(m_mapImage.rectTransform.sizeDelta.x * oasisNormalPos.x, m_mapImage.rectTransform.sizeDelta.y * oasisNormalPos.y);
        }
    }

    private void Update()
    {
        SynchronizePlayerLocation();
    }

    private void Start()
    {
        m_mapOasis = GameObject.FindGameObjectsWithTag(ValueDefine.OASIS_TAG);
        SynchronizeOasisLocation();

        m_mapPlayerImage.transform.SetAsFirstSibling();
        m_mapImage.transform.SetAsLastSibling();
    }
}
