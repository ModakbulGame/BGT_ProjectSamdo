using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHidable))]
public class HideScript : MonoBehaviour             // 불빛과의 상호작용이 가능한 오브젝트엔 이 스크립트를 넣어야 한다. (IHidable)을 상속한 스크립트 필수
{
    private IHidable m_hidable;             // 오브젝트 내 IHidable을 상속한 스크립트

    private int m_layerIdx;                 // 오브젝트의 원래 layer

    private float DistToPlayer { get { return (PlayManager.PlayerPos-transform.position).magnitude; } }     // 플레이어와 거리
    public bool IsHiding { get; private set; }                                                                // 숨어있는 상태인지


    private void ChangeLayer(int _layer)            // 오브젝트(자식 포함) 전체 레이어 바꾸기
    {
        foreach (Transform tran in GetComponentsInChildren<Transform>())
        {
            tran.gameObject.layer = _layer;
        }
    }

    private void GetLight()                         // 빛을 받았을 때
    {
        ChangeLayer(m_layerIdx);
        m_hidable.GetLight();
        IsHiding = false;
    }
    private void LooseLight()                       // 빛을 그만 받을 때
    {
        ChangeLayer(ValueDefine.HIDING_LAYER_IDX);
        m_hidable.LooseLight();
        IsHiding = true;
    }

    private void CheckLight()                      // 프레임마다 빛 변화 감지
    {
        ELightState state = PlayerLightScript.CurState;
        float size = PlayerLightScript.CurSize;
        if (IsHiding)
        {
            if (state == ELightState.ON || 
                (state == ELightState.CHANGE && size >= DistToPlayer))
            {
                GetLight();
            }
        }
        else
        {
            if (state == ELightState.OFF ||
                state == ELightState.CHANGE && size <= DistToPlayer)
            {
                LooseLight();
            }
        }
    }


    private void Awake()
    {
        m_hidable = GetComponent<IHidable>();
        m_layerIdx = gameObject.layer;
    }

    private void Start()
    {
        LooseLight();
    }

    private void Update()
    {
        CheckLight();
    }
}