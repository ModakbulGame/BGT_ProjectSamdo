using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectScript : MonoBehaviour, IPoolable
{
    public ObjectPool<GameObject> m_originPool { get; set; }

    public void SetDestroyTime(float _time)
    {
        StartCoroutine(ReturnEffect(_time));
    }
    private IEnumerator ReturnEffect(float _time)
    {
        yield return new WaitForSeconds(_time);
        ReleaseEffect();
    }

    public void ReleaseEffect() { m_originPool.Release(gameObject); }
}
