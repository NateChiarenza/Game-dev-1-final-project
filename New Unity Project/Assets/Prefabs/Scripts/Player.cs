using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public int hp;
    public int damage;
    public GameObject rock;
    public GameObject arr;
    public int rockspeed = 500;
    private bool throwable = true;
    Camera cam;
    public GameObject hand;
    public int item = 1;
    public GameObject Equip1;
    public GameObject Equip2;
    public bool leave = false;
    public static int level = 2;
    public int arrowsLeft = 10;
    public bool detected = false;
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
            if(item < 1)
            {
                item = 2;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            item = (item +1) %3;
            if (item == 0 )
            {
                item = 1;
            }
        }
        if(item == 1)
        {
            Equip1.SetActive(true);
            Equip2.SetActive(false);
            if (Input.GetAxis("Fire1") == 1 && throwable)
            {
                throwable = false;
                fire(1);
                StartCoroutine(timer());
            }
        }
        
        if(item == 2)
        {
            Equip1.SetActive(false);
            Equip2.SetActive(true);
            if (Input.GetAxis("Fire1") == 1 && throwable && arrowsLeft >0)
            {
                throwable = false;
                arrowsLeft--;
                fire(2);
                StartCoroutine(timer());
            }
        }
        
    }

    public void TakeDamage(int d)
    {
        hp = hp - d;

        if(hp <= 0)
        {
            
        }
    }
    void fire(int type)
    {
        if(type == 1)
        {
            GameObject bullet = Instantiate(rock, hand.transform.position + cam.transform.forward, cam.transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        }
        if (type == 2)
        {
            GameObject arrow = Instantiate(arr, hand.transform.position + cam.transform.forward, cam.transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        }


    }
    private IEnumerator timer()
    {

        yield return new WaitForSeconds(2.0f);
        throwable = true;
    }
    private void OnTriggerStay(Collider t)
    {
        if(t.gameObject.tag == "PowerBox" && !leave)
        {
           GetComponentInChildren<PlayerUI>().Interact.text = "Press E to turn of the powerbox"; 
            if(Input.GetAxis("Interact") == 1)
            {
                leave = true;
                GetComponentInChildren<PlayerUI>().Interact.text = "";
                var lights = GameObject.FindGameObjectsWithTag("Light");
                foreach (var light in lights)
                {
                    light.gameObject.SetActive(false);

                }
            }
        }
        if (t.gameObject.tag == "Finish" && leave)
        {
            GetComponentInChildren<PlayerUI>().Interact.text = "Press E to Leave";
            if (Input.GetAxis("Interact") == 1)
            {
                GetComponentInChildren<PlayerUI>().Interact.text = "";
                SceneManager.LoadScene(level);
                level++;
            }
        }
    }
}
