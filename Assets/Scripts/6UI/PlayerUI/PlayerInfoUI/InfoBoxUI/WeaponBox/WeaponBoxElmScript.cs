using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBoxElmScript : MonoBehaviour
{
    private WeaponBoxUIScript m_parent;
    public void SetParent(WeaponBoxUIScript _parent) { m_parent = _parent; }


    private EWeaponName ElmWeapon { get; set; }     // �Ҵ�� ����
    private bool IsCurWeapon { get; set; }          // �÷��̾ ���� ���� ��������

    private Image m_weaponImg;                      // ���� �̹���
    private TextMeshProUGUI m_weaponNameTxt;        // ���� �̸�
    private Button m_equipBtn;                      // ���� ��ư


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
        SetBtn();
    }

    private void Awake()
    {
        SetComps();
    }
}
