using Pathfinding;
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
    public Image m_mapImg;

    protected OasisIconScript[] m_oasisPoints;
    protected AltarIconScript[] m_altarPoints;

    protected RectTransform MapRect { get { return m_mapImg.rectTransform; } }
    protected float MapImgHeight { get { return m_mapImg.sprite.rect.height; } }

    private Vector2 MapImgSize { get { return new(MapImgHeight, MapImgHeight); } }
    protected float MapHeight { get { return PlayManager.MapHeight; } }

    public float MapScale { get { return m_mapScale; } private set { m_mapScale = value; } }


    private void SetPosition()
    {
        Vector2 player = PlayManager.PlayerPos2;
        Vector2 playerOffset = player / MapHeight;

        Vector2 pivot = playerOffset * 0.5f + new Vector2(0.25f, 0.25f);
        MapRect.pivot = pivot;
    }

    private void SetRotation()
    {
        float player = PlayManager.CameraRotation;
        MapRect.localEulerAngles = new(0, 0, player);

        float gimic = 360 - player;
        foreach (OasisIconScript oasis in m_oasisPoints)
        {
            oasis.SetRotation(gimic);
        }
        foreach (AltarIconScript altar in m_altarPoints)
        {
            altar.SetRotation(gimic);
        }
    }

    private void SetMapGimicPosition()
    {
        OasisNPC[] oasisList = PlayManager.OasisList;
        for (int i=0;i<(int)EOasisName.LAST;i++)
        {
            Vector2 pos = NormalizePos(oasisList[i].Position2);
            m_oasisPoints[i].SetPosition(pos);
        }
        AltarScript[] altarList = PlayManager.AltarList;
        for (int i = 0; i<(int)EAltarName.LAST; i++)
        {
            Vector2 pos = NormalizePos(altarList[i].Position2);
            m_altarPoints[i].SetPosition(pos);
        }
    }
    private Vector2 NormalizePos(Vector2 _pos)
    {
        Vector2 mapSize = m_mapImg.GetComponent<RectTransform>().sizeDelta;
        Vector2 offset = new(mapSize.x * _pos.x / MapHeight, mapSize.y * _pos.y / MapHeight);
        offset = offset * 0.5f + MapImgSize * 0.25f;
        return offset;
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


    private void SetComps()
    {
        m_oasisPoints = GetComponentsInChildren<OasisIconScript>();
        foreach (OasisIconScript oasis in m_oasisPoints) { oasis.SetComps(); }
        m_altarPoints = GetComponentsInChildren<AltarIconScript>();
        foreach (AltarIconScript altar in m_altarPoints) { altar.SetComps(); }
    }

    private void Awake()
    {
        SetComps();
    }
    protected virtual void Start()
    {
        if (!GameManager.IsInGame) { return; }
        InitSize();
        SetMapGimicPosition();
        SetScale(MapScale);
        SetRotation();
    }
    protected virtual void Update()
    {
        if (!GameManager.IsInGame) { return; }
        SetPosition();
        SetRotation();
    }
}
