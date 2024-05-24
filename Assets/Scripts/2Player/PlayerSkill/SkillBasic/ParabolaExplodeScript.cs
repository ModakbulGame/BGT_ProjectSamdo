using UnityEngine;
using UnityEngine.VFX;

public class ParabolaExplodeScript : ParabolaSkillScript
{
    [SerializeField]
    private GameObject[] m_skillExplosion;

    private void ExplodeSkill()
    {
        for (int i = 0; i < m_skillExplosion.Length; i++)   
        {
            m_skillExplosion[i].SetActive(true);
            m_skillExplosion[i].transform.SetParent(null);
            ExplodeScript explode = m_skillExplosion[i].GetComponent<ExplodeScript>();
            explode.SetDamage(m_attacker, 5, 1);
            explode.SetReturnTransform(transform);
        }
    }

    public override void CollideTarget()
    {
        ExplodeSkill();
        base.CollideTarget();
    }

    private void OnTriggerEnter(Collider _other)
    {
        GameObject gameObject = _other.gameObject;
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (gameObject.layer == groundLayer) { ExplodeSkill(); }
        CheckSkillTrigger(_other);
    }
}
