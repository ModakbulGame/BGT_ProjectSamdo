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
    private float m_zoomMin = 1;  // ī�޶��� orthograpicSize �ּ� ũ��
    [SerializeField]
    private float m_zoomMax = 30;  // ī�޶��� orthograpicSize �ִ� ũ��
    [SerializeField]
    private float m_zoomStepSize = 1;  // 1ȸ �� �� ���� Ȥ�� ���ҵǴ� ��ġ
    [SerializeField]
    private TextMeshProUGUI textMapName;

    private void Awake()
    {
        // �� �̸��� ���� scene �̸����� ����
        textMapName.text = SceneManager.GetActiveScene().name;
    }

   public void ZoomIn()
    {
        // ī�޶� ���̴� �繰 ũ�� Ȯ��
        m_minimapCamera.orthographicSize = Mathf.Max(m_minimapCamera.orthographicSize - m_zoomStepSize, m_zoomMin);
    }

    public void ZoomOut()
    {
        // ī�޶� ���̴� �繰 ũ�� ���
        m_minimapCamera.orthographicSize = Mathf.Min(m_minimapCamera.orthographicSize + m_zoomStepSize, m_zoomMax);
    }

}
