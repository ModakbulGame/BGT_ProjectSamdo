using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELightState
{
    OFF,
    CHANGE,
    ON
}

public class TempLightScript : MonoBehaviour            // 임시 불빛 오브젝트
{
    private static TempLightScript Inst;

    private const float MinSize = 0;                            // 최소 크기
    private const float MaxSize = 100;                          // 최대 크기

    private Transform PlayerTransform { get; set; }             // 플레이어 위치
    public static ELightState CurState { get; private set; }    // 불빛의 현재 상태 (static)
    public static float CurSize { get; private set; }           // 현재 크기 (static)
    private float ChangeSpeed { get; set; }                     // 크기 변화 속도
    private const float ChangeTime = 1;                         // 크기 변화 시간

    private void SetSize(float _size)                           // 크기 설정
    {
        CurSize = _size;
        transform.localScale = new(_size, _size, _size);
    }
    public void SetPlayerTransform(Transform _transform)        // 플레이어 쫓아가기 설정
    {
        PlayerTransform = _transform;
        TrackPos();
    }

    private void TrackPos()                                     // 위치 쫓아가기
    {
        transform.position = PlayerTransform.position;
    }

    private void StartLight()                                   // 빛 밝히기 시작
    {
        CurState = ELightState.CHANGE;
        SetSize(MinSize);
        ChangeSpeed = (MaxSize - MinSize) / ChangeTime;
        StartCoroutine(LightExpand());
    }
    public void EndLight()                                      // 빛 밝히기 종료
    {
        gameObject.SetActive(true);
        CurState = ELightState.CHANGE;
        StartCoroutine(LightShrink());
    }
    private IEnumerator LightExpand()                           // 크기 증가 코루틴
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
    private IEnumerator LightShrink()                           // 크기 감소 코루틴
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
