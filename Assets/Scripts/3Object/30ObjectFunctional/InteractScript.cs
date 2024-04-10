using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInteractable), typeof(CapsuleCollider), typeof(Rigidbody))]
public class InteractScript : MonoBehaviour                 // 상호작용이 가능한 오브젝트에 넣는 스크립트
{
    [SerializeField]
    private float m_canInteractDist = 2.5f;                                             // 상호작용 가능 거리

    private IInteractable m_interactable;                                               // 오브젝트 내 IInteractable을 상속한 오브젝트

    private InteractToggleUI m_interactToggleUI;                                        // 상호작용 조작 띄우는 UI

    public bool CanInteract { get { return DistToPlayer <= m_canInteractDist; } }  // 상호작용 가능한지

    public float DistToPlayer { get { return PlayManager.GetDistToPlayer(transform.position); } }           // 플레이어와의 거리
 

    public void AbleInteract()                 // 조작 허용
    {
        ShowToggleUI();
    }
    public void DisableInteract()              // 조작 비허용
    {
        HideToggleUI();
    }
    private void ShowToggleUI()                 // 조작 UI 띄우기
    {
        m_interactToggleUI.gameObject.SetActive(true);
    }
    private void HideToggleUI()                 // 조작 UI 숨기기
    {
        m_interactToggleUI.gameObject.SetActive(false);
    }


    public void StartInteract()                // 상호작용 시작
    {
        m_interactable.StartInteract();
        HideToggleUI();
    }
    public void StopInteract()                  // 상호작용 중단
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
