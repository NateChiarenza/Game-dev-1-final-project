using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class navigate : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform[] points;
    private int destPoint = 0;
    
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
        if(Physics.SphereCast(ray,1.0f, out hitdata))
        {
            enemy.destination = hitdata.transform.position;
        }
        if (!enemy.pathPending && enemy.remainingDistance < 0.5f)
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
}
