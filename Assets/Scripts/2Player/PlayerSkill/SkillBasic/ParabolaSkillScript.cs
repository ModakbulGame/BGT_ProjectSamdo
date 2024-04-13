using System.Collections;
using UnityEngine;

public class ParabolaSkillScript : ProjectileSkillScript
{
    protected Rigidbody m_rigid;

    [SerializeField]
    private float m_moveSpeed = 6; // MoveSpeed SO 데려오기
    [SerializeField]
    private float m_upperForce = 5;

    private Vector3 MoveDir { get; set; }
    public override void ReleaseTopool()
    {
        m_rigid.velocity = Vector3.zero;
        base.ReleaseTopool();
    }


    public void SetSkill(PlayerController _player, float _attack, float _magic, Vector3 _dir)
    {
        base.SetSkill(_player, _attack, _magic);
        MoveDir = new(_dir.x, 0, _dir.z);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        m_rigid.velocity = Vector3.up * m_upperForce;
    }

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    private override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector3 vel = m_rigid.velocity;
        Vector3 dir = m_moveSpeed * MoveDir;
        vel.x = dir.x; vel.z = dir.z;
        m_rigid.velocity = vel;
        m_rigid.AddForce(Vector3.down * ValueDefine.PARABOLA_GRAVITY);
    }
}
