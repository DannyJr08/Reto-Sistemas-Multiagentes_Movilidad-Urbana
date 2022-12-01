using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public List<GameObject> waypoints;
    public float speed = 2f;
    public int index = 0;


    void Start()
    {
       
    }

    private void Update()
    {
        

        Vector3 destination = waypoints[index].transform.position;
        Vector3 newPos = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, speed * Time.deltaTime);
        transform.position = newPos;
        transform.rotation = Quaternion.LookRotation(newPos);
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
        
        Debug.Log("Index: " + index);
        Debug.Log("Distance: " + distance);

    }
}
