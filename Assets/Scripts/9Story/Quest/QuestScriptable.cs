using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScriptable : ScriptableObject
{
    public uint Idx;
    public string Title;              // ����Ʈ �̸�
    public EQuestEnum Enum;
    public string Id;                 // ����Ʈ ID
    public string NextQuest;          // ���� ����Ʈ ID
    public EQuestType[] Types;        // ����Ʈ ����
    public string Description;        // ����Ʈ ����
    public float TimeLimit;           // ���� �ð�
   
    public string QuestObject;        // ����Ʈ �� �ʿ��� ������Ʈ
    public int QuestObjectCount;      // ����Ʈ �� �ʿ��� ������Ʈ ��
    public ERewardName Reward;        // ����
    public int RewardNum;             // ���� �� 

    public string StartObject;        // ����Ʈ�� �ִ� ������Ʈ
    public string EndObject;          // ����Ʈ ���� �� ��ġ�ų� �Ϸ��ϴ� ������Ʈ

    private ERewardName String2Reward(string _data)
    {
        return _data switch
        {
            "��ȥ" => ERewardName.SOUL,
            "����" => ERewardName.STAT,
            "ITEM" => ERewardName.ITEM,

            _ => ERewardName.LAST
        };
    }

    public void SetQuestScriptable(uint _idx, string[] _data)
    {
        Idx = _idx;
        Id = _data[(int)EQuestAttribute.ID];
        Enum = (EQuestEnum)Idx;
        Title = _data[(int)EQuestAttribute.TITLE];
        Types = DataManager.String2QuestTypes(_data[(int)EQuestAttribute.TYPE]);
        Description = _data[(int)EQuestAttribute.DESCRIPTION];
        NextQuest = _data[(int)EQuestAttribute.NEXTQUEST];
        QuestObject = _data[(int)EQuestAttribute.QUESTOBJECT];
        Reward = String2Reward(_data[(int)EQuestAttribute.REWARD]);
        StartObject = _data[(int)EQuestAttribute.STARTOBJECT];
        EndObject = _data[(int)EQuestAttribute.ENDOBJECT];
        int.TryParse(_data[(int)EQuestAttribute.OBJECTNUM], out QuestObjectCount);
        int.TryParse(_data[(int)EQuestAttribute.REWARDNUM], out RewardNum);
        float.TryParse(_data[(int)EQuestAttribute.TIMELIMIT], out TimeLimit);
    }
}
