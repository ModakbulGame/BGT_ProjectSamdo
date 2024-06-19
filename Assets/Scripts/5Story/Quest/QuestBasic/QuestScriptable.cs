using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScriptable : ScriptableObject
{
    public uint Idx;
    public string Id;                           // ����Ʈ ID
    public EQuestName Enum;
    public string Name;                         // ����Ʈ �̸�
    public QuestContent Content;                // ����Ʈ ����
    public float TimeLimit;                     // ���� �ð�
    public EQuestName NextQuest;                // ���� ����Ʈ
    public string Description;                  // ����Ʈ ����
    public QuestReward Reward;                  // ����
    public NPCDialogue StartDialogue;           // ����Ʈ�� �ִ� ��ȭ
    public NPCDialogue FinishDialogue;          // ����Ʈ �Ϸ� ��ȭ

    private QuestContent String2Content(string _type, string _detail, string _amount)
    {
        float.TryParse(_amount, out float amount);
        switch (_type)
        {
            case "FIND":
                SNPC npc = StoryManager.String2NPC(_detail);
                if(!npc.IsNull) { return new(EQuestType.FIND, npc); }
                EMonsterName monster = MonsterManager.ID2Monster(_detail);
                if(monster != EMonsterName.LAST) { return new(EQuestType.FIND, monster, 1); }
                break;
            case "TALK":
                return new(EQuestType.TALK, StoryManager.String2NPC(_detail));
            case "KILL":
                return new(EQuestType.KILL, MonsterManager.ID2Monster(_detail), amount);
            case "PURIFY":
                return new(EQuestType.PURIFY, MonsterManager.ID2Monster(_detail), amount);
            case "COLLECT":
                return new(EQuestType.COLLECT, ItemManager.ID2Item(_detail), amount);

        }
        Debug.LogError("����Ʈ ���� �߸� �Էµ�");
        return QuestContent.Null;
    }
    private QuestReward String2Reward(string _type, string _num, string _detail)
    {
        int.TryParse(_num, out int num);
        switch (_type)
        {
            case "SOUL":
                return new(ERewarTyoe.SOUL, num);
            case "PURIFIED":
                return new(ERewarTyoe.PURIFIED, num);
            case "STAT":
                EStatName stat = PlayerForceManager.String2Stat(_detail);
                if (stat == EStatName.LAST) { return new(ERewarTyoe.STAT, num); }
                else { return new(stat, num); }
            case "TRAIT":
                // �̱���
                break;
            case "ITEM":
                SItem item = ItemManager.ID2Item(_detail);
                return new(item, num);
        }
        Debug.LogError("���� Ÿ�� �߸� �Էµ�");
        return QuestReward.Null;
    }
    private NPCDialogue String2Dialogue(string _data)
    {
        string[] data = _data.Split('_');
        SNPC npc = StoryManager.String2NPC(data[0]);
        int.TryParse(data[1], out int idx);
        return new(npc, idx);
    }

    public void SetQuestScriptable(uint _idx, string[] _data)
    {
        Idx =               _idx;
        Id =                _data[(int)EQuestAttribute.ID];
        Enum =              (EQuestName)Idx;
        Name =              _data[(int)EQuestAttribute.NAME];
        Content =           String2Content(_data[(int)EQuestAttribute.QUEST_TYPE], _data[(int)EQuestAttribute.QUEST_DETAIL], _data[(int)EQuestAttribute.QUEST_AMOUNT]);
        float.TryParse(     _data[(int)EQuestAttribute.TIME_LIMIT], out TimeLimit);
        NextQuest =         QuestManager.String2Enum(_data[(int)EQuestAttribute.NEXT_QUEST]);
        Description =       _data[(int)EQuestAttribute.DESCRIPTION];
        Reward =            String2Reward(_data[(int)EQuestAttribute.REWARD_TYPE], _data[(int)EQuestAttribute.REWARD_NUM], _data[(int)EQuestAttribute.REWARD_DETAIL]);
        StartDialogue =     String2Dialogue(_data[(int)EQuestAttribute.START_DIALOGUE]);
        FinishDialogue =    String2Dialogue(_data[(int)EQuestAttribute.FINISH_DIALOGUE]);
    }
}
