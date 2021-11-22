using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int damage;
    public GameObject target;
    public bool attack = true;

    // Start is called before the first frame update
    void Start()
    {
        target = this.GetComponent<navigate>().player;
    }
    void Update()
    {
        
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
            attack = false;
            StartCoroutine(Attack(c));
            
        }
    }
    IEnumerator Attack(Collision c)
    {
        c.gameObject.GetComponent<Player>().TakeDamage(damage);
        yield return new WaitForSeconds(1);
        attack = true;
    }

}
