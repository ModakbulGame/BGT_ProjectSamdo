using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    private bool inTrigger = false;
    private QuestBoolStatus m_staus;

    public List<int> availableQuestIDs = new List<int>();
    public List<int> receivableQuestIDs = new List<int>();

    public GameObject questMaker;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == ValueDefine.PLAYER_TAG)
        {
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == ValueDefine.PLAYER_TAG)
        {
            inTrigger = false;
        }
    }
}
