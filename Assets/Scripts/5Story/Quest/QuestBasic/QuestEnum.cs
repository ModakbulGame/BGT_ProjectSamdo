using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EQuestAttribute
{
    ID,
    NAME,
    QUEST_TYPE,
    QUEST_DETAIL,
    QUEST_AMOUNT,
    TIME_LIMIT,
    NEXT_QUEST,
    DESCRIPTION,
    REWARD_TYPE,
    REWARD_NUM,
    REWARD_DETAIL,
    START_NPC,
    COMPLETE_NPC,
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
    DONE,

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