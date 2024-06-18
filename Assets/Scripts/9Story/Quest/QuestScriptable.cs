using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScriptable : ScriptableObject
{
    public uint Idx;
    public string Title;              // 퀘스트 이름
    public EQuestEnum Enum;
    public string Id;                 // 퀘스트 ID
    public string NextQuest;          // 다음 퀘스트 ID
    public EQuestType[] Types;        // 퀘스트 종류
    public string Description;        // 퀘스트 설명
    public float TimeLimit;           // 제한 시간
   
    public string QuestObject;        // 퀘스트 시 필요한 오브젝트
    public int QuestObjectCount;      // 퀘스트 시 필요한 오브젝트 수
    public ERewardName Reward;        // 보상
    public int RewardNum;             // 보상 수 

    public string StartObject;        // 퀘스트를 주는 오브젝트
    public string EndObject;          // 퀘스트 수행 중 거치거나 완료하는 오브젝트

    private ERewardName String2Reward(string _data)
    {
        return _data switch
        {
            "영혼" => ERewardName.SOUL,
            "스탯" => ERewardName.STAT,
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
