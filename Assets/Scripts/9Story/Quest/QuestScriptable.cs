using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScriptable : ScriptableObject
{
    public uint Idx;
    public string Title;              // 퀘스트 이름
    public string Id;                 // 퀘스트 ID
    public string NextQuest;          // 다음 퀘스트 ID
    public EQuestType Type;           // 퀘스트 종류
    public string Description;        // 퀘스트 설명
   
    public string QuestObject;        // 퀘스트 시 필요한 오브젝트
    public int QuestObjectCount;      // 퀘스트 시 필요한 오브젝트 수
    public ERewardName Reward;             // 보상
    public int RewardNum;             // 보상 수 

    public EQuestStatus Status;     // 퀘스트 수행 상태
    public int CurQuestObjectCount;    // 현재 수집 / 사냥한 오브젝트 수 

    private EQuestType String2QuestType(string _data)
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

    private ERewardName String2Reward(string _data)
    {
        return _data switch
        {
            "SOUL" => ERewardName.SOUL,
            "STAT" => ERewardName.STAT,
            "ITEM" => ERewardName.ITEM,

            _ => ERewardName.LAST
        };
    }

    public void SetQuestScriptable(uint _idx, string[] _data)
    {
        Idx = _idx;
        Title = _data[(int)EQuestAttribute.TITLE];
        Id = _data[(int)EQuestAttribute.ID];
        NextQuest = _data[(int)EQuestAttribute.NEXTQUEST];
        Type = String2QuestType(_data[(int)EQuestAttribute.TYPE]);
        Description = _data[(int)EQuestAttribute.DESCRIPTION];
        QuestObject = _data[(int)EQuestAttribute.QUESTOBJECT];
        Reward = String2Reward(_data[(int)EQuestAttribute.REWARD]);
        int.TryParse(_data[(int)EQuestAttribute.OBJECTNUM], out QuestObjectCount);
        int.TryParse(_data[(int)EQuestAttribute.REWARDNUM], out RewardNum);

        Status = EQuestStatus.AVAILABLE;
        CurQuestObjectCount = 0;
    }
}
