using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCInfo : ObjectBaseInfo
{
    public int NPCID;
    public float UIOffset = 5;
    private void SetInfo(int _id)
    {
        NPCID = _id;
    }
}

public class NPCScript : MonoBehaviour, IInteractable
{
    [SerializeField]
    private NPCInfo m_npcInfo;
    [SerializeField]
    private NPCScriptable m_scriptable;
    public string NPCName { get { return m_npcInfo.ObjectName; } }
    public int NPCID { get { return m_npcInfo.NPCID; } }
    public void SetScriptable(NPCScriptable _scriptable) { m_scriptable = _scriptable; SetInfo(); }

    [SerializeField]
    private bool m_isQuestStarted;  // 시트에 따라 퀘스트가 존재하는 npc인 경우에는 내부처리로 조정 예정
    [SerializeField]
    private bool m_isQuestEnded;  // 얘도

    public bool IsQuestStarted { get { return m_isQuestStarted; } }   
    public bool IsQuestEnded { get { return m_isQuestEnded; } }

    protected Transform m_npcTransform;
    public string[] m_npcDialogue;

    public bool InteractableRotation 
    {
        get
        {
            Vector3 forward = transform.forward;                                // 오브젝트의 전방 벡터
            Vector3 toTarget = PlayManager.PlayerPos - transform.position;      // 플레이어 위치로의 벡터

            toTarget.Normalize();
            float angle = Vector3.SignedAngle(forward, toTarget, Vector3.up);
            return angle >= -60f && angle <= 60f;
        }
    }

    public EInteractType InteractType { get { return EInteractType.NPC; } }
    public bool Interactions { get { return gameObject.CompareTag(ValueDefine.NPC_TAG); } }
    public float UIOffset { get { return m_npcInfo.UIOffset; } }        // 상호작용 UI 띄울 높이
    public virtual string InfoTxt { get { return "대화"; } }    // 상호작용 UI에 띄울 말 => 말이 상황에 따라 바뀌는 경우 조건문 추가 

    private void SetInfo()
    {

    }

    

    public virtual void StartInteract() 
    {
        PlayManager.OpenNPCUI(this);
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
    }

    public virtual void StopInteract() 
    {
        PlayManager.StopPlayerInteract();
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
    }
}
