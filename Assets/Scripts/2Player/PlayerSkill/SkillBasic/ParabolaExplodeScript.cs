using UnityEngine;
using UnityEngine.UIElements;

public class ParabolaExplodeScript : ParabolaSkillScript
{
    [SerializeField]
    private GameObject[] m_skillExplosion;
    [SerializeField]
    private float m_explodeRange = 5f;

    private void ExplodeSkill()
    {
        for (int i = 0; i < m_skillExplosion.Length; i++)
        {
            m_skillExplosion[i].SetActive(true);
            m_skillExplosion[i].transform.SetParent(null);
            PlayerSkillScript explode = m_skillExplosion[i].GetComponent<PlayerSkillScript>();
            explode.SetDamage(5);
        }
    }
    public override void CheckSkillTrigger(Collider _other)
    {
        IHittable hittable = _other.GetComponentInParent<IHittable>();
        hittable ??= _other.GetComponentInChildren<IHittable>();
        if (hittable == null) { return; }
        if (hittable.IsPlayer) { return; }
        Vector3 point = _other.ClosestPoint(transform.position);
        GiveDamage(hittable, point);
        ExplodeSkill();
        CollideTarget();
    }

    private void OnTriggerEnter(Collider _other)
    {
        GameObject gameObject = _other.gameObject;
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (gameObject.layer == groundLayer) { ExplodeSkill(); }
    }
}
