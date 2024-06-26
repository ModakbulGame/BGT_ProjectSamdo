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
    protected TextMeshProUGUI m_mapName;
    public Image m_mapImg;

    protected RectTransform MapRect { get { return m_mapImg.rectTransform; } }
    protected float MapImgHeight { get { return m_mapImg.sprite.rect.height; } }

    protected float MapHeight { get { return PlayManager.MapHeight; } }

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
    protected virtual void Start()
    {
        m_mapName = GetComponentInChildren<TextMeshProUGUI>();
        m_mapName.text = SceneManager.GetActiveScene().name;
        InitSize();
        SetScale(MapScale);
        SetRotation();
    }
    protected virtual void Update()
    {
        SetPosition();
        SetRotation();
    }
}
