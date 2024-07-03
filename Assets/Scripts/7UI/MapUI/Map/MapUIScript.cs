using UnityEngine;
using UnityEngine.UI;

public class MapUIScript : BaseUI
{
    [SerializeField]
    private Image m_mapImg;
    [SerializeField]
    private RectTransform m_mapTrans;
    [SerializeField]
    private RectTransform m_player;
    [SerializeField]
    private RectTransform[] m_oasisList;
    [SerializeField]
    private RectTransform[] m_altartList;

    protected Vector2 MapImgSize { get { return m_mapTrans.sizeDelta; } }
    protected float MapHeight { get { return PlayManager.MapHeight; } }

    public override void OpenUI()
    {
        base.OpenUI();
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
        GameManager.UIControlInputs.CloseMapUI.started += delegate { CloseUI(); };
    }

    private void SetPlayerPos()
    {
        Vector2 pos = PlayManager.PlayerPos2;
        Vector2 offset = NormalizePos(pos);
        m_player.anchoredPosition = offset;
    }
    private void SetNPCPos()
    {
        OasisNPC[] oasisList = PlayManager.OasisList;
        for (int i = 0; i<(int)EOasisName.LAST; i++)
        {
            Vector2 pos = NormalizePos(oasisList[i].Position2);
            m_oasisList[i].anchoredPosition = pos;
        }
        AltarScript[] altarList = PlayManager.AltarList;
        for (int i = 0; i<(int)EAltarName.LAST; i++)
        {
            Vector2 pos = NormalizePos(altarList[i].Position2);
            m_altartList[i].anchoredPosition = pos;
        }
    }

    private Vector2 NormalizePos(Vector2 _pos)
    {
        Vector2 offset = new(MapImgSize.x * _pos.x / MapHeight, MapImgSize.y * _pos.y / MapHeight);
        offset = offset * 0.5f + MapImgSize * 0.25f;
        return offset;
    }


    public override void CloseUI()
    {
        GameManager.UIControlInputs.CloseMapUI.started -= delegate { CloseUI(); };
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
        base.CloseUI();
    }

    public override void SetComps()
    {
        SetNPCPos();
        SetPlayerPos();
    }
    private void Update()
    {
        SetPlayerPos();
    }
}
