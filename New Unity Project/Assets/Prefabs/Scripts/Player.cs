using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
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
    public bool Diamond = false;
    public static int level = 1;
    public int arrowsLeft = 10;
    public bool detected = false;
    bool failsafe = true;
    public Animator anime;
    public bool walking = false;
    public GameObject[] points;
    public AudioSource source;
    public AudioClip clip;
    public bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        source = GameObject.Find("HM_crossbow_finished").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        anime.SetBool("Walking", walking);
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && throwable)
        {
            throwable = false;
            anime.SetBool("Switch", true);
            StartCoroutine(swap(1));
           
           
            
           
            
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && throwable)
        {
            throwable = false;
            anime.SetBool("Switch", true);
            StartCoroutine(swap(2));
            
           

        }
        if(item == 1)
        {
            Equip1.SetActive(true);
            Equip2.SetActive(false);
            if (Input.GetAxis("Fire1") == 1 && throwable)
            {
                anime.SetBool("Fire", true);
                throwable = false;
                
                
                
                StartCoroutine(timer());
                
            }
        }
        
        if(item == 2)
        {
            Equip1.SetActive(false);
            Equip2.SetActive(true);
            if (Input.GetAxis("Fire1") == 1 && throwable && arrowsLeft >0)
            {
                anime.SetBool("Bow", true);
                throwable = false;
                arrowsLeft--;
                fire(2);
                StartCoroutine(timer());
                
            }
        }
        if (detected && failsafe)
        {
            failsafe = false;
            StartCoroutine(detectedFailSafe());
        }
        
    }

    public void TakeDamage(int d)
    {
        hp = hp - d;

        if(hp <= 0)
        {
            dead = true;
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
    private IEnumerator detectedFailSafe()
    {

        yield return new WaitForSeconds(15.0f);
        detected = false;
        failsafe = true;
    }
    private IEnumerator timer()
    {

       
        if (item == 1)
        {
            yield return new WaitForSeconds(.8f);
            anime.SetBool("Fire", false);
            fire(1);
        }
        else
        {
            yield return new WaitForSeconds(.05f);
            anime.SetBool("Bow", false);
        }
        yield return new WaitForSeconds(1f);
        throwable = true;
    }
    private IEnumerator swap(int i)
    {
        yield return new WaitForSeconds(.3f);
        if (i == 1)
        {
            item--;
            if (item < 1)
            {
                item = 2;
            }
        }
        else
        {
            item = (item + 1) % 3;
            if (item == 0)
            {
                item = 1;
            }
        }
        anime.SetBool("Switch", false);
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
        if (t.gameObject.tag == "Diamond Box" && !Diamond)
        {
            GetComponentInChildren<PlayerUI>().Interact.text = "Press E to open the Diamond Gate";
            if (Input.GetAxis("Interact") == 1)
            {
                source.PlayOneShot(clip);
                Diamond = true;
                GameObject.Find("gate").SetActive(false);
                GetComponentInChildren<PlayerUI>().Interact.text = "";
               
            }
        }
        if (t.gameObject.tag == "Diamond" && Diamond)
        {
            GetComponentInChildren<PlayerUI>().Interact.text = "Press E to take the Dimond";
            if (Input.GetAxis("Interact") == 1)
            {
                leave = true;
                GameObject.Find("Diamond").SetActive(false);
                GetComponentInChildren<PlayerUI>().Interact.text = "";
                
                foreach (var e in points)
                {
                    e.gameObject.SetActive(true);
                }

            }
        }
        if (t.gameObject.tag == "Finish" && leave)
        {
            GetComponentInChildren<PlayerUI>().Interact.text = "Press E to Leave";
            if (Input.GetAxis("Interact") == 1)
            {
                leave = false;
                GetComponentInChildren<PlayerUI>().Interact.text = "";
                level++;
                if (level >= 4)
                {
                    level = 0;
                }
                SceneManager.LoadScene(level);
                
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        GetComponentInChildren<PlayerUI>().Interact.text = "";
    }
}
