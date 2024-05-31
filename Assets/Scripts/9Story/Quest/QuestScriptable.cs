using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScriptable : ScriptableObject
{
    public uint Idx;
    public string Title;              // ����Ʈ �̸�
    public string Id;                 // ����Ʈ ID
    public string NextQuest;          // ���� ����Ʈ ID
    public EQuestType Type;           // ����Ʈ ����
    public string Description;        // ����Ʈ ����
   
    public string QuestObject;        // ����Ʈ �� �ʿ��� ������Ʈ
    public int QuestObjectCount;      // ����Ʈ �� �ʿ��� ������Ʈ ��
    public string Reward;             // ����
    public int RewardNum;             // ���� �� 

    public EQuestStatus Status;     // ����Ʈ ���� ����
    public int CurQuestObjectCount;    // ���� ���� / ����� ������Ʈ �� 

    private EQuestType String2Type(string _data)
    {
        return _data switch
        {
            "TALKING" => EQuestType.TALKING,
            "COLLECTION" => EQuestType.COLLECTION,
            "HUNTING" => EQuestType.HUNTING,
            "TIMELIMIT" => EQuestType.TIMELIMIT,

            _ => EQuestType.LAST
        };
    }

    public void SetQuestScriptable(uint _idx, string[] _data)
    {
        Idx = _idx;
        Title = _data[(int)EQuestAttribute.TITLE];
        Id = _data[(int)EQuestAttribute.ID];
        NextQuest = _data[(int)EQuestAttribute.NEXTQUEST];
        Type = String2Type(_data[(int)EQuestAttribute.TYPE]);
        Description = _data[(int)EQuestAttribute.DESCRIPTION];
        QuestObject = _data[(int)EQuestAttribute.QUESTOBJECT];
        Reward = _data[(int)EQuestAttribute.REWARD];
        int.TryParse(_data[(int)EQuestAttribute.OBJECTNUM], out QuestObjectCount);
        int.TryParse(_data[(int)EQuestAttribute.REWARDNUM], out RewardNum);

        Status = EQuestStatus.NOT_AVAILABLE;
        CurQuestObjectCount = 0;
    }
}
