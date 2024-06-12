using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapUIScript : MonoBehaviour
{
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

    private float m_zoomIncrement = 0.1f;       // Ȯ�� ��Ҹ� ���� ����
    private float m_currentZoom = 1f;
    private float m_maxIncrement = 2.0f;
    private float m_minIncrement = 0.4f;

    private OasisNPC[] OasisList { get { return PlayManager.OasisList; } }


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
        // ī�޶�ó�� ����ٴϴ� ���� �����ϱ� ���� �� �̹����� �÷��̾� �̵������� �ݴ�������� �����̴� ������ ����
        m_mapImage.rectTransform.anchoredPosition = new Vector2(m_mapImage.rectTransform.sizeDelta.x * PlayManager.NormalizeLocation(m_targetPlayer).x * -1,
             m_mapImage.rectTransform.sizeDelta.y * PlayManager.NormalizeLocation(m_targetPlayer).y * -1);
    }

    private void SynchronizeOasisLocation()
    {
        for (uint i = 0; i < OasisList.Length; i++)
        {
            GameObject OasisImage = Instantiate(m_mapOasisImage, Vector3.zero, Quaternion.identity, m_mapImage.transform);
            Image mapOasisImage = OasisImage.GetComponent<Image>();
            Transform mapOasisTransform = OasisList[i].transform;

            mapOasisImage.rectTransform.anchoredPosition = new Vector2(m_mapImage.rectTransform.sizeDelta.x * PlayManager.NormalizeLocation(mapOasisTransform).x, 
                m_mapImage.rectTransform.sizeDelta.y * PlayManager.NormalizeLocation(mapOasisTransform).y);
        }
    }

    private void SetComps()
    {
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
