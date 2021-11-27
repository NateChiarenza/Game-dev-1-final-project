using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int damage;
    public GameObject target;
    public bool attack = true;
    public Animator anime;
    public bool walking = true;
    public bool shooting = false;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
    }
    void Update()
    {
        anime.SetBool("walking", walking);
        anime.SetBool("Shoot", shooting);
    }
    public void TakeDamage(int d)
    {
        hp = hp - d;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision c)
    {
        if(c.gameObject.tag == "Player" && attack)
        {
            walking = false;
            anime.SetBool("Punch", true);
            attack = false;
            StartCoroutine(Attack(c));
            
        }
    }
    IEnumerator Attack(Collision c)
    {
        yield return new WaitForSeconds(1.5f);
        anime.SetBool("Punch", false);
        target.GetComponent<Player>().TakeDamage(damage);
        yield return new WaitForSeconds(1.5f);
        
        
        attack = true;
        walking = true;
    }

}
