using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCScript : ObjectScript, IInteractable
{
    [SerializeField] private string m_NPCName;      // npc �̸� ( �ΰ��� ǥ�ÿ� )  ->  ObjectScript ObjectInfo�� ����
    [SerializeField] private Canvas m_NPCCanvas;    // npc ��ȭâ
    protected Transform m_NPCTransform;
    private Button m_exitBtn;

    public EInteractType InteractType { get { return EInteractType.NPC; } }
    public bool Interactions { get { return gameObject.CompareTag(ValueDefine.NPC_TAG); } }
    public float UIOffset { get { return m_uiOffset; } }        // ��ȣ�ۿ� UI ��� ����
    public virtual string InfoTxt { get { return "��ȭ"; } }    // ��ȣ�ۿ� UI�� ��� �� => ���� ��Ȳ�� ���� �ٲ�� ��� ���ǹ� �߰� 

    public virtual void StartInteract() 
    {
        m_NPCCanvas.gameObject.SetActive(true);
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
    }

    public virtual void StopInteract() 
    {
        PlayManager.StopPlayerInteract();
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
    }

    private void CancelInteract()
    {
        m_NPCCanvas.gameObject.SetActive(false);
        StopInteract();
    }

    public override void Start() 
    {
       m_exitBtn = GetComponentInChildren<Button>();
       // m_exitBtn.onClick.AddListener(CancelInteract);
    }
}
