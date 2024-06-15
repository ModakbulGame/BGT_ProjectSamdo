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
            explode.SetDamage(m_attacker, 10, 1);
            explode.SetReturnTransform(transform);
        }
    }

    public override void SetSkill(PlayerController _player, float _attack, float _magic, Vector2 _dir)
    {
        _magic = 0;
        _attack = 0;
        base.SetSkill(_player, _attack, _magic, _dir);
    }

    public override void CollideTarget()
    {
        ExplodeSkill();
        base.CollideTarget();
    }

    public override void ReleaseToPool()
    {
        base.ReleaseToPool();
    }


    public override void Start()
    {
        base.Start();
        SkillOffset = new Vector3[m_skillExplosion.Length];
        for(int i = 0; i<m_skillExplosion.Length; i++) { SkillOffset[i] = m_skillExplosion[i].transform.localPosition; }
    }
}
