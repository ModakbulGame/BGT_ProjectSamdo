using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EQuestAttributes
{
    ID,
    NAME,
    QUEST_TYPE,
    QUEST_DETAIL,
    QUEST_AMOUNT,
    TIME_LIMIT,
    RESULT_DIALOGUES,
    DESCRIPTION,
    COMPLETE_INFO,
    REWARD_TYPE,
    REWARD_NUM,
    REWARD_DETAIL,
    LAST
}

public enum EQuestType
{
    FIND,
    TALK,
    KILL,
    PURIFY,
    COLLECT,


    LAST
}

public enum EQuestState
{
    LOCKED,
    UNLOCKED,
    ACCEPTED,
    COMPLETE,
    FAIL,
    FINISH,

    LAST
}

public enum EQuestName
{
    Q1,
    Q2,
    Q3,


    LAST
}

public enum ERewarTyoe
{
    SOUL,
    PURIFIED,
    STAT,
    TRAIT,
    ITEM,

    LAST
}