using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WaterGuardianSkill3Script : ObjectAttackScript
{
    private Rigidbody m_rigid;
    [SerializeField]
    private VisualEffect m_hitEffect;

    public override void AttackOn()
    {
        gameObject.SetActive(true);
        m_rigid.constraints = RigidbodyConstraints.FreezeRotation;
        base.AttackOn();
    }
    private void SkillCollided()
    {
        m_rigid.velocity = Vector3.zero;
        m_rigid.constraints = RigidbodyConstraints.FreezeAll;
        m_hitEffect.Play();
        StartCoroutine(DestroySkill());
    }

    private IEnumerator DestroySkill()
    {
        yield return new WaitForSeconds(0.5f);
        AttackOff();
        yield return new WaitForSeconds(1);
        transform.SetParent(Attacker.transform);
        gameObject.SetActive(false);
    }



    private void OnTriggerEnter(Collider _other)
    {
        if (!_other.CompareTag(ValueDefine.TERRAIN_TAG)) { return; }
        SkillCollided();
    }

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }
    public override void Start() { }

    private void FixedUpdate()
    {
        m_rigid.AddForce(Vector3.down * 20);
    }
}
