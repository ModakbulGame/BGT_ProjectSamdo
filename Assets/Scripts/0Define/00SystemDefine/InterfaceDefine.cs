using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IHaveData
{
    public void LoadData();
    public void SetData();
}

public interface IHittable          // �������� ���� �� �ִ� ������Ʈ���� ���������� ������ �ϴ� �������̽�
{
    public bool IsPlayer { get; }
    public bool IsMonster { get; }
    public void GetHit(HitData _hit);   // ������ �ޱ� => Ÿ�� �����̳� ������ ���� �ʿ� �� �߰�
}

public interface IHidable               // �� ���缭 ��ȭ�� ���� �� �ִ� ������Ʈ�� �ʼ� ����
{
    public void GetLight();                 // ���� �޾��� ��
    public void LooseLight();               // ���� �׸� ���� ��
}

public interface IInteractable          // ��ȣ�ۿ��� ������ ������Ʈ�� �ʼ� ����
{
    public EInteractType InteractType { get; }
    public float UIOffset { get; }          // ��ȣ�ۿ� ��� UI ����
    public string InfoTxt { get; }          // ��ȣ�ۿ� ���� �ؽ�Ʈ
    public void StartInteract();            // ��ȣ�ۿ� ����
    public void StopInteract();             // ��ȣ�ۿ� �ߴ�
}

public interface IPoolable
{
    public ObjectPool<GameObject> OriginalPool { get; }
    public void SetPool(ObjectPool<GameObject> _pool);
    public void OnPoolGet();
    public void ReleaseTopool();
}