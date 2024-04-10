using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleLauncher : MonoBehaviour
{
    [SerializeField]
    private GameObject m_firePrefab;

    [SerializeField]
    private float m_launchGap = 3;


    private IEnumerator LaunchFire()
    {
        yield return new WaitForSeconds(m_launchGap);
        CreateFire();
        StartCoroutine(LaunchFire());
    }

    private void CreateFire()
    {
        Vector3 pos = transform.position + new Vector3(0, 0.25f, -0.3f);
        Instantiate(m_firePrefab, pos, Quaternion.Euler(90, 0, 0));
    }


    private void Start()
    {
        StartCoroutine(LaunchFire());    
    }
}
