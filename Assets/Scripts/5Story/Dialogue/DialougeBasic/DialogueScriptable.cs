using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct DialLine
{
    public string Text;
    public DialQuest TriggerQuest;
    public bool HasQuest { get { return !TriggerQuest.IsNull; } }
    public DialLine(string _line) : this(_line, DialQuest.Null) { }
    public DialLine(string _line, DialQuest _quest) { Text = _line; TriggerQuest = _quest; }
}
[Serializable]
public struct DialQuest
{
    public EQuestName Quest;
    public EDialQuestFunction Function;
    public bool IsNull { get { return Quest == EQuestName.LAST; } }
    public static DialQuest Null { get { return new(EQuestName.LAST, EDialQuestFunction.LAST); } }
    public DialQuest(EQuestName _quest, EDialQuestFunction _func) { Quest = _quest; Function = _func; }
}

public class DialogueScriptable : ScriptableObject
{
    public SNPC NPC;
    public int DialougeIdx;
    public int BranchIdx;
    public DialLine[] Lines;

    private DialQuest Data2Quest(string _id, string _func)
    {
        EQuestName quest = StoryManager.ID2Quest(_id);
        if(quest == EQuestName.LAST) { return DialQuest.Null; }
        EDialQuestFunction func = Data2Func(_func);
        if(func == EDialQuestFunction.LAST) { return DialQuest.Null; }
        return new(quest, func);

    }
    private EDialQuestFunction Data2Func(string _func)
    {
        return _func switch
        {
            "START" => EDialQuestFunction.START,
            "COMPLETE" => EDialQuestFunction.COMPLETE,
            "FINISH" => EDialQuestFunction.FNIINSH,

            _ => EDialQuestFunction.LAST
        };
    }

    public void SetScriptable(string _npc, List<string[]> _data)
    {
        NPC = StoryManager.String2NPC(_npc);
        int cnt = _data.Count;
        Lines = new DialLine[cnt];
        for (int i = 0; i<cnt; i++)
        {
            string[] data = _data[i];
            string line = data[(int)EDialogueAttributes.LINE_TEXT];
            DialQuest quest = Data2Quest(data[(int)EDialogueAttributes.RESULT_QUEST], data[(int)EDialogueAttributes.QUEST_FUNCTION]);
            Lines[i] = new(line, quest);
        }
        string[] firstData = _data[0];
        int.TryParse(firstData[(int)EDialogueAttributes.DIALOGUE_IDX], out DialougeIdx);
        int.TryParse(firstData[(int)EDialogueAttributes.BRANCH_IDX], out BranchIdx);
    }
}
