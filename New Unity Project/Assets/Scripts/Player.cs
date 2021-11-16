using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int hp;
    public int damage;
    public GameObject rock;
    public int rockspeed = 500;
    private bool throwable = true;
    Camera cam;
    public GameObject hand;
    int item = 1;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            item--;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            item++;
        }
        if (Input.GetAxis("Fire1") == 1 && throwable && item == 1)
        {
            throwable = false;
            fire();
            StartCoroutine(timer());
        }
    }

    public void TakeDamage(int d)
    {
        hp = hp - d;

        if(hp <= 0)
        {
            Debug.Log("game over");
        }
    }
    void fire()
    {
        GameObject bullet = Instantiate(rock, hand.transform.position + cam.transform.forward, cam.transform.rotation);

    }
    private IEnumerator timer()
    {

        yield return new WaitForSeconds(2.0f);
        throwable = true;
    }
}
