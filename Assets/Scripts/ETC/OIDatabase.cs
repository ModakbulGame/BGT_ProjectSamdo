using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName ="OIDatabase")]
public class OIDatabase : ScriptableObject
{
    [SerializeField]
    private List<ObjectIdentified> datas = new();

    public IReadOnlyList<ObjectIdentified> Datas => datas;
    public int Count => datas.Count;

    public ObjectIdentified this[int index] => datas[index];

    private void SetID(ObjectIdentified target, int id)
    {
        // IdentifiedObject�� id ���� ã�ƿ�
        // BindingFlags.NonPublic = �����ڰ� public�� �ƴϿ�����, BindingFlags.Instance = static type�� �ƴϿ�����
        var field = typeof(ObjectIdentified).GetField("id", BindingFlags.NonPublic | BindingFlags.Instance);
        // ������ id ���� ������ ���� target�� id ���� ���� ������
        field.SetValue(target, id);
        // Serialize ����(���⼭�� id ����)�� code�󿡼� �������� ��� EditorUtility.SetDirty�� ���ؼ� Serailize ������ �����Ǿ����� Unity�� �˷������
        // �׷��� ������ ������ ���� �ݿ����� �ʰ� ���� ������ ���ư�
        // ���⼭�� ���� �����Ǿ��ٰ� Unity�� �˷��ٻ�, ������ ���� ����ɷ��� Editor Code���� ApplyModifiedProperties �Լ� Ȥ��
        // ������Ʈ ��ü�� �����ϴ� AssetDatabase.SaveAssets �Լ��� ȣ��Ǿ����
        // ���⼭�� ���߿� �ٸ� ������ AssetDatabase.SaveAssets�� ȣ�� �� ���̱� ���� �ۼ����� ����.
#if UNITY_EDITOR
        EditorUtility.SetDirty(target);
#endif
    }

    // index ������ IdentifiedObjects�� id�� �缳����
    private void ReorderDatas()
    {
        var field = typeof(ObjectIdentified).GetField("id", BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0; i < datas.Count; i++)
        {
            field.SetValue(datas[i], i);
#if UNITY_EDITOR
            EditorUtility.SetDirty(datas[i]);
#endif
        }
    }

    public void Add(ObjectIdentified newData)
    {
        datas.Add(newData);
        SetID(newData, datas.Count - 1);
    }

    public void Remove(ObjectIdentified data)
    {
        datas.Remove(data);
        ReorderDatas();
    }

    public ObjectIdentified GetDataByID(int id) => datas[id];
    public T GetDataByID<T>(int id) where T : ObjectIdentified => GetDataByID(id) as T;

    public ObjectIdentified GetDataCodeName(string codeName) => datas.Find(item => item.CodeName == codeName);
    public T GetDataCodeName<T>(string codeName) where T : ObjectIdentified => GetDataCodeName(codeName) as T;

    public bool Contains(ObjectIdentified item) => datas.Contains(item);

    // Data�� CodeName�� �������� ������������ ������
    public void SortByCodeName()
    {
        datas.Sort((x, y) => x.CodeName.CompareTo(y.CodeName));
        ReorderDatas();
    }
}
