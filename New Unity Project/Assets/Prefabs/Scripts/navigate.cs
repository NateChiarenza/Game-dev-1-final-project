using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class navigate : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform[] points;
    private int destPoint = 0;
    public LayerMask mask;
    public bool playerFound = false;
    public GameObject player;
    public float huntTime = 5.0f;
    public float viewR;
    RaycastHit hitdata;
    RaycastHit hitdata1;
    RaycastHit hitdata2;
    public bool shooter = false;
    public GameObject arr;
    public Transform[] aim;
    public bool inSight = false;
    bool canFire = true;
    string area;
    public int A=1;
    public bool canhunt = true;
    bool readyTo = false;
    public int range;
    
 
    // Start is called before the first frame update
    void Awake()
    {
        switch (A)
        {
            case 1:
                area = "patroll1";
                break;
            case 2:
                area = "patroll2";
                break;
            case 3:
                area = "patroll3";
                break;
            default:
                break;
        }
        enemy.avoidancePriority = Random.Range(1, 30);

        int n = 2;
        int temp = -1;
        for (int i = 0; i < points.Length; i++)
        {
            n = Random.Range(0, GameObject.FindGameObjectsWithTag(area).Length-1);
            if(n == temp)
            {
                n = Random.Range(0, GameObject.FindGameObjectsWithTag(area).Length - 1);
            }
            temp = n;
            points[i] = GameObject.FindGameObjectsWithTag(area)[n].transform;
        }
        GotoNextPoint();
        player = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        
        var ray = new Ray(aim[0].transform.position, aim[0].transform.forward);
        var ray2 = new Ray(aim[1].transform.position, aim[1].transform.forward);
        var ray3 = new Ray(aim[2].transform.position, aim[2].transform.forward);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 lookPlayer = transform.position - player.transform.position;

        
        if (canhunt)
        {
            canhunt = false;
            StartCoroutine(Hunt());

        }
        if (Vector3.Dot(forward.normalized, lookPlayer.normalized) < -.3f && Vector3.Dot(forward.normalized, lookPlayer.normalized) > -1.0f && Vector3.Distance(forward, lookPlayer) < range && ((hitdata.transform != null && hitdata.transform.tag == "Player") || (hitdata1.transform != null && hitdata1.transform.tag == "Player") || (hitdata2.transform != null && hitdata2.transform.tag == "Player")))
        {
            inSight = true;
            playerFound = true;
            if (hitdata.transform != null && hitdata.transform.tag == "Floor")
            {
                inSight = false;
                playerFound = false;
            }
           

            if (playerFound){
                StartCoroutine(Hunt());
            }
            player.GetComponent<Player>().detected = true;

            if (!shooter)
            {
                enemy.destination = player.transform.position;
            }
            else
            {
                if(Vector3.Distance(transform.position, player.transform.position) < 7 && Vector3.Distance(transform.position, player.transform.position) > 2)
                {

                    GetComponent<Enemy>().walking = false;
                    StartCoroutine(timer());
                    GetComponent<Enemy>().shooting = true;
                    enemy.destination = transform.position;
                }
                else if(Vector3.Distance(transform.position, player.transform.position) > 7 )
                {
                    readyTo = false;
                    GetComponent<Enemy>().walking = true;
                    GetComponent<Enemy>().shooting = false;
                    enemy.destination = player.transform.position;
                }
                else
                {
                    readyTo = false;
                    GetComponent<Enemy>().walking = true;
                    GetComponent<Enemy>().shooting = false;
                    enemy.destination = new Vector3(player.transform.position.x + 3, player.transform.position.y, player.transform.position.z + 3);
                }
                

                enemy.transform.LookAt(player.transform.Find("Main Camera").Find("Target").transform.position);
                if (canFire && readyTo)
                {
                    canFire = false;
                    GameObject arrow = Instantiate(arr, aim[0].position + aim[0].forward, aim[0].rotation * Quaternion.Euler(0f, 180f, 0f));
                    arrow.gameObject.tag = "bad arrow";
                    StartCoroutine(fire());
                }
                if (playerFound && !shooter)
                {
                    enemy.destination = player.transform.position;
                }
            }

        }
        else
        {
            inSight = false;
        }
         if (Physics.SphereCast(ray, viewR, out hitdata, mask))
         {
                
                if (hitdata.transform.gameObject.tag == "rock" && !playerFound)
                {
                    enemy.destination = hitdata.transform.position;
                } 
                if (hitdata.transform.gameObject.tag == "Floor")
                {
                    inSight = false; 
                }    
           
        }
        if (Physics.SphereCast(ray2, viewR, out hitdata1, mask))
        {

         

        }
        if (Physics.SphereCast(ray3, viewR, out hitdata2, mask))
        {

          

        }




        if (!enemy.pathPending && enemy.remainingDistance < 0.1f && !playerFound && !inSight)
        {
            GotoNextPoint();
        }
       
    }
    void GotoNextPoint()
    {
       
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        enemy.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    private void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag == "rock" || c.gameObject.tag == "Arrow")
        {
            
            Destroy(c.gameObject);
        }
        if (c.gameObject.tag == "Player")
        {
            inSight = true;
            playerFound = true;
            transform.LookAt(c.transform.position);
            StartCoroutine(Hunt());
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "rock" && !playerFound)
        {
            
            enemy.destination = other.gameObject.transform.position;
        }
    }
    private IEnumerator Hunt()
    {
        
        yield return new WaitForSeconds(huntTime);
        if (!inSight)
        {
            playerFound = false;
            inSight = false;
            player.GetComponent<Player>().detected = false;
           
                GotoNextPoint();
            
            canhunt = false;
        }
       
    }
    private IEnumerator fire()
    {
        yield return new WaitForSeconds(.5f);
        canFire = true;
    }
    private IEnumerator timer()
    {
        yield return new WaitForSeconds(1.5f);
        readyTo = true;
    }
}
