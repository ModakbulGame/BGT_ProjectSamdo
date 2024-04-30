using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController
{
    [SerializeField]
    private InteractScript m_interactableObject;                                    // ��ȣ�ۿ� ������ ��ȣ�ۿ� �� ���

    private bool Interacting { get; set; }                                          // ��ȣ�ۿ� ������

    private void DetactObjectsNear()                                                // �ֺ� ��ȣ�ۿ� ���� ������Ʈ Ž��
    {
        if(Interacting) { return; } // ��ȣ�ۿ� ������ ���

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit[] hits = Physics.RaycastAll(ray, 10, ValueDefine.INTERACT_LAYER);

        InteractScript interact = null; // ��ȣ�ۿ� ��� Ž��
        for (int i = 0; i<hits.Length; i++) // ��ó ��� Ȯ��
        {
            GameObject obj = hits[i].collider.gameObject;
            InteractScript script = obj.GetComponentInParent<InteractScript>()
                ??obj.GetComponentInChildren<InteractScript>();
            if (script == null || !script.CanInteract) { continue; }    // ��ũ��Ʈ�� ���ų� ��ȣ�ۿ� �Ұ����� ���
            interact = script;
        }
        if (interact != null && interact != m_interactableObject)   // ����� �ٲ� ���
        {
            if(m_interactableObject != null)
                m_interactableObject.DisableInteract();

            m_interactableObject = interact;
            m_interactableObject.AbleInteract();
        }
        if (m_interactableObject != null && !m_interactableObject.CanInteract)  // ����� ����� ���
        {
            m_interactableObject.DisableInteract();
            m_interactableObject = null;
        }
    }

    public void StartInteract()                                                     // ��ȣ�ۿ� ����
    {
        Interacting = true;
    }
    public void StopInteract()                                                      // ��ȣ�ۿ� �ߴ�
    {
        Interacting = false;
    }

    private void PlayerDetactUpdate()                                               // Ž�� ���� ������Ʈ
    {
        DetactObjectsNear();
        if (m_interactableObject != null && PlayerInput.Interact.triggered)
        {
            StartInteract();
            m_interactableObject.StartInteract();
        }
    }
}
