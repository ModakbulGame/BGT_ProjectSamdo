using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSphere : MonoBehaviour
{
    private Rigidbody m_rigid;



    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        m_rigid.velocity = Vector3.up * 10;
    }

    void FixedUpdate()
    {
        
    }
}
