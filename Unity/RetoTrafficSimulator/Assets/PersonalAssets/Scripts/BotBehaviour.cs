using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class BotBehaviour : MonoBehaviour
{
    public GameObject[] points;
    public int destPoint = 0;
    private NavMeshAgent agent;

    // [SerializeField] private Waypoint points;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // NewPath();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        //agent.autoBraking = false;

        GotoNextPoint(true);
    }


    void GotoNextPoint(bool cond)
    {
        // Returns if no points have been set up
        if (points.Length == 0) { 

            Debug.Log("no Points");
            return;
        }
        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].transform.position;

        // Debug.Log("Current Destination 1: " + points[0].transform.position);
        // Debug.Log("Current Destination 2: " + points[1].transform.position);
        // Debug.Log("Current Destination 3: " + points[2].transform.position);

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;

        // Change Paths so it can go for a new one
        /* if (cond == false && destPoint == 0)
        {
            NewPath();         
        }*/
    }


    void NewPath()
    {
        points = GameObject.FindGameObjectsWithTag("Path_1"); //  retun GameObject[]
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint(false);
    }
}