using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCScript : ObjectScript, IInteractable
{
    [SerializeField] 
    private string m_NPCName;      // npc �̸� ( �ΰ��� ǥ�ÿ� )  ->  ObjectScript ObjectInfo�� ����
    protected Transform m_NPCTransform;
    public string[] m_NPCDialogue;

    public EInteractType InteractType { get { return EInteractType.NPC; } }
    public bool Interactions { get { return gameObject.CompareTag(ValueDefine.NPC_TAG); } }
    public float UIOffset { get { return m_uiOffset; } }        // ��ȣ�ۿ� UI ��� ����
    public virtual string InfoTxt { get { return "��ȭ"; } }    // ��ȣ�ۿ� UI�� ��� �� => ���� ��Ȳ�� ���� �ٲ�� ��� ���ǹ� �߰� 

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

    public override void Start() 
    {

    }
}
