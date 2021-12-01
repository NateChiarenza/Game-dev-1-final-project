using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Projectile : MonoBehaviour
{
    public float m_Speed = 100f;   // this is the projectile's speed
    public float m_Lifespan = 3f; // this is the projectile's lifespan (in seconds)
    public bool isRock = false;
    private Rigidbody m_Rigidbody;
    public GameObject player;
    public AudioSource arrow;
    public AudioClip clip;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }
    void Start()
    {
        if (!isRock &&  this.tag !="bad arrow")
        {
            arrow = GameObject.Find("HM_crossbow_finished").GetComponent<AudioSource>();
        }
        m_Rigidbody.AddForce(-m_Rigidbody.transform.forward * m_Speed);
        if (!isRock)
        {
            arrow.PlayOneShot(clip);
        }
        Destroy(gameObject, m_Lifespan);
    }
    private void OnCollisionEnter(Collision c)
    {
        if (isRock)
        {
            if (c.gameObject.tag == "Floor")
            {
                this.GetComponent<SphereCollider>().enabled = true;
                arrow.PlayOneShot(clip);
                isRock = false;
            }
            if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "Respawn")
            {
                c.gameObject.GetComponent<Enemy>().TakeDamage(2);
                Destroy(gameObject);
            }
        }
        if(this.gameObject.tag == "bad arrow")
        {
            Debug.Log("hello");
            if(c.gameObject.tag == "Player")
            {
                Debug.Log("hit");
                player.GetComponent<Player>().TakeDamage(2);
            }
        }
        else if(this.gameObject.tag == "Arrow")
        {
            if (c.gameObject.tag == "Enemy"|| c.gameObject.tag == "Respawn")
            {
                c.gameObject.GetComponent<Enemy>().TakeDamage(50);
                Destroy(gameObject);
            }
        }
        
    }
}
