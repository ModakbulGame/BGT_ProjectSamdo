using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveData
{
    public void LoadData();
    public void SetData();
}

public interface IHittable          // 데미지를 입을 수 있는 오브젝트들이 공통적으로 가져야 하는 인터페이스
{
    public bool IsPlayer { get; }
    public bool IsMonster { get; }
    public void GetHit(HitData _hit);   // 데미지 받기 => 타격 지점이나 방향은 추후 필요 시 추가
}

public interface IItem              // 갯수가 존재하는 아이템들에 사용할 인터페이스
{
    public void Use(GameObject target); // 입력으로 받는 target은 아이템 효과가 적용될 대상
}

public interface IHidable               // 빛 비춰서 변화가 생길 수 있는 오브젝트에 필수 부착
{
    public void GetLight();                 // 빛을 받았을 때
    public void LooseLight();               // 빛을 그만 받을 때
}

public interface IInteractable          // 상호작용이 가능한 오브젝트에 필수 부착
{
    public EInteractType InteractType { get; }
    public float UIOffset { get; }          // 상호작용 토글 UI 높이
    public string InfoTxt { get; }          // 상호작용 정보 텍스트
    public void StartInteract();            // 상호작용 시작
    public void StopInteract();             // 상호작용 중단
}
