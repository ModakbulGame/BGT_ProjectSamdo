using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELightState
{
    OFF,
    CHANGE,
    ON
}

public class TempLightScript : MonoBehaviour            // �ӽ� �Һ� ������Ʈ
{
    private static TempLightScript Inst;

    private const float MinSize = 0;                            // �ּ� ũ��
    private const float MaxSize = 100;                          // �ִ� ũ��

    private Transform PlayerTransform { get; set; }             // �÷��̾� ��ġ
    public static ELightState CurState { get; private set; }    // �Һ��� ���� ���� (static)
    public static float CurSize { get; private set; }           // ���� ũ�� (static)
    private float ChangeSpeed { get; set; }                     // ũ�� ��ȭ �ӵ�
    private const float ChangeTime = 1;                         // ũ�� ��ȭ �ð�

    private void SetSize(float _size)                           // ũ�� ����
    {
        CurSize = _size;
        transform.localScale = new(_size, _size, _size);
    }
    public void SetPlayerTransform(Transform _transform)        // �÷��̾� �Ѿư��� ����
    {
        PlayerTransform = _transform;
        TrackPos();
    }

    private void TrackPos()                                     // ��ġ �Ѿư���
    {
        transform.position = PlayerTransform.position;
    }

    private void StartLight()                                   // �� ������ ����
    {
        CurState = ELightState.CHANGE;
        SetSize(MinSize);
        ChangeSpeed = (MaxSize - MinSize) / ChangeTime;
        StartCoroutine(LightExpand());
    }
    public void EndLight()                                      // �� ������ ����
    {
        gameObject.SetActive(true);
        CurState = ELightState.CHANGE;
        StartCoroutine(LightShrink());
    }
    private IEnumerator LightExpand()                           // ũ�� ���� �ڷ�ƾ
    {
        while (CurSize < MaxSize)
        {
            TrackPos();
            SetSize(CurSize + ChangeSpeed * Time.deltaTime);
            yield return null;
        }
        ;
        CurState = ELightState.ON;
        gameObject.SetActive(false);
    }
    private IEnumerator LightShrink()                           // ũ�� ���� �ڷ�ƾ
    {
        while (CurSize > MinSize)
        {
            TrackPos();
            SetSize(CurSize - ChangeSpeed * Time.deltaTime);
            yield return null;
        }
        CurState = ELightState.OFF;
        Destroy(gameObject);
    }


    private void Awake()
    {
        if(Inst != null) { Destroy(Inst.gameObject); }
        Inst = this;
    }

    private void Start()
    {
        StartLight();
    }
}
