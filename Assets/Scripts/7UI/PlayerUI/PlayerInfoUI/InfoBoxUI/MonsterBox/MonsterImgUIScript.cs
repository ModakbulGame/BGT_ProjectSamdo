using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterImgUIScript : MonoBehaviour
{
    private Image m_monsterImg;
    private TextMeshProUGUI m_monsterNameTxt;

    private PlayerUIScript m_parent;
    public void SetParent(PlayerUIScript _parent) { m_parent = _parent; }

    public void SetMonsterInfo(EMonsterName _monster)
    {
        Sprite img = GameManager.GetMonsterSprite(_monster);
        MonsterInfo info = GameManager.GetMonsterInfo(_monster);

        m_monsterImg.sprite = img;
        m_monsterNameTxt.text = info.MonsterName;
    }
}
