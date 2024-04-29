using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBoxElmScript : MonoBehaviour
{
    private WeaponBoxUIScript m_parent;
    public void SetParent(WeaponBoxUIScript _parent) { m_parent = _parent; }

    private WeaponImgScript m_img;

    private EWeaponName ElmWeapon { get; set; }     // �Ҵ�� ����
    private bool IsCurWeapon { get; set; }          // �÷��̾ ���� ���� ��������

    private Image m_weaponImg;                      // ���� �̹���      
    private Button m_equipBtn;                      // ���� ��ư

    private TextMeshProUGUI m_weaponNameTxt;         // ���� �̸�

    public void SetWeaponInfo(int _weapon)          // ���� ����
    {
        EWeaponName weapon = (EWeaponName)_weapon;
        ElmWeapon = weapon;

        SItem item = new(EItemType.WEAPON, _weapon);

        Sprite img = GameManager.GetItemSprite(item);
        ItemInfo info = PlayManager.GetWeaponInfo(weapon);

        m_weaponImg.sprite = img;
        m_weaponNameTxt.text = info.ItemName;
        bool obtained = info.Obtained;
        IsCurWeapon = weapon == PlayManager.CurWeapon;
        SetBtnState(obtained);
    }

    private void SetBtnState(bool _obtained)        // ��ư ���� ����
    {
        if (IsCurWeapon) { SetBtnTxt("���� ��"); }
        else { SetBtnTxt("����"); }

        if (_obtained) { m_equipBtn.interactable = true; }
        else { m_equipBtn.interactable = false; }
    }
    private void SetBtnTxt(string _txt)             // ��ư �ؽ�Ʈ ����
    {
        m_equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = _txt;
    }

    public void HideElm()                           // �����
    {
        gameObject.SetActive(false);
    }


    public void ShowInfo()
    {
        m_parent.ShowInfoUI(ElmWeapon);
    }
    public void SetInfoPos(Vector2 _pos)
    {
        m_parent.SetInfoUIPos(_pos);
    }
    public void HideInfo()
    {
        m_parent.HideInfoUI();
    }


    private void EquipWeapon()                      // �Ҵ�� ���� ����
    {
        if(IsCurWeapon) { return; }
        m_parent.EquipWeapon(ElmWeapon);
    }


    private void SetBtn()
    {
        m_equipBtn.onClick.AddListener(EquipWeapon);
    }

    private void SetComps()
    {
        m_weaponImg = GetComponentsInChildren<Image>()[1];
        m_weaponNameTxt = GetComponentsInChildren<TextMeshProUGUI>()[0];
        m_equipBtn = GetComponentInChildren<Button>();
        m_img = GetComponentInChildren<WeaponImgScript>();
        m_img.SetParent(this);
        m_img.SetComps();
        SetBtn();
    }

    private void Awake()
    {
        SetComps();
    }
}
