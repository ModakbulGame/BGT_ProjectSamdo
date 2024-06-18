using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
    [SerializeField]
    private float m_mapScale = 8;
    [SerializeField]
    private Image m_mapImg;
    private TextMeshProUGUI m_mapName;
    private RectTransform MapRect { get { return m_mapImg.rectTransform; } }
    private float MapImgHeight { get { return m_mapImg.sprite.rect.height; } }

    private float MapHeight { get { return PlayManager.MapHeight; } }

    public float MapScale { get { return m_mapScale; } private set { m_mapScale = value; } }


    private void SetPosition()
    {
        Vector2 player = PlayManager.PlayerPos2;
        Vector2 playerOffset = player / MapHeight;
        MapRect.pivot = playerOffset;
    }

    private void SetRotation()
    {
        float player = PlayManager.CameraRotation;
        MapRect.localEulerAngles = new(0, 0, player);
    }



    public void SetScale(float _scale)
    {
        m_mapScale = _scale;
        float realScale = MapHeight / MapImgHeight * _scale;
        m_mapImg.rectTransform.localScale = new(realScale, realScale, 1);
        SetPosition();
    }

    private void InitSize()
    {
        MapRect.sizeDelta = new(MapImgHeight, MapImgHeight);
    }
    private void Start()
    {
        m_mapName = GetComponentInChildren<TextMeshProUGUI>();
        m_mapName.text = SceneManager.GetActiveScene().name;
        InitSize();
        SetScale(MapScale);
        SetRotation();
    }
    private void Update()
    {
        SetPosition();
        SetRotation();
    }
}
