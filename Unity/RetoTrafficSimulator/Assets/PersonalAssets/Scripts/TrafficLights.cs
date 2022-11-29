using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLights : MonoBehaviour
{ 
    public GameObject light;
    public float s_time; //  The Starting time needs to vary from 1 to 2 seconds for each Square

    private Vector3 red_y;
    private Vector3 yellow_y;
    private Vector3 green_y;

    private bool greenLight = false;

    // Start is called before the first frame update
    void Start()
    {
        red_y = light.transform.position;
        yellow_y = red_y - new Vector3(0, -1, 0);
        green_y = red_y - new Vector3(0, -2, 0);

    // InvokeRepeating(StartCoroutine(ChangeLights()), startingTime, 5f);
    StartCoroutine(TheRoutine(s_time));
    }

    IEnumerator TheRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        
        while (true)
        {
            StartCoroutine(ChangeLights());
            yield return new WaitForSeconds(5f);
        }
     }

    IEnumerator ChangeLights()
    {
        if (greenLight == true)
        {
            for (int i = 0; i < 6; i++)
            {
                if (i%2 == 0)
                {
                    light.transform.position = new Vector3(0, -100, 0);
                }
                else
                {
                    TurnOn("YellowLight");
                }
                yield return new WaitForSeconds(0.5f);
            }
            greenLight = false;
            TurnOn("RedLight");
        }
        else if (greenLight == false)
        {
            TurnOn("GreenLight");
        }
    }

   

    void TurnOn(string light_seq)
    {
        Debug.Log("Turning " + light_seq + " on...");

        if (light_seq == "YellowLight")
        {
            light.transform.position = yellow_y;
            light.GetComponent<Light>().color = Color.yellow;
        }
        else if (light_seq == "RedLight")
        {
            light.transform.position = red_y;
            light.GetComponent<Light>().color = Color.red;
        }
        else if (light_seq == "GreenLight")
        {
            light.transform.position = green_y;
            light.GetComponent<Light>().color = new Color32(61, 161, 27, 255);
        }
    }
    
}
