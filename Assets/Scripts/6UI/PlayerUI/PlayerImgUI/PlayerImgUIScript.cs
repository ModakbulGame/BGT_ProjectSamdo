using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerImgUIScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_weapons;
    [SerializeField]
    private Image m_weaponSlotImg;

    private GameObject CurWeapon { get; set; }

    public void OpenUI()
    {
        UpdatePlayerWeapon(PlayManager.CurWeapon);
    }

    public void UpdatePlayerWeapon(EWeaponName _weapon)
    {
        if (CurWeapon != null) { CurWeapon.SetActive(false); }
        CurWeapon = m_weapons[(int)_weapon];
        CurWeapon.SetActive(true);

        m_weaponSlotImg.sprite = GameManager.GetItemSprite(new(EItemType.WEAPON, (int)_weapon));
    }



    public void SetComps()
    {

    }
}
