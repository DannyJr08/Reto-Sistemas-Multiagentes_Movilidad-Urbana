using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaphore : MonoBehaviour
{
    public GameObject red_light;
    public GameObject yellow_light;
    public GameObject green_light;

    public GameObject red_light_2;
    public GameObject yellow_light_2;
    public GameObject green_light_2;

    public bool redlight = true;

    // Start is called before the first frame update
    void Start()
    {
        red_light.SetActive(true);
        yellow_light.SetActive(false);
        green_light.SetActive(false);

        red_light_2.SetActive(true);
        yellow_light_2.SetActive(false);
        green_light_2.SetActive(false);


        // InvokeRepeating(StartCoroutine(ChangeLights()), startingTime, 5f);
        StartCoroutine(ChangeLights());
        StartCoroutine(ChangeLights_2());

    }

    IEnumerator ChangeLights()
    {
        // yield return new WaitForSeconds(time);

        while (true)
        {
            redlight = false;

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
            redlight = true;
            yellow_light.SetActive(false);
            red_light.SetActive(true);
            yield return new WaitForSeconds(8f);
        }
    }

    IEnumerator ChangeLights_2()
    {
        
        yield return new WaitForSeconds(8f);

        while (true)
        {
            // Green Light
            red_light_2.SetActive(false);
            green_light_2.SetActive(true);
            yield return new WaitForSeconds(5f);

            // Yellow Light
            green_light_2.SetActive(false);
            for (int i = 0; i < 6; i++)
            {
                if (i % 2 == 0) { yellow_light_2.SetActive(false); } else { yellow_light_2.SetActive(true); }
                yield return new WaitForSeconds(0.5f);
            }
            // Red Light
            yellow_light_2.SetActive(false);
            red_light_2.SetActive(true);
            yield return new WaitForSeconds(8f);
        }
    }
}
