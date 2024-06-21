using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EAlarmType
{
    SIDE_TEXT,              // ȭ�� ���̵� �ؽ�Ʈ �˶�
    LAST
}

public class UIManager : MonoBehaviour
{
    // UI�� �˶� ������ -> UI�� �����̳� ��ȣ�ۿ� ����, �˶��� �ѹ� ���� �� (��� ���� ��Ȯ���� ������ ������ �������� �������� �� ���Ƽ� ������)

    [SerializeField]
    private GameObject[] m_alarmPrefabs = new GameObject[(int)EAlarmType.LAST];                     // �˶� �������
    public GameObject GetAlarmPrefab(EAlarmType _alarm) { return m_alarmPrefabs[(int)_alarm]; }


    public Sprite GetMonsterSprite(EMonsterName _monster)
    {
        return GameManager.GetMonsterData(_monster).MonsterProfile;
    }

    public Sprite GetItemSprite(SItem _item)
    {
        return GameManager.GetItemData(_item).ItemIcon;
    }

    public Sprite GetPowerSprite(EPowerName _skill)
    {
        return GameManager.GetPowerData(_skill).PowerIcon;
    }
}