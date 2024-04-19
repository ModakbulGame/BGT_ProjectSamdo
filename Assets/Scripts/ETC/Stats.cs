using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EntityManager))]
public class Stats : MonoBehaviour
{
    [SerializeField]
    private StatManager hpStat;
    [SerializeField]
    private StatManager skillCostStat;
    // �ʿ��� cost ���� override �̿��Ͽ� ���� ����

    [Space]
    [SerializeField]
    private StatManagerOverride[] statManagerOverrides;

    private StatManager[] stats;

    public EntityManager Owner { get; private set; }
    public StatManager HPStat { get; private set; }
    public StatManager SkillCostStat { get; private set; }

    private void OnGUI()
    {
        // Debugging �뵵�� Player�� ���� Stat���� ȭ�鿡 ����ִ� �뵵�� OnGUI 

        // Player�� ���� return ���ش�
        if (!Owner.IsPlayer)
            return;

        // ���� ��ܿ� ���� Box�� �׷���
        GUI.Box(new Rect(2f, 2f, 250f, 250f), string.Empty);

        // Box �� �κп� Player Stat Text ������
        GUI.Label(new Rect(4f, 2f, 100f, 30f), "Plyaer Stat");

        var textRect = new Rect(4f, 22f, 200f, 30f);
        // Stat ������ ���� + button ���� ��ġ
        var plusButtonRect = new Rect(textRect.x + textRect.width, textRect.y, 20f, 20f);
        // stat ���ָ� ���� - button ���� ��ġ
        var minusButtonRect = plusButtonRect;
        minusButtonRect.x += 22f;

        foreach (var stat in stats)
        {
            // % type�� �� ���ϱ� 100���� 0~100 ���
            // 0.*** �϶�, �Ҽ��� 2°�ڸ�����
            // ����� �״��, ������ -�� ���δ�
            string defaultValueAsString = stat.IsPercentType ?
                $"{stat.DefaultValue * 100f:0.##;-0.##}%" :
                stat.DefaultValue.ToString("0.##;-0.##");

            string bonusValueAsString = stat.IsPercentType ?
                $"{stat.BonusValue * 100f:0.##;-0##}%" :
                stat.BonusValue.ToString("0.##;-0.##");

            GUI.Label(textRect, $"{stat.DisplayName}: {defaultValueAsString}({bonusValueAsString}");
            // + Button ���� �� ����
            if (GUI.Button(plusButtonRect, "+"))
            {
                if (stat.IsPercentType)
                    stat.DefaultValue += 0.01f;
                else
                    stat.DefaultValue += 1f;
            }

            // - Button�� ������ Stat ����
            if (GUI.Button(minusButtonRect, "-"))
            {
                if (stat.IsPercentType)
                    stat.DefaultValue -= 0.01f;
                else
                    stat.DefaultValue -= 1f;
            }

            // ���� Stat ���� ����� ���Ͽ� y�� ��ĭ ������
            textRect.y += 22f;
            plusButtonRect.y = minusButtonRect.y = textRect.y;
        }

    }
    public void Setup(EntityManager entity)
    {
        Owner = entity;

        stats = statManagerOverrides.Select(x => x.CreateStat()).ToArray();
        HPStat = hpStat ? GetStat(hpStat) : null;
    }

    public void OnDestroy()
    {
        foreach (var stat in stats)
            Destroy(stat);
        stats = null;
    }
    public StatManager GetStat(StatManager stat)
    {
        return stats.FirstOrDefault(x => x.ID == stat.ID);
    }

    public bool TryGetStat(StatManager stat, out StatManager outStat)
    {
        Debug.Assert(stat != null, $"Stats::TryGetStat - stat�� null�� �� �� �����ϴ�.");
        outStat = stats.FirstOrDefault(x => x.ID == stat.ID);
        return outStat != null;
    }
    public float GetValue(StatManager stat)
        => GetStat(stat).Value;
    public bool HasStat(StatManager stat)
    {
        Debug.Assert(stat != null, $"Stats::HasStat - stat�� null�� �� �� �����ϴ�");
        return stats.Any(x => x.ID == stat.ID);
    }

    public void SetDefaultValue(StatManager stat, float value)
        => GetStat(stat).DefaultValue = value;

    public float GetDefaultValue(StatManager stat)
        => GetStat(stat).DefaultValue;

    public void IncreaseDefaultValue(StatManager stat, float value)
        => GetStat(stat).DefaultValue += value;

    public void SetBonusValue(StatManager stat, object key, float value)
        => GetStat(stat).SetBonusValue(key, value);

    public void SetBonusValue(StatManager stat, object key, object subkey, float value)
        => GetStat(stat).SetBonusValue(key, subkey, value);

    public float GetBonusValue(StatManager stat)
        => GetStat(stat).BonusValue;

    public float GetBonusValue(StatManager stat, object key)
        => GetStat(stat).GetBonusValue(key);

    public void RemoveBonusValue(StatManager stat, object key)
        => GetStat(stat).RemoveBonusValue(key);

#if UNITY_EDITOR
    [ContextMenu("LoadStats")]
    private void LoadStats()
    {
        // Database���� Stat�� �����ͼ� StatOverride �迭�� ����� �ش�
        var stats = Resources.LoadAll<StatManager>("Stat").OrderBy(x => x.ID);
        statManagerOverrides = stats.Select(x => new StatManagerOverride(x)).ToArray();
    }
#endif
}
