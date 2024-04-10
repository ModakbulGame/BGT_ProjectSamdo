using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Entity�� control ��ü ǥ���� ���� enum, �߸�,��ȣ�� �ִ� �͵� �����ϴ�
public enum EntityControlType
{
    Player,
    AI
}

public class EntityManager : MonoBehaviour
{

    public delegate void TakeDamageHandler(EntityManager entity, EntityManager instigator, object causer, float damage);
    public delegate void DeadHandler(EntityManager entity);
    // Category�� ���⼭�� ���� �Ʊ��� ������ �뵵
    [SerializeField]
    private Category[] categories;
    [SerializeField]
    private EntityControlType controltype;

    // socket�� Entity Script�� ���� GameObject�� �ڽ� Gameobject�� �ǹ��Ѵ�
    // ��ų�� �߻� ��ġ, Ư�� ��ġ �����ϰ� �ܺο��� ã�ƿ��� ���� ����
    // ex) �÷��̾��� �չٴ�, �߹ٴ�
    private Dictionary<string, Transform> socketsByName = new();

    public EntityControlType ControlType => controltype;
    public IReadOnlyList<Category> Categories => categories;
    public bool IsPlayer => controltype == EntityControlType.Player;
    public Animator Animator { get; private set; }
    public Stats Stats { get; private set; }
    public bool IsDead => Stats.HPStat != null && Mathf.Approximately(Stats.HPStat.DefaultValue, 0f);
    public EntityManager Target { get; set; }
    // Target�� ��ǥ ���, Entity�� ���� Target or ġ�� Target

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

    // root transform�� �ڽ� transform�� ��ȸ�ϸ� �̸��� socketName�� GameObject�� Transform ã�ƿ´�
    private Transform GetTransformSocket(Transform root, string socketname)
    {
        if (root.name == socketname)
            return root;

        foreach (Transform child in root)
        {
            // ����Լ��� �ڽ� �߿� socketName�� �ִ��� Ȯ��
            var socket = GetTransformSocket(child, socketname);
            if (socket)
                return socket;
        }
        return null;
    }

    // ����Ǿ� �ִ� Socket�� �������ų� ��ȸ�� ������
    public Transform GetTransformSocket(string socketname)
    {
        // dictionary�� socketName�� �˻��Ͽ� �ִٸ� return
        if (socketsByName.TryGetValue(socketname, out var socket))
            return socket;
        // dictionary�� �����Ƿ� ��ȸ �˻� ����
        socket = GetTransformSocket(transform, socketname);
        // socket�� ã���� dictionary�� ����, �� �˻��� �ʿ� ����
        if (socket)
            socketsByName[socketname] = socket;
        return socket;
    }

    public bool HasCategory(Category category)
        => categories.Any(x => x.ID == category.ID);
}