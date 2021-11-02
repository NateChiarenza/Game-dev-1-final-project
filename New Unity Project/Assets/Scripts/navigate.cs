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
    // Start is called before the first frame update
    void Start()
    {
        GotoNextPoint();

    }

    // Update is called once per frame
    void Update()
    {
        var ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hitdata;
        if(Physics.SphereCast(ray,1.0f, out hitdata,mask))
        {

            if( hitdata.transform.gameObject.tag == "rock")
            {
                enemy.destination = hitdata.transform.position;
            }
            if (hitdata.transform.gameObject.tag == "Player" )
            {
                enemy.destination = hitdata.transform.position;
            }
        }
        if (!enemy.pathPending && enemy.remainingDistance < 0.1f)
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

    private void OnCollisionStay(Collision c)
    {
        if(c.gameObject.tag == "rock")
        {
            Destroy(c.gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "rock")
        {
            Debug.Log("hello");
            enemy.destination = other.gameObject.transform.position;
        }
    }
}
