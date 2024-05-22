using UnityEngine;
using UnityEngine.UIElements;

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
        }
    }
    public override void CollideTarget()
    {
        ExplodeSkill();
    }

    private void OnTriggerEnter(Collider _other)
    {
        int groundLayer = LayerMask.NameToLayer("Ground");
        if(_other is MeshCollider &&_other.gameObject.layer == groundLayer)
        {
            ExplodeSkill();
        }
    }
}
