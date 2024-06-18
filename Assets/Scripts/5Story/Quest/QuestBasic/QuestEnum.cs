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

public enum EQuestStatus
{
    NOT_AVAILABLE,
    AVAILABLE,
    ACCEPTED,
    COMPLETE,
    FAIL,
    DONE,

    LAST
}

public enum EQuestEnum
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