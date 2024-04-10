using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHidable))]
public class HideScript : MonoBehaviour             // �Һ����� ��ȣ�ۿ��� ������ ������Ʈ�� �� ��ũ��Ʈ�� �־�� �Ѵ�. (IHidable)�� ����� ��ũ��Ʈ �ʼ�
{
    private IHidable m_hidable;             // ������Ʈ �� IHidable�� ����� ��ũ��Ʈ

    private int m_layerIdx;                 // ������Ʈ�� ���� layer

    private float DistToPlayer { get { return (PlayManager.PlayerPos-transform.position).magnitude; } }     // �÷��̾�� �Ÿ�
    public bool IsHiding { get; private set; }                                                                // �����ִ� ��������


    private void ChangeLayer(int _layer)            // ������Ʈ(�ڽ� ����) ��ü ���̾� �ٲٱ�
    {
        foreach (Transform tran in GetComponentsInChildren<Transform>())
        {
            tran.gameObject.layer = _layer;
        }
    }

    private void GetLight()                         // ���� �޾��� ��
    {
        ChangeLayer(m_layerIdx);
        m_hidable.GetLight();
        IsHiding = false;
    }
    private void LooseLight()                       // ���� �׸� ���� ��
    {
        ChangeLayer(ValueDefine.HIDING_LAYER_IDX);
        m_hidable.LooseLight();
        IsHiding = true;
    }

    private void CheckLight()                      // �����Ӹ��� �� ��ȭ ����
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