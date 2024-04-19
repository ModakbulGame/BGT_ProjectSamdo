using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHidable))]
public class HideScript : GetLightScript        // 불빛과의 상호작용이 가능한 오브젝트엔 이 스크립트를 넣어야 한다. (IHidable)을 상속한 스크립트 필수
{
    private IHidable m_hidable;             // 오브젝트 내 IHidable을 상속한 스크립트

    private int m_layerIdx;                 // 오브젝트의 원래 layer


    private void ChangeLayer(int _layer)            // 오브젝트(자식 포함) 전체 레이어 바꾸기
    {
        foreach (Transform tran in GetComponentsInChildren<Transform>())
        {
            tran.gameObject.layer = _layer;
        }
    }

    public override void GetLight()                         // 빛을 받았을 때
    {
        base.GetLight();
        ChangeLayer(m_layerIdx);
        m_hidable.GetLight();
    }
    public override void LoseLight()                       // 빛을 그만 받을 때
    {
        base.LoseLight();
        ChangeLayer(ValueDefine.HIDING_LAYER_IDX);
        m_hidable.LoseLight();
    }



    public override void SetComps()
    {
        base.SetComps();
        m_hidable = GetComponent<IHidable>();
        m_layerIdx = gameObject.layer;
    }
}