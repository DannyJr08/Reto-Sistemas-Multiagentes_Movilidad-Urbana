using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public List<GameObject> waypoints;

    public float speed = 2f;
    public int index = 0;

    private bool semaphore;

    [SerializeField] GameObject light;

        
    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        semaphore = light.GetComponent<Semaphore>().redlight;







        // - - - - - - - - - - - - - - - - - - - - - - - 

        // Check List of Waypoints, fill up waypoints based if they're occupied

        // - - - - - - - - - - - - - - - - - - - - - - - 








        if ((semaphore && index == 15) || (!semaphore && index == 49)) // RedLight 1 On
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, 0);
        }
        else
        {

            Vector3 destination = waypoints[index].transform.position;
            Vector3 newPos = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(waypoints[index].transform.position - transform.position);
            transform.position = newPos;
            float distance = Vector3.Distance(transform.position, destination);

            if (distance <= 0.05)
            {
                if (index < waypoints.Count - 1)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
            }
        }
    }

   
}
