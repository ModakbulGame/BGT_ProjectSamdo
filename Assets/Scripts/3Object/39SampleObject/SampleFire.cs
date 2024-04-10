using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleFire : MonoBehaviour
{
    [SerializeField]
    private GameObject m_parryingSound;

    private void OnTriggerEnter(Collider _other)
    {
        switch (_other.tag)
        {
            case ValueDefine.PLAYER_WEAPON_TAG:
                if(!_other.GetComponent<WeaponScript>().IsAttacking) { return; }
                GameObject sound = Instantiate(m_parryingSound, transform.position, Quaternion.identity);
                Destroy(sound, 1);
                Destroy(gameObject);
                break;
            case ValueDefine.PLAYER_TAG:
                if(_other.GetComponent<PlayerController>().IsDead) { return; }
                Destroy(gameObject);
                break;
        }
    }

    public void HitPlayer()
    {

    }

    private void Start()
    {
        Destroy(gameObject, 10);
    }
    private void Update()
    {
        transform.position += 5 *Time.deltaTime*Vector3.back;
    }
}
