using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponInfo
{
    public EWeaponType WeaponType;
    public EHitType HitType;
    public string WeaponName;
    public FRange Attack;
    public FRange Magic;
    public float AttackSpeed;
    public void SetInfo(WeaponScriptable _scriptable)
    {
        WeaponType = DataManager.IDToWeaponType(_scriptable.ID);
        if(WeaponType == EWeaponType.SCEPTER) { HitType = EHitType.BLOW; }
        else { HitType = EHitType.SLASH; }
        WeaponName = _scriptable.ItemName;
        Attack = _scriptable.Attack;
        Magic = _scriptable.Magic;
        AttackSpeed = _scriptable.AttackSpeed;
    }
}

[RequireComponent(typeof(BoxCollider))]
public class WeaponScript : ObjectAttackScript
{
    [SerializeField]
    private WeaponInfo m_weaponInfo = new();
    public EWeaponType WeaponType { get { return m_weaponInfo.WeaponType; } }
    public EHitType HitType { get { return m_weaponInfo.HitType; } }
    public string WeaponName { get { return m_weaponInfo.WeaponName; } }
    public FRange WeaponAttack { get { return m_weaponInfo.Attack; } }
    public FRange WeaponMagic { get { return m_weaponInfo.Magic; } }
    public float WeaponAttackSpeed { get { return m_weaponInfo.AttackSpeed; } }
    public void SetWeaponInfo(WeaponScriptable _scriptable) { m_weaponInfo.SetInfo(_scriptable); }

    [SerializeField]
    private ECCType m_curCC = ECCType.KNOCKBACK;
    public ECCType CurCC { get { return m_curCC; } set { m_curCC = value; } }
    public void SetCC(ECCType _cc) { CurCC = _cc; }


    [SerializeField]
    private WeaponScriptable m_scriptable;      // ��ũ���ͺ�
    public bool IsScriptableSet { get { return m_scriptable != null; } }
    public EWeaponName WeaponEnum { get { return (EWeaponName)m_scriptable.Idx; } }

    private PlayerController Player { get { return (PlayerController)m_attacker; } }

    private float ResultDamage { get { return Player.Attack; } }


    // ���� ������ Ʈ����
    private BoxCollider m_collider;

    private const int MAX_TRAIL_FRAME = 2;
    private const float MODEL_MULTIPLIER = 30.90435f;


    private readonly LinkedList<BufferObject> m_trailList = new();
    private LinkedList<BufferObject> m_trailFillerList = new();

    private void CheckTrail()
    {
        Vector3 localScale = transform.localScale;
        Vector3 size = m_collider.size * MODEL_MULTIPLIER;
        size.x *= localScale.x; size.y *= localScale.y; size.z *= localScale.z;


        Vector3 center = MODEL_MULTIPLIER*m_collider.center;
        center.x *= localScale.x; center.y *= localScale.y; center.z *= localScale.z;
        Vector3 offset = m_collider.transform.TransformDirection(center);
        BufferObject col = new()
        {
            Size = size,
            Rotation = m_collider.transform.rotation,
            Position = m_collider.transform.position + offset
        };
        m_trailList.AddFirst(col);
        if (m_trailList.Count > MAX_TRAIL_FRAME) { m_trailList.RemoveLast(); }
        if (m_trailList.Count > 1) { m_trailFillerList = FillTrail(m_trailList.First.Value, m_trailList.Last.Value); }

        Collider[] hits = Physics.OverlapBox(col.Position, col.Size / 2, col.Rotation, ValueDefine.HITTABLE_LAYER, QueryTriggerInteraction.UseGlobal);
        HitObject(hits);
        foreach (BufferObject co in m_trailFillerList)
        {
            hits = Physics.OverlapBox(co.Position, co.Size / 2, co.Rotation, ValueDefine.HITTABLE_LAYER, QueryTriggerInteraction.UseGlobal);
            HitObject(hits);
        }
    }

    private void HitObject(Collider[] _hits)
    {
        if(!IsAttacking) { return; }

        foreach (Collider collider in _hits)
        {
            IHittable hittable = collider.GetComponentInParent<IHittable>();
            if (hittable == null) { Debug.LogError("히터블 스크립트 없음"); continue; }
            if (collider.CompareTag(ValueDefine.PLAYER_HIT_TAG)) { continue; }
            Vector3 pos = CheckNHit(hittable);
            AddHitObject(hittable);
            if (hittable.IsMonster && pos != Vector3.zero)
            {
                EEffectName effectName = HitType == EHitType.SLASH ? EEffectName.HIT_SLASH : EEffectName.HIT_BLOW;
                GameObject effect = GameManager.GetEffect(effectName);
                effect.transform.position = pos;
            }
        }
    }
    protected Vector3 CheckNHit(IHittable _hittable)
    {
        if (CheckHit(_hittable)) { return Vector3.zero; }
        Vector3 pos = GetComponent<Collider>().ClosestPoint(transform.position);
        HitData hit = new(Player, ResultDamage, pos, CurCC);
        _hittable.GetHit(hit);
        AddHitObject(_hittable);
        return pos;
    }

    private LinkedList<BufferObject> FillTrail(BufferObject _from, BufferObject _to)
    {
        LinkedList<BufferObject> fillerList = new();
        float distance = Mathf.Abs((_from.Position - _to.Position).magnitude);
        Vector3 localScale = transform.localScale;
        Vector3 size = m_collider.size * MODEL_MULTIPLIER;
        size.x *= localScale.x; size.y *= localScale.y; size.z *= localScale.z;
        float gap = size.z;
        if (distance > gap)
        {
            float steps = Mathf.Ceil(distance / gap);
            float stepAmount = 1 / (steps + 1);
            float stepValue = 0;
            for (int i = 0; i<(int)steps; i++)
            {
                stepValue += stepAmount;
                BufferObject obj = new()
                {
                    Size = size,
                    Position = Vector3.Lerp(_from.Position, _to.Position, stepValue),
                    Rotation = Quaternion.Lerp(_from.Rotation, _to.Rotation, stepValue)
                };
                fillerList.AddFirst(obj);
            }
        }
        return fillerList;
    }
    private void OnDrawGizmos()
    {
        if(!IsAttacking) { return; }
        foreach (BufferObject col in m_trailList)
        {
            Gizmos.color = Color.blue;
            Gizmos.matrix = Matrix4x4.TRS(col.Position, col.Rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, col.Size);
        }
        foreach (BufferObject bet in m_trailFillerList)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(bet.Position, bet.Rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, bet.Size);
        }
    }


    public void SetScriptable(WeaponScriptable _scriptable)
    { 
        m_scriptable = _scriptable; 
        SetInfo();
    }
    private void SetInfo()
    {
        m_weaponInfo.SetInfo(m_scriptable);
    }
    private void SetComps()
    {
        m_collider = GetComponent<BoxCollider>();
    }

    private void Awake()
    {
        SetComps();
    }

    private void FixedUpdate()
    {
        if (IsAttacking)
        {
          CheckTrail();
        }
    }
}
