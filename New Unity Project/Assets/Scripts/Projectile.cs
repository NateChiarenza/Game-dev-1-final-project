using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_Speed = 100f;   // this is the projectile's speed
    public float m_Lifespan = 3f; // this is the projectile's lifespan (in seconds)
    public bool isRock = false;
    private Rigidbody m_Rigidbody;
    GameObject player;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        m_Rigidbody.AddForce(-m_Rigidbody.transform.forward * m_Speed);

        Destroy(gameObject, m_Lifespan);
    }
    private void OnCollisionEnter(Collision c)
    {
        if (isRock)
        {
            if (c.gameObject.tag == "Floor")
            {
                this.GetComponent<SphereCollider>().enabled = true;
                isRock = false;
            }
            if (c.gameObject.tag == "Enemy")
            {
                c.gameObject.GetComponent<Enemy>().TakeDamage(2);
                Destroy(gameObject);
            }
        }
        if(this.gameObject.tag == "bad arrow")
        {
            if(c.gameObject.tag == "Player")
            {
                player.GetComponent<Player>().TakeDamage(1);
            }
        }
        else if(this.gameObject.tag == "Arrow")
        {
            if (c.gameObject.tag == "Enemy")
            {
                c.gameObject.GetComponent<Enemy>().TakeDamage(50);
                Destroy(gameObject);
            }
        }
        
    }
}
