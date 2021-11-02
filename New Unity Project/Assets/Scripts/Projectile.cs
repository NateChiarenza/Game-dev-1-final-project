using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_Speed = 100f;   // this is the projectile's speed
    public float m_Lifespan = 3f; // this is the projectile's lifespan (in seconds)

    /// <summary>
    /// Private fields
    /// </summary>
    private Rigidbody m_Rigidbody;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
        m_Rigidbody.AddForce(m_Rigidbody.transform.forward * m_Speed);
        Destroy(gameObject, m_Lifespan);
    }
    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Floor")
        {
            this.GetComponent<SphereCollider>().enabled = true;
        }
    }
}
