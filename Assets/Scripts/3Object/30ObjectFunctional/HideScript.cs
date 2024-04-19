using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHidable))]
public class HideScript : GetLightScript        // �Һ����� ��ȣ�ۿ��� ������ ������Ʈ�� �� ��ũ��Ʈ�� �־�� �Ѵ�. (IHidable)�� ����� ��ũ��Ʈ �ʼ�
{
    private IHidable m_hidable;             // ������Ʈ �� IHidable�� ����� ��ũ��Ʈ

    private int m_layerIdx;                 // ������Ʈ�� ���� layer


    private void ChangeLayer(int _layer)            // ������Ʈ(�ڽ� ����) ��ü ���̾� �ٲٱ�
    {
        foreach (Transform tran in GetComponentsInChildren<Transform>())
        {
            tran.gameObject.layer = _layer;
        }
    }

    public override void GetLight()                         // ���� �޾��� ��
    {
        base.GetLight();
        ChangeLayer(m_layerIdx);
        m_hidable.GetLight();
    }
    public override void LoseLight()                       // ���� �׸� ���� ��
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