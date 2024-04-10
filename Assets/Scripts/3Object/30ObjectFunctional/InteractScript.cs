using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInteractable), typeof(CapsuleCollider), typeof(Rigidbody))]
public class InteractScript : MonoBehaviour                 // ��ȣ�ۿ��� ������ ������Ʈ�� �ִ� ��ũ��Ʈ
{
    [SerializeField]
    private float m_canInteractDist = 2.5f;                                             // ��ȣ�ۿ� ���� �Ÿ�

    private IInteractable m_interactable;                                               // ������Ʈ �� IInteractable�� ����� ������Ʈ

    private InteractToggleUI m_interactToggleUI;                                        // ��ȣ�ۿ� ���� ���� UI

    public bool CanInteract { get { return DistToPlayer <= m_canInteractDist; } }  // ��ȣ�ۿ� ��������

    public float DistToPlayer { get { return PlayManager.GetDistToPlayer(transform.position); } }           // �÷��̾���� �Ÿ�
 

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
    }
    private void Start()
    {
        SetToggleUI();
    }
}
