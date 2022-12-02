using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public List<GameObject> waypoints;

    public float speed = 2f;
    public int index = 0;

    public int carId;
    private bool semaphore;

    [SerializeField] GameObject light;

    LlamarApi api;

    private void Start()
    {
        api = LlamarApi.Instance;
    }

    private void FixedUpdate()
    {
        semaphore = light.GetComponent<SemaphoreScript>().redlight;

        index = api.variablesGuardar.agents[carId].loc;
        // Debug.Log(api.variablesGuardar.loc);

        Vector3 destination = waypoints[index].transform.position;
        Vector3 newPos = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(waypoints[index].transform.position - transform.position);
        transform.position = newPos;
        // float distance = Vector3.Distance(transform.position, destination);

        
  
    }

   
}
