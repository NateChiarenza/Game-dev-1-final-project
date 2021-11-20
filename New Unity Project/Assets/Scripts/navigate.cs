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
    public float viewR = 15.0f;
    RaycastHit hitdata;
    public bool shooter = false;
    public GameObject arr;
    public Transform aim;
    public bool inSight = false;
    bool canFire = true;
    // Start is called before the first frame update
    void Start()
    {
        GotoNextPoint();

    }

    // Update is called once per frame
    void Update()
    {
        var ray = new Ray(aim.transform.position, this.transform.forward);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 lookPlayer = player.transform.position + transform.position;
        if (Physics.SphereCast(ray, viewR, out hitdata,7, mask))
        {
            Debug.Log(hitdata.transform.tag);
            if (hitdata.transform.gameObject.tag == "rock" && !playerFound)
            {
                enemy.destination = hitdata.transform.position;
            }
            if (hitdata.transform.gameObject.tag == "Player")
            {
                
                playerFound = true;
                if (Vector3.Dot(forward, lookPlayer) < 6 && Vector3.Dot(forward, lookPlayer) > -4 && Vector3.Distance(player.transform.position, transform.position) < 7)
                {
                    inSight = true;
                    // Debug.Log(Vector3.Distance(player.transform.position, this.transform.position));


                    //if (Physics.SphereCast(ray, viewR, out hitdata, mask))
                    //{

                   
                        if (hitdata.transform.gameObject.tag == "Player")
                        {
                            
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

                    //}
                    if (inSight && shooter)
                    {
                        enemy.destination = transform.position;

                        enemy.transform.LookAt(player.transform);
                        if (canFire)
                        {
                            canFire = false;
                            GameObject arrow = Instantiate(arr, aim.position + transform.forward, transform.rotation);
                            arrow.gameObject.tag = "bad arrow";
                            StartCoroutine(fire());
                        }

                    }
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
        if(other.gameObject.tag == "rock" )
        {
            
            enemy.destination = other.gameObject.transform.position;
        }
       
    }
    private IEnumerator Hunt()
    {
        
        yield return new WaitForSeconds(huntTime);
        //if (hitdata.transform == null || hitdata.transform.gameObject.tag != "Player")
        //{
        //    playerFound = false;
        //}
        if (!inSight)
        {
            playerFound = false;
            inSight = false;
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
}
