using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotElmUIScript : MonoBehaviour
{
    private Image m_skillImg;
    private Image m_cooltimeImg;



    public void SetSkill(ESkillName _skill)
    {
        Sprite skillImg = GameManager.GetSkillSprite(_skill);
        m_skillImg.sprite = skillImg;
    }

    public void UseSkill(float _cooltime)
    {
        StartCoroutine(ShowCooltime(_cooltime));
    }
    private IEnumerator ShowCooltime(float _cooltime)
    {
        float cnt = _cooltime;
        while (cnt > 0)
        {
            cnt-=Time.deltaTime;
            float per = cnt / _cooltime;
            SetCooltime(per);
            yield return null;
        }
    }
    private void SetCooltime(float _per)
    {
        m_cooltimeImg.fillAmount = _per;
    }



    public void SetComps()
    {
        m_skillImg = GetComponentsInChildren<Image>()[1];
        m_cooltimeImg = GetComponentsInChildren<Image>()[2];
    }
}
