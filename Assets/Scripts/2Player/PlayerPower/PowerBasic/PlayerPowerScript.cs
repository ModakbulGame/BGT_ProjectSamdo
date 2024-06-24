using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerPowerScript : ObjectAttackScript, IPoolable
{
    [SerializeField]
    protected PowerScriptable m_scriptable;

    [SerializeField]
    private AttackEffect m_effect;
    public bool IsScriptableSet { get { return m_scriptable != null; } }
    public void SetScriptable(PowerScriptable _scriptable) { m_scriptable = _scriptable; SetInfo(); }

    [SerializeField]
    private float m_lastTime = 10;

    protected readonly float[] Damages = new float[2];

    private PlayerController Player { get { return (PlayerController)m_attacker; } }

    public override ECCType[] CCList { get { return new ECCType[1] { PowerManager.IDToCC(m_scriptable.ID) }; } }
    public EPowerProperty[] PowerProps { get { return m_scriptable.PowerProps; } }

    public virtual void SetPower(PlayerController _player, float _attack, float _magic)
    {
        m_attacker = _player;
        Damages[0] = _attack;
        Damages[1] = _magic;
    }

    public float ResultDamage { get { 
            return (m_scriptable.Attack * Damages[0] * Attacker.AttackMultiplier +
                m_scriptable.Magic * Damages[1] * Attacker.MagicMultiplier) * Attacker.DamageMultiplier; } }

    public ObjectPool<GameObject> OriginalPool { get; set; }
    public void SetPool(ObjectPool<GameObject> _pool) { OriginalPool = _pool; }
    public void OnPoolGet() { }
    public virtual void ReleaseToPool()
    {
        AttackOff();
        OriginalPool.Release(gameObject);
    }

    public virtual void OnTriggerEnter(Collider _other)
    {
        CheckPowerTrigger(_other);
    }
    public virtual void CheckPowerTrigger(Collider _other)
    {
        IHittable hittable = _other.GetComponentInParent<IHittable>();
        hittable ??= _other.GetComponentInChildren<IHittable>();
        if (hittable == null) { return; }
        if (hittable.IsPlayer) { return; }
        Vector3 point = _other.ClosestPoint(transform.position);
        GiveDamage(hittable, point);
        CollideTarget();
    }
    public override void GiveDamage(IHittable _hittable, Vector3 _point)
    {
        if (CheckHit(_hittable)) { return; }
        HitData hit = new(Player, ResultDamage, _point, CCList);
        if (_hittable.GetHit(hit))
        {
            AddHitObject(_hittable);
        }
    }

    public virtual void CollideTarget() { }

    public virtual void CreateEffect()
    {
        if (m_effect == null) { return; }
        m_effect.EffectOn(transform);
    }


    private IEnumerator ReleaseDelay()
    {
        yield return new WaitForSeconds(m_lastTime);
        if (gameObject.activeSelf) { ReleaseToPool(); }
    }

    public virtual void OnEnable()
    {
        AttackOn();
        StartCoroutine(ReleaseDelay());
    }
    public override void AttackOn()
    {
        base.AttackOn();
    }

    private void SetInfo()
    {

    }

    public override void Start() { }
}
