using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_Speed = 30f;   // this is the projectile's speed
    public float m_Lifespan = 2f; // this is the projectile's lifespan (in seconds)

    private Rigidbody m_Rigidbody;

    
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    
    void Start()
    {
        m_Rigidbody.velocity = Vector3.right*m_Speed;
        transform.TransformDirection(m_Rigidbody.velocity);
        Destroy(gameObject, m_Lifespan);
    }
}
