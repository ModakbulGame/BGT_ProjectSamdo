using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBoxElmScript : MonoBehaviour
{
    private SkillBoxUIScript m_parent;
    public void SetParent(SkillBoxUIScript _parent) { m_parent = _parent; }
    public SkillBoxUIScript Box { get { return m_parent; } }


    private Image m_skillImg;
    private TextMeshProUGUI m_skillName;

    private SkillBoxDragScript m_drag;

    public ESkillName CurSkill { get; private set; }

    public void SetSkillInfo(ESkillName _skill, bool _equipped)
    {
        CurSkill = _skill;
        Sprite img = GameManager.GetSkillSprite(_skill);
        SkillInfo info = GameManager.GetSkillInfo(_skill);

        m_skillImg.sprite = img;
        m_skillName.text = info.SkillName;
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
        m_drag = GetComponentInChildren<SkillBoxDragScript>();
        m_drag.SetParent(this);
    }

    private void Awake()
    {
        SetComps();
    }
}
