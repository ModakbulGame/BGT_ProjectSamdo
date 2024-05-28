using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScriptable : ScriptableObject
{
    public uint Idx;
    public string Title;              // ����Ʈ �̸�
    public int Id;                    // ����Ʈ ID
    public EQuestType Type;           // ����Ʈ ����
    public string Description;        // ����Ʈ ����
    public int NextQuest;             // ���� ����Ʈ ID
        
    public string QuestObject;        // ����Ʈ �� �ʿ��� ������Ʈ
    public string Reward;             // ����
    public int RewardNum;             // ���� �� 

    private EQuestType Name2Type(EQuestType _questType)
    {
        if (_questType < EQuestType.TALKING)
            return EQuestType.TALKING;
        else if (_questType < EQuestType.COLLECTION)
            return EQuestType.COLLECTION;
        else if (_questType < EQuestType.HUNTING)
            return EQuestType.HUNTING;
        else if (_questType < EQuestType.TIMELIMIT)
            return EQuestType.TIMELIMIT;
        else return EQuestType.LAST;
    }

    public void SetQuestScriptable(uint _idx, string[] _data)
    {
        Idx = _idx;
        Title = _data[(int)EQuestAttribute.TITLE];
        Type = Name2Type(Type);
        Description = _data[(int)EQuestAttribute.DESCRIPTION];
        QuestObject = _data[(int)EQuestAttribute.QUESTOBJECT];
        Reward = _data[(int)EQuestAttribute.REWARD];
        int.TryParse(_data[(int)EQuestAttribute.ID], out Id);
        int.TryParse(_data[(int)EQuestAttribute.NEXTQUEST], out NextQuest);
        int.TryParse(_data[(int)EQuestAttribute.REWARDNUM], out RewardNum);
    }
}
