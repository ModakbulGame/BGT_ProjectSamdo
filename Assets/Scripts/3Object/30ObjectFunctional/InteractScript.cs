using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInteractable))]
public class InteractScript : MonoBehaviour                 // ��ȣ�ۿ��� ������ ������Ʈ�� �ִ� ��ũ��Ʈ
{
    private Vector2 Position2 { get { return new(transform.position.x, transform.position.z); } }

    [SerializeField]
    private float m_canInteractDist = 2.5f;                                             // ��ȣ�ۿ� ���� �Ÿ�
    private float m_canInteractAngle = 120f;

    private IInteractable m_interactable;                                               // ������Ʈ �� IInteractable�� ����� ������Ʈ
    private QuestNPCScript m_questNPC;

    private float InteractAngle { get { if (m_interactable.InteractType == EInteractType.NPC) return m_canInteractAngle / 2; return m_canInteractAngle; } }
    public bool CanInteract { get { return DistToPlayer <= m_canInteractDist &&
                (m_interactable.InteractType != EInteractType.NPC || AngleToPlayer <= InteractAngle); } }  // ��ȣ�ۿ� ��������


    public float DistToPlayer { get { return PlayManager.GetDistToPlayer(transform.position); } }           // �÷��̾���� �Ÿ�
    public float AngleToPlayer { get {
            Vector2 dir = (Position2 - PlayManager.PlayerPos2).normalized;
            float rot = FunctionDefine.VecToDeg(dir);
            Vector2 forward = new(transform.forward.x, transform.forward.z);
            float fRot = FunctionDefine.VecToDeg(forward);
            float gap = rot - fRot;
            if (gap <= -360) { gap += 360; } else if(gap >= 360) { gap -= 360; }
            return gap; } }
    public Transform InteractTransform { get { return transform; } }                                        // ��ȣ�ۿ� ����� ��ġ


    public void AbleInteract()                 // ���� ���
    {
        ShowToggleUI();
    }
    public void DisableInteract()              // ���� �����
    {
        HideToggleUI();
    }
    private void ShowToggleUI()                 // ���� UI ����
    {
        PlayManager.ShowInteractInfo(m_interactable.InfoTxt);
    }
    private void HideToggleUI()                 // ���� UI �����
    {
        PlayManager.HideInteractInfo();
    }


    public void StartInteract()                // ��ȣ�ۿ� ����
    {
        m_interactable.StartInteract();
        if (m_questNPC != null) { PlayManager.SetNPCView(); }
        HideToggleUI();
    }
    public void StopInteract()                  // ��ȣ�ۿ� �ߴ�
    {
        m_interactable.StopInteract();
        ShowToggleUI();
    }

    private void Awake()
    {
        m_interactable = GetComponent<IInteractable>();
        m_questNPC = GetComponent<QuestNPCScript>();
    }
}
