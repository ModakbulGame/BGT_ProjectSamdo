using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OasisIconScript : MonoBehaviour
{
    private MapUIScript m_parent;
    private Image m_oasisIcon;

    public void SetParent(MapUIScript _parent) { m_parent = _parent; }
    private void InitOasisPosition(Image _oasis)
    {
        m_parent.InitOasisPosition(_oasis);
    }

    public void SetComps()
    {
        m_oasisIcon = GetComponent<Image>();
        InitOasisPosition(m_oasisIcon);
    }
}
