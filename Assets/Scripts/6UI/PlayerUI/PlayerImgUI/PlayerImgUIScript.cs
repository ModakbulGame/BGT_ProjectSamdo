using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerImgUIScript : MonoBehaviour
{
    [SerializeField]
    private Transform m_weaponTransform;
    [SerializeField]
    private Image m_weaponSlotImg;

    private GameObject CurWeapon { get; set; }

    public void OpenUI()
    {
        updatePlayerWeapon(PlayManager.CurWeapon);
    }

    public void updatePlayerWeapon(EWeaponName _weapon)
    {
        if (CurWeapon != null) { Destroy(CurWeapon); }
        GameObject weapon = Instantiate(PlayManager.GetWeaponPrefab(_weapon), m_weaponTransform);
        Destroy(weapon.GetComponent<WeaponScript>());

        Transform[] transforms = weapon.GetComponentsInChildren<Transform>();
        foreach (Transform transform in transforms)
            transform.gameObject.layer = ValueDefine.UI_LAYER_IDX;

        m_weaponSlotImg.sprite = GameManager.GetItemSprite(new(EItemType.WEAPON, (int)_weapon));

        CurWeapon = weapon;
    }



    public void SetComps()
    {

    }
}
