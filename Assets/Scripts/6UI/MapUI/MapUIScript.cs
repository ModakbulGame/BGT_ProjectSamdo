using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class MapUIScript : MonoBehaviour
{
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
    private Image m_transportUI;

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
        // 플레이어 위치 정규화
        Vector2 charPos = new Vector2(Vector3.Distance(PlayManager.NormalizeObjects[0].position, new Vector3(m_targetPlayer.transform.position.x, 0f, 0f)),
            Vector3.Distance(PlayManager.NormalizeObjects[2].position, new Vector3(0f, 0f, m_targetPlayer.transform.position.z)));
        Vector2 normalPos = new Vector2(charPos.x / m_mapArea.x, charPos.y / m_mapArea.y);

        // 카메라처럼 따라다니는 것을 묘사하기 위해 맵 이미지를 플레이어 이동방향의 반대방향으로 움직이는 것으로 구현
        m_mapPlayerImage.rectTransform.anchoredPosition = new Vector2(m_mapImage.rectTransform.sizeDelta.x * normalPos.x, m_mapImage.rectTransform.sizeDelta.y * normalPos.y);
    }

    private void SynchronizeOasisLocation()
    {
        for (uint i = 0; i < m_mapOasis.Length; i++)
        {
            // 화톳불 위치 정규화
            Vector2 oasisPos = new Vector2(Vector3.Distance(PlayManager.NormalizeObjects[0].position, new Vector3(m_mapOasis[i].transform.position.x, 0f, 0f)),
                Vector3.Distance(PlayManager.NormalizeObjects[2].position, new Vector3(0f, 0f, m_mapOasis[i].transform.position.z)));
            Vector2 oasisNormalPos = new Vector2(oasisPos.x / m_mapArea.x, oasisPos.y / m_mapArea.y);

            GameObject OasisImage = Instantiate(m_mapOasisImage, Vector3.zero, Quaternion.identity, m_mapImage.transform);
            Image mapOasisImage = OasisImage.GetComponent<Image>();

            mapOasisImage.rectTransform.anchoredPosition = new Vector2(m_mapImage.rectTransform.sizeDelta.x * oasisNormalPos.x, m_mapImage.rectTransform.sizeDelta.y * oasisNormalPos.y);
        }
    }

    private void Transport(Transform _oasisPos)
    {
        // PlayManager.SetCameraMode(EControlMode.UI_CONTROL);
        m_transportUI.gameObject.SetActive(true);
        Debug.Log("Transport!");

        Vector3 oasisOriginPos = new Vector3(_oasisPos.position.x * m_mapArea.x, 0, _oasisPos.position.y * m_mapArea.y);

        Button[] btns = m_transportUI.GetComponentsInChildren<Button>();
        // 예 버튼
        btns[0].onClick.AddListener(() =>
        {
            // PlayManager.TeleportPlayer(oasisOriginPos);
        });
        // 아니오 버튼
        btns[1].onClick.AddListener(() =>
        {
            m_transportUI.gameObject.SetActive(false);
        }
        );
    }

    private void SetComps()
    {
        m_mapOasis = GameObject.FindGameObjectsWithTag(ValueDefine.OASIS_TAG);

        // index -> 0 : left, 1 : right, 2 : bottom, 3 : top
        m_mapArea = new Vector2(Vector3.Distance(PlayManager.NormalizeObjects[0].position, PlayManager.NormalizeObjects[1].position),
            Vector3.Distance(PlayManager.NormalizeObjects[2].position, PlayManager.NormalizeObjects[3].position));
        SynchronizeOasisLocation();
        m_transportUI.gameObject.SetActive(false);

        Button[] oasisBtns = GetComponentsInChildren<Button>();
        for(uint i = 0; i < oasisBtns.Length; i++)
        {
            oasisBtns[i].onClick.AddListener(() =>
            {
                Transport(oasisBtns[i].gameObject.transform);
            });
        }
    }

    private void Update()
    {
        SynchronizePlayerLocation();
    }

    private void OnEnable()
    {
        SetComps();
    }
}
