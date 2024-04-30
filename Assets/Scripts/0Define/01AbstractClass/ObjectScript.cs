using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract partial class ObjectScript : MonoBehaviour, IHittable
{
    // 기본 컴포넌트
    protected Rigidbody m_rigid;            // Rigidbody
    protected Animator m_anim;              // Animator

    public virtual ObjectAttackScript AttackObject { get; set; }    // 부속 공격 판정


    // 위치, 회전
    public Vector2 Velocity2 { get { return new(m_rigid.velocity.x, m_rigid.velocity.z); } }
    public Vector3 Position { get { return transform.position; } }
    public Vector2 Position2 { get { return new(transform.position.x, transform.position.z); } }    // 평면 좌표
    public float Rotation                               // y축 각도
    {
        get { return transform.eulerAngles.y; } 
        protected set { Vector3 rot = transform.eulerAngles; rot.y = value; transform.eulerAngles = rot; }
    }
    private const float ROTATE_SPEED = 0.05f;           // 회전 속도
    private const float SLOW_ROTATE_SPEED = 0.3f;       // 회전 속도


    // 현재 상태
    public float CurHP { get; protected set; }                          // 현재 HP
    public virtual float CurSpeed { get; protected set; }               // 현재 속도
    public bool IsDead { get; protected set; }                          // 죽음 상태
    public virtual bool IsUnstoppable { get; } = true;                  // 히트 상태 가능 여부


    // 물리 관련
    protected virtual float DampSpeedUp { get { return 0.2f; } }        // 가속도
    protected virtual float DampSpeedDown { get { return 0.1f; } }      // 감속도
    protected Vector3 velocityRef = Vector3.zero;                       // 속도 참조
    private float RotationRef;                                          // 각도 참조


    // 애니메이션
    public virtual void IdleAnimation() { m_anim.SetTrigger("IDLE"); }
    public virtual void MoveAnimation() { m_anim.SetTrigger("MOVE"); }
    public virtual void AttackAnimation() { m_anim.SetTrigger("ATTACK"); }
    public virtual void HitAnimation() { m_anim.SetTrigger("HIT"); }
    public virtual void DieAnimation() { m_anim.SetTrigger("DIE"); }


    // UI
    [Space(10f)]
    [SerializeField]
    protected float m_uiOffset = 2;


    // 기본 메소드
    public virtual void OnInitiated() { }                               // 생성 시
    public virtual void CreateAttack() { }                              // 공격 생성 타이밍
    public virtual void AttackTriggerOn() { if (!AttackObject) return; AttackObject.AttackOn(); }       // 공격 트리거 on
    public virtual void AttackTriggerOff() { if (!AttackObject) return; AttackObject.AttackOff(); }     // 공격 트리거 off
    public virtual void AttackDone() { }                                // 공격 모션 끝
    public virtual void GetHit(HitData _hit)                            // 공격 맞음
    {
        if (IsDead) { return; }
        GetDamage(_hit.Damage);
        if (!IsUnstoppable) { PlayHitAnim(); }
        if (!IsDead) { GetCC(_hit); }
    }
    public virtual void GetDamage(float _damage)
    {
        float hp = CurHP;
        hp -= _damage;
        if (hp <= 0) { hp = 0; SetDead(); }
        SetHP(hp);
    }
    public virtual void PlayHitAnim()                             // 데미지 받기
    {
        HitAnimation();
    }
    public virtual void SetHP(float _hp) { CurHP = _hp; }       // HP 설정
    public virtual void SetDead() { IsDead = true; }            // 죽음 설정
    public virtual void MoveTo(Vector3 _dir)                    // 방향으로 이동
    {
        float damp = _dir == Vector3.zero ? DampSpeedDown : DampSpeedUp;
        m_rigid.velocity = Vector3.SmoothDamp(m_rigid.velocity, _dir * CurSpeed, ref velocityRef, damp);
    }
    public void RotateTo(float _deg)
    {
        if (Rotation == _deg) { return; }
        float angle = Mathf.SmoothDampAngle(Rotation, _deg, ref RotationRef, ROTATE_SPEED);
        Rotation = angle;
    }
    public void RotateTo(Vector2 _dir)                          // 방향으로 회전
    {
        float deg = FunctionDefine.VecToDeg(_dir);
        RotateTo(deg);
    }
    public void SlowRotate(float _deg)
    {
        if (Rotation == _deg) { return; }
        float angle = Mathf.SmoothDampAngle(Rotation, _deg, ref RotationRef, SLOW_ROTATE_SPEED);
        Rotation = angle;
    }
    public virtual void StopMove()
    {
        if (m_rigid.velocity.magnitude > 0)
            m_rigid.velocity = Vector3.zero;
    }
    public virtual void GetCC(HitData _hit)
    {
        switch (_hit.CCType)
        {
            case ECCType.AIRBORNE:
                GetAirborne(_hit);
                break;
            case ECCType.STUN:
                GetStun(_hit);
                break;
            case ECCType.BLEED:
                GetBleed(_hit);
                break;
            case ECCType.STAGGER:
                GetStagger(_hit);
                break;
            case ECCType.POISON:
                GetPoison(_hit);
                break;
            case ECCType.SLOW:
                GetSlow(_hit);
                break;
            case ECCType.KNOCKBACK:
                GetKnockBack(_hit);
                break;
        }
    }


    // CC기
    // HitData들은 나중에 적용.
    private readonly float[] m_ccCount = new float[(int)ECCType.LAST];
    private IEnumerator DebuffCotouine(ECCType _cc)
    {
        while (!IsDead && m_ccCount[(int)_cc] > 0)
        {
            m_ccCount[(int)_cc] -= Time.deltaTime;
            yield return null;
        }
        m_ccCount[(int)_cc] = 0;
    }
    private IEnumerator DamageCoroutine(ECCType _cc, float _damage)
    {
        while (!IsDead && m_ccCount[(int)_cc] > 0)
        {
            yield return new WaitForSeconds(1);
            GetDamage(_damage);
            m_ccCount[(int)_cc]--;
        }
        m_ccCount[(int)_cc] = 0;
    }
    private void GetAirborne(HitData _hit)
    {
        Vector3 force = 8 * Vector3.up;
        m_rigid.AddForce(force);
    }
    public bool IsStunned { get { return m_ccCount[(int)ECCType.STUN] > 0; } }
    private void GetStun(HitData _hit)
    {
        // 기절.
        StartCoroutine(DebuffCotouine(ECCType.STUN));
    }
    public bool IsBleeding { get { return m_ccCount[(int)ECCType.BLEED] > 0; } }
    private void GetBleed(HitData _hit)
    {
        bool startCor = !IsBleeding;
        m_ccCount[(int)ECCType.BLEED] = 5;
        if (startCor) { StartCoroutine(DamageCoroutine(ECCType.BLEED, 5)); }
    }
    private void GetStagger(HitData _hit)
    {
        // 경직.
        StartCoroutine(DebuffCotouine(ECCType.STUN));   // => 필요한가?
    }
    public bool IsPoisoned { get { return m_ccCount[(int)ECCType.POISON] > 0; } }
    private void GetPoison(HitData _hit)
    {
        bool startCor = !IsPoisoned;
        m_ccCount[(int)ECCType.POISON] = 10;
        if (startCor) { StartCoroutine(DamageCoroutine(ECCType.POISON, 2)); }
    }
    public bool IsSlowed { get { return m_ccCount[(int)ECCType.SLOW] > 0; } }
    private void GetSlow(HitData _hit)
    {
        if(IsSlowed) { m_ccCount[(int)ECCType.SLOW] = 10; return; }
        StartCoroutine(DebuffCotouine(ECCType.SLOW));
    }
    private void GetKnockBack(HitData _hit)
    {
        Vector2 flatDir = (Position2 -_hit.Attacker.Position2).normalized;
        Vector3 dir = new(flatDir.x, 0, flatDir.y);
        m_rigid.velocity = dir * 8;
    }



    // 기본 설정
    public virtual void SetComps()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_anim = GetComponentInChildren<Animator>();
        SetAttackObject();
    }
    public virtual void SetInfo() { }
    public virtual void SetAttackObject()
    {
        AttackObject = GetComponentInChildren<ObjectAttackScript>();
    }

    public virtual void Awake() { SetComps(); }
    public virtual void Start() { SetHP(MaxHP); CurSpeed = MoveSpeed; }
}
