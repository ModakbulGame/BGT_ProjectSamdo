using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoUIScript : MonoBehaviour
{
    private RectTransform m_rect;
    private TextMeshProUGUI m_nameTxt;
    private TextMeshProUGUI m_descTxt;
    private TextMeshProUGUI m_elseTxt;

    private bool IsCompsSet { get; set; }

    public void ShowUI(SItem _item)
    {
        if (!IsCompsSet) { SetComps(); }
        ItemInfo info = PlayManager.GetItemInfo(_item);
        gameObject.SetActive(true);
        m_nameTxt.text = info.ItemName;
        m_descTxt.text = info.ItemDescription;
        m_elseTxt.text = SetItemInfo(info);
    }
    public string SetItemInfo(ItemInfo _info)
    {
        string itemTxt = "";

        if (_info.WeaponData != null)
        {
            itemTxt = "공격력 : " + _info.Attack.Min.ToString() + " ~ " + _info.Attack.Max.ToString() + '\n' + "마법 공격력 : " + _info.Magic.Min.ToString() + " ~ " + _info.Magic.Max.ToString() + '\n'
                + "공격속도 : " + _info.AttackSpeed.ToString(); 
        }
        else if (_info.ThrowItemData != null)
        {
            itemTxt = "데미지 : " + _info.ThrowDamage.ToString() + '\n' + "투척속도 : " + _info.ThrowSpeed.ToString() + '\n' + "대기시간 : " + _info.ExplodeTime.ToString();
        }

        return itemTxt;
    }
    public void SetPos(Vector2 _pos)
    {
        m_rect.anchoredPosition = _pos;
    }
    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void SetComps()
    {
        m_rect = GetComponent<RectTransform>();
        TextMeshProUGUI[] txts = GetComponentsInChildren<TextMeshProUGUI>();
        m_nameTxt = txts[0];
        m_descTxt = txts[1];
        m_elseTxt = txts[2];
        IsCompsSet = true;
    }

    private void Awake()
    {
        if(!IsCompsSet) { SetComps(); }
    }
}
