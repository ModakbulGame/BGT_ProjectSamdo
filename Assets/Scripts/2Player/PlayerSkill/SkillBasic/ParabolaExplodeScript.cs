using UnityEngine;
using UnityEngine.VFX;

public class ParabolaExplodeScript : ParabolaSkillScript
{
    [SerializeField]
    private GameObject[] m_skillExplosion;
    Vector3[] SkillOffset { get; set; }


    public override void CollideGround()
    {
        CollideTarget();
    }

    private void ExplodeSkill()
    {
        for (int i = 0; i < m_skillExplosion.Length; i++)   
        {
            m_skillExplosion[i].SetActive(true);
            m_skillExplosion[i].transform.localPosition = SkillOffset[i];
            m_skillExplosion[i].transform.SetParent(null);
            ExplodeScript explode = m_skillExplosion[i].GetComponent<ExplodeScript>();
            explode.SetDamage(m_attacker, 5, 1); //
            explode.SetReturnTransform(transform);
        }
    }

    private void DisableExplosion()
    {
        if(m_skillExplosion!= null)
        {
            for (int i = 0; i < m_skillExplosion.Length; i++)
            {
                m_skillExplosion[i].SetActive(false);
                m_skillExplosion[i].transform.SetParent(gameObject.transform);
            }
        }
    }

    public override void CollideTarget()
    {
        ExplodeSkill();
        base.CollideTarget();
    }

    public override void ReleaseToPool()
    {
        base.ReleaseToPool();
        //DisableExplosion();
    }


    public override void Start()
    {
        base.Start();
        SkillOffset = new Vector3[m_skillExplosion.Length];
        for(int i = 0; i<m_skillExplosion.Length; i++) { SkillOffset[i] = m_skillExplosion[i].transform.localPosition; }
    }
}
