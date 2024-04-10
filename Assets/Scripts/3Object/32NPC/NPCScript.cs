using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : ObjectScript, IInteractable
{
    [SerializeField] private string m_NPCName;      // npc 이름 ( 인게임 표시용 )  ->  ObjectScript ObjectInfo에 포함

    public EInteractType InteractType { get { return EInteractType.NPC; } }
    public bool Interactions { get { return gameObject.CompareTag(ValueDefine.NPC_TAG); } }
    public float UIOffset { get { return m_uiOffset; } }        // 상호작용 UI 띄울 높이
    public virtual string InfoTxt { get { return "대화"; } }    // 상호작용 UI에 띄울 말 => 말이 상황에 따라 바뀌는 경우 조건문 추가 

    public virtual void StartInteract() 
    {
        
    }

    public virtual void StopInteract() 
    {
        PlayManager.StopPlayerInteract();
    }

    public override void Start() { }
}
