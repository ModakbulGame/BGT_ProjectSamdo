using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EntityManager))]
public class Stats : MonoBehaviour
{
    [SerializeField]
    private StatManager hpStat;
    [SerializeField]
    private StatManager skillCostStat;
    // 필요한 cost 마다 override 이용하여 제한 가능

    [Space]
    [SerializeField]
    private StatManagerOverride[] statManagerOverrides;

    private StatManager[] stats;

    public EntityManager Owner { get; private set; }
    public StatManager HPStat { get; private set; }
    public StatManager SkillCostStat { get; private set; }

    private void OnGUI()
    {
        // Debugging 용도로 Player가 가진 Stat들을 화면에 띄워주는 용도의 OnGUI 

        // Player일 때만 return 해준다
        if (!Owner.IsPlayer)
            return;

        // 좌측 상단에 넓은 Box를 그려줌
        GUI.Box(new Rect(2f, 2f, 250f, 250f), string.Empty);

        // Box 윗 부분에 Player Stat Text 보여줌
        GUI.Label(new Rect(4f, 2f, 100f, 30f), "Plyaer Stat");

        var textRect = new Rect(4f, 22f, 200f, 30f);
        // Stat 증가를 위한 + button 기존 위치
        var plusButtonRect = new Rect(textRect.x + textRect.width, textRect.y, 20f, 20f);
        // stat 감솔르 위한 - button 기존 위치
        var minusButtonRect = plusButtonRect;
        minusButtonRect.x += 22f;

        foreach (var stat in stats)
        {
            // % type일 때 곱하기 100으로 0~100 출력
            // 0.*** 일때, 소수점 2째자리까지
            // 양수면 그대로, 음수면 -를 붙인다
            string defaultValueAsString = stat.IsPercentType ?
                $"{stat.DefaultValue * 100f:0.##;-0.##}%" :
                stat.DefaultValue.ToString("0.##;-0.##");

            string bonusValueAsString = stat.IsPercentType ?
                $"{stat.BonusValue * 100f:0.##;-0##}%" :
                stat.BonusValue.ToString("0.##;-0.##");

            GUI.Label(textRect, $"{stat.DisplayName}: {defaultValueAsString}({bonusValueAsString}");
            // + Button 누를 시 증가
            if (GUI.Button(plusButtonRect, "+"))
            {
                if (stat.IsPercentType)
                    stat.DefaultValue += 0.01f;
                else
                    stat.DefaultValue += 1f;
            }

            // - Button을 누르면 Stat 감소
            if (GUI.Button(minusButtonRect, "-"))
            {
                if (stat.IsPercentType)
                    stat.DefaultValue -= 0.01f;
                else
                    stat.DefaultValue -= 1f;
            }

            // 다음 Stat 정보 출력을 위하여 y축 한칸 내린다
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
        Debug.Assert(stat != null, $"Stats::TryGetStat - stat은 null이 될 수 없습니다.");
        outStat = stats.FirstOrDefault(x => x.ID == stat.ID);
        return outStat != null;
    }
    public float GetValue(StatManager stat)
        => GetStat(stat).Value;
    public bool HasStat(StatManager stat)
    {
        Debug.Assert(stat != null, $"Stats::HasStat - stat은 null이 될 수 없습니다");
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
        // Database에서 Stat을 가져와서 StatOverride 배열을 만들어 준다
        var stats = Resources.LoadAll<StatManager>("Stat").OrderBy(x => x.ID);
        statManagerOverrides = stats.Select(x => new StatManagerOverride(x)).ToArray();
    }
#endif
}
