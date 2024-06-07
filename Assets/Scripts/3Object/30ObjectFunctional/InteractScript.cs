using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInteractable))]
public class InteractScript : MonoBehaviour                 // ��ȣ�ۿ��� ������ ������Ʈ�� �ִ� ��ũ��Ʈ
{
    [SerializeField]
    private float m_canInteractDist = 2.5f;                                             // ��ȣ�ۿ� ���� �Ÿ�
    private float m_canInteractAngle = 120f;

    private IInteractable m_interactable;                                               // ������Ʈ �� IInteractable�� ����� ������Ʈ

    private InteractToggleUI m_interactToggleUI;                                        // ��ȣ�ۿ� ���� ���� UI
//    private NPCScript m_npc;

    public bool CanInteract { get { return DistToPlayer <= m_canInteractDist; } }  // ��ȣ�ۿ� ��������
/*    public bool CanInteractNPC 
    { 
        get { return DistToPlayer <= m_canInteractDist && m_npc.InteractableRotation; } 
    }*/

    public float DistToPlayer { get { return PlayManager.GetDistToPlayer(transform.position); } }           // �÷��̾���� �Ÿ�
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
        m_interactToggleUI.gameObject.SetActive(true);
    }
    private void HideToggleUI()                 // ���� UI �����
    {
        m_interactToggleUI.gameObject.SetActive(false);
    }


    public void StartInteract()                // ��ȣ�ۿ� ����
    {
        m_interactable.StartInteract();
        HideToggleUI();
    }
    public void StopInteract()                  // ��ȣ�ۿ� �ߴ�
    {
        m_interactable.StopInteract();
        ShowToggleUI();
    }


    private void SetToggleUI()
    {
        m_interactToggleUI = GetComponentInChildren<InteractToggleUI>();
        m_interactToggleUI.SetInfoTxt(m_interactable.InfoTxt);
        HideToggleUI();
    }

    private void Awake()
    {
        m_interactable = GetComponent<IInteractable>();
        // m_npc = GetComponent<NPCScript>();
    }
    private void Start()
    {
        SetToggleUI();
    }
}
