using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Entity의 control 주체 표현을 위한 enum, 중립,우호를 넣는 것도 가능하다
public enum EntityControlType
{
    Player,
    AI
}

public class EntityManager : MonoBehaviour
{

    public delegate void TakeDamageHandler(EntityManager entity, EntityManager instigator, object causer, float damage);
    public delegate void DeadHandler(EntityManager entity);
    // Category가 여기서는 적과 아군의 구분의 용도
    [SerializeField]
    private Category[] categories;
    [SerializeField]
    private EntityControlType controltype;

    // socket은 Entity Script를 가진 GameObject의 자식 Gameobject를 의미한다
    // 스킬의 발사 위치, 특정 위치 저장하고 외부에서 찾아오기 위해 존재
    // ex) 플레이어의 손바닥, 발바닥
    private Dictionary<string, Transform> socketsByName = new();

    public EntityControlType ControlType => controltype;
    public IReadOnlyList<Category> Categories => categories;
    public bool IsPlayer => controltype == EntityControlType.Player;
    public Animator Animator { get; private set; }
    public Stats Stats { get; private set; }
    public bool IsDead => Stats.HPStat != null && Mathf.Approximately(Stats.HPStat.DefaultValue, 0f);
    public EntityManager Target { get; set; }
    // Target은 목표 대상, Entity의 공격 Target or 치유 Target

    public event TakeDamageHandler OnTakeDamage;
    public event DeadHandler OnDead;

    private void Awake()
    {
        Animator = GetComponent<Animator>();

        Stats = GetComponent<Stats>();
        Stats.Setup(this);
    }

    public void TakeDamage(EntityManager instigator, object causer, float damage)
    {
        if (IsDead)
            return;

        float prevValue = Stats.HPStat.DefaultValue;
        Stats.HPStat.DefaultValue -= damage;

        OnTakeDamage?.Invoke(this, instigator, causer, damage);

        if (Mathf.Approximately(Stats.HPStat.DefaultValue, 0f))
            WhenDead();
    }

    private void WhenDead()
    {
        OnDead?.Invoke(this);   
    }

    // root transform의 자식 transform을 순회하며 이름이 socketName인 GameObject의 Transform 찾아온다
    private Transform GetTransformSocket(Transform root, string socketname)
    {
        if (root.name == socketname)
            return root;

        foreach (Transform child in root)
        {
            // 재귀함수로 자식 중에 socketName이 있는지 확인
            var socket = GetTransformSocket(child, socketname);
            if (socket)
                return socket;
        }
        return null;
    }

    // 저장되어 있는 Socket을 가져오거나 순회로 가져옴
    public Transform GetTransformSocket(string socketname)
    {
        // dictionary에 socketName을 검색하여 있다면 return
        if (socketsByName.TryGetValue(socketname, out var socket))
            return socket;
        // dictionary에 없으므로 순회 검색 시작
        socket = GetTransformSocket(transform, socketname);
        // socket을 찾으면 dictionary에 저장, 또 검색할 필요 없다
        if (socket)
            socketsByName[socketname] = socket;
        return socket;
    }

    public bool HasCategory(Category category)
        => categories.Any(x => x.ID == category.ID);
}