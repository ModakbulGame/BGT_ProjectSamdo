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

    public string NPCID { get; private set; }
    public string NPCName { get; private set; }
    public string[] NPCDialogues { get; private set; }

    public bool IsQuestStarted { get; set; }
    public bool IsQuestEnded { get; set; }

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
    public virtual string InfoTxt { get { return "대화"; } }            // 상호작용 UI에 띄울 말 => 말이 상황에 따라 바뀌는 경우 조건문 추가 

    private void SetInfo()
    {
        NPCID = m_scriptable.ID;
        NPCName = m_scriptable.NPCName;
        NPCDialogues = m_scriptable.Dialogues;
    }

    public virtual void StartInteract()
    {
        SetInfo();
        PlayManager.OpenNPCUI(this);
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
    }

    public virtual void StopInteract()
    {
        PlayManager.StopPlayerInteract();
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
    }

    private void Start()
    {
        PlayManager.SetQuestStartObjectStatus(NPCName);
        PlayManager.SetQuestEndObjectStatus(NPCName);
    }
}
