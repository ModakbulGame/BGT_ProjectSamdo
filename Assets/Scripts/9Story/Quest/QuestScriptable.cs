using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScriptable : ScriptableObject
{
    public uint Idx;
    public string Title;              // 퀘스트 이름
    public int Id;                    // 퀘스트 ID
    public EQuestType Type;           // 퀘스트 종류
    public string Description;        // 퀘스트 설명
    public int NextQuest;             // 다음 퀘스트 ID
        
    public string QuestObject;        // 퀘스트 시 필요한 오브젝트
    public string Reward;             // 보상
    public int RewardNum;             // 보상 수 

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
