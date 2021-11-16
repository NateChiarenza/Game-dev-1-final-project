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
    public float viewR = 100.0f;
    RaycastHit hitdata;
    // Start is called before the first frame update
    void Start()
    {
        GotoNextPoint();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 lookPlayer = player.transform.position + transform.position;
        
        if (Vector3.Dot(forward, lookPlayer) < 6 && Vector3.Dot(forward, lookPlayer) > -4 && Vector3.Distance(player.transform.position,transform.position) < 7)
        {
            
            // Debug.Log(Vector3.Distance(player.transform.position, this.transform.position));
            var ray = new Ray(this.transform.position, this.transform.forward);
            
            if (Physics.SphereCast(ray, viewR, out hitdata, mask))
            {

                if (hitdata.transform.gameObject.tag == "rock" && !playerFound)
                {
                    enemy.destination = hitdata.transform.position;
                }
                if (hitdata.transform.gameObject.tag == "Player")
                {
                    playerFound = true;
                    enemy.destination = player.transform.position;
                    StartCoroutine(Hunt());
                }
            }
        }
       
        if (!enemy.pathPending && enemy.remainingDistance < 0.1f)
        {
            GotoNextPoint();
        }
        if (playerFound)
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
        if(c.gameObject.tag == "rock")
        {
            Destroy(c.gameObject);
        }
        if (c.gameObject.tag == "Player")
        {

            playerFound = true;
            StartCoroutine(Hunt());
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "rock")
        {
            
            enemy.destination = other.gameObject.transform.position;
        }
       
    }
    private IEnumerator Hunt()
    {
      
        yield return new WaitForSeconds(huntTime);
        if(hitdata.transform == null || hitdata.transform.gameObject.tag != "Player")
        {
            playerFound = false;
        }
       
    }
}
