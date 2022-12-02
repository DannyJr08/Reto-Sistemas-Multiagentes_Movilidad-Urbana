using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public List<GameObject> waypoints;

    public float speed = 2f;
    public int index = 0;
    private int rayCont = 0;
    private bool semaphore;
    //    public bool botCollide = false
    [SerializeField] private GameObject light;

        
    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        semaphore = light.GetComponent<Semaphore>().redlight;







        // - - - - - - - - - - - - - - - - - - - - - - - 

        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, 10)) { 
            print("There is something in front of the object!");
            transform.position = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, 0);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - 








        if ((semaphore && index == 5) || (!semaphore && index == 17)) // RedLight 1 On
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
