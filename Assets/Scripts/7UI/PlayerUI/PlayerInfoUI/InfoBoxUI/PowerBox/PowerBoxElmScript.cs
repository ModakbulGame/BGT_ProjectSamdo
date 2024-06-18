using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerBoxElmScript : MonoBehaviour
{
    private PowerBoxUIScript m_parent;
    public void SetParent(PowerBoxUIScript _parent) { m_parent = _parent; }
    public PowerBoxUIScript Box { get { return m_parent; } }


    private Image m_skillImg;
    private TextMeshProUGUI m_skillName;

    private PowerBoxDragScript m_drag;

    public EPowerName CurPower { get; private set; }

    public void SetSkillInfo(EPowerName _skill, bool _equipped)
    {
        CurPower = _skill;
        Sprite img = GameManager.GetPowerSprite(_skill);
        PowerInfo info = GameManager.GetPowerInfo(_skill);

        m_skillImg.sprite = img;
        m_skillName.text = info.PowerName;
        if(_equipped) { m_skillName.color = Color.red; }
        else { m_skillName.color = Color.black; }
    }


    public void HideElm()
    {
        gameObject.SetActive(false);
    }


    private void SetComps()
    {
        m_skillImg = GetComponentsInChildren<Image>()[1];
        m_skillName = GetComponentInChildren<TextMeshProUGUI>();
        m_drag = GetComponentInChildren<PowerBoxDragScript>();
        m_drag.SetParent(this);
    }

    private void Awake()
    {
        SetComps();
    }
}
