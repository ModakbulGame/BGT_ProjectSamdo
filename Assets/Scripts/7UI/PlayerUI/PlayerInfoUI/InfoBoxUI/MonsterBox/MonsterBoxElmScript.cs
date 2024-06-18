using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBoxElmScript : MonoBehaviour
{
    private Image m_monsterImg;
    private TextMeshProUGUI m_monsterNameTxt;
    private TextMeshProUGUI m_monsterStateTxt;


    public void SetMonsterInfo(EMonsterName _monster)
    {
        Sprite img = GameManager.GetMonsterSprite(_monster);
        MonsterInfo info = GameManager.GetMonsterInfo(_monster);

        m_monsterImg.sprite = img;
        m_monsterNameTxt.text = info.MonsterName;
        if (info.Cleared)
            m_monsterStateTxt.text = "완료";
        else
            m_monsterStateTxt.text = "미완료";
    }

    public void HideElm()
    {
        gameObject.SetActive(false);
    }

    private void SetComps()
    {
        m_monsterImg = GetComponentsInChildren<Image>()[1];
        m_monsterNameTxt = GetComponentsInChildren<TextMeshProUGUI>()[0];
        m_monsterStateTxt = GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    private void Awake()
    {
        SetComps();
    }
}
