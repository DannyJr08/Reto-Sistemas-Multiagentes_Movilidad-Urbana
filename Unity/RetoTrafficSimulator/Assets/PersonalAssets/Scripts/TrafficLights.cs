using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLights : MonoBehaviour
{ 
    public GameObject red_light;
    public GameObject yellow_light;
    public GameObject green_light;

    public float s_time; //  The Starting time needs to vary from 1 to 2 seconds for each Square

    private bool greenLight = false;

    // Start is called before the first frame update
    void Start()
    {
        red_light.SetActive(true);
        yellow_light.SetActive(false);
        green_light.SetActive(false);


        // InvokeRepeating(StartCoroutine(ChangeLights()), startingTime, 5f);
        StartCoroutine(ChangeLights(s_time));
    }

    IEnumerator ChangeLights(float time)
    {
        yield return new WaitForSeconds(time);
        
        while (true)
        {
            // Green Light
            red_light.SetActive(false);
            green_light.SetActive(true);
            yield return new WaitForSeconds(5f);

            // Yellow Light
            green_light.SetActive(false);
            for (int i = 0; i < 6; i++)
            {
                if (i % 2 == 0) { yellow_light.SetActive(false); } else { yellow_light.SetActive(true); }
                yield return new WaitForSeconds(0.5f);
            }
            // Red Light
            yellow_light.SetActive(false);
            red_light.SetActive(true);
            yield return new WaitForSeconds(8f);
        }
     }
}
