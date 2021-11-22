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
    public bool shooter = false;
    public GameObject arr;
    public Transform aim;
    public bool inSight = false;
    bool canFire = true;
    string area;
    public int A=1;
    // Start is called before the first frame update
    void Start()
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

        int n = 2;
        for (int i = 0; i < points.Length; i++)
        {
            n = Random.Range(0, GameObject.FindGameObjectsWithTag(area).Length-1);
            points[i] = GameObject.FindGameObjectsWithTag(area)[n].transform;
        }
        GotoNextPoint();
        player = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        
        var ray = new Ray(aim.transform.position, aim.transform.forward);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 lookPlayer = transform.position - player.transform.position;
       
        if (Physics.SphereCast(ray, viewR, out hitdata,5, mask))
        {
            
            if (hitdata.transform.gameObject.tag == "rock" && !playerFound)
            {
                enemy.destination = hitdata.transform.position;
            }
            if (hitdata.transform.gameObject.tag == "Player")
            {
                
                playerFound = true;
                if (Vector3.Dot(forward, lookPlayer) < 1 && Vector3.Dot(forward, lookPlayer) > -5)
                {
                    inSight = true;
                        if (hitdata.transform.gameObject.tag == "Player")
                        {
                        player.GetComponent<Player>().detected = true;
                            
                            if (!shooter)
                            {
                                enemy.destination = player.transform.position;
                            }
                            else
                            {
                                enemy.destination = transform.position;
                            }

                            StartCoroutine(Hunt());
                        }
                    if (inSight && shooter)
                    {
                        enemy.destination = transform.position;

                        enemy.transform.LookAt(player.transform.Find("Main Camera").Find("Target").transform.position);
                        if (canFire)
                        {
                            canFire = false;
                            GameObject arrow = Instantiate(arr, aim.position + aim.forward, aim.rotation * Quaternion.Euler(0f, 180f, 0f));
                            arrow.gameObject.tag = "bad arrow";
                            StartCoroutine(fire());
                        }

                    }
                }
                else
                {
                    inSight = false;
                }
            }
        }
        
       
        if (!enemy.pathPending && enemy.remainingDistance < 0.1f && !playerFound && !inSight)
        {
            GotoNextPoint();
        }
        if (playerFound && !shooter)
        {
            enemy.destination = player.transform.position;
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

    private void OnCollisionStay(Collision c)
    {
        if(c.gameObject.tag == "rock" || c.gameObject.tag == "Arrow")
        {
            Destroy(c.gameObject);
        }
        if (c.gameObject.tag == "Player")
        {
            inSight = true;
            playerFound = true;
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
            if (shooter)
            {
                GotoNextPoint();
            }
        }
       
    }
    private IEnumerator fire()
    {
        yield return new WaitForSeconds(1);
        canFire = true;
    }
    private void OnDestroy()
    {
        player.GetComponent<Player>().detected = false;
    }
}
