using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemaphoreScript : MonoBehaviour
{
    public GameObject red_light;
    public GameObject yellow_light;
    public GameObject green_light;

    public GameObject red_light_2;
    public GameObject yellow_light_2;
    public GameObject green_light_2;

    public bool redlight = true;

    LlamarApi api;

    // Start is called before the first frame update
    void Start()
    {
        api = LlamarApi.Instance;

        red_light.SetActive(true);
        yellow_light.SetActive(false);
        green_light.SetActive(false);

        red_light_2.SetActive(true);
        yellow_light_2.SetActive(false);
        green_light_2.SetActive(false);


        // InvokeRepeating(StartCoroutine(ChangeLights()), startingTime, 5f);
        StartCoroutine(ChangeLights(api.variablesGuardar.semaphoreTimer));
        StartCoroutine(ChangeLights_2(api.variablesGuardar.semaphoreTimer));
        Debug.Log(api.variablesGuardar.semaphoreTimer);

    }

    IEnumerator ChangeLights(float timer)
    {
        // yield return new WaitForSeconds(time);
        float gtime = timer * 0.625f;
        float ytime = timer * 0.375f;
        float ywait = ytime * 0.083333333333f;

        while (true)
        {
            redlight = false;

            // Green Light
            red_light.SetActive(false);
            green_light.SetActive(true);
            yield return new WaitForSeconds(gtime);

            // Yellow Light
            green_light.SetActive(false);
            for (int i = 0; i < ytime * 2; i++)
            {
                if (i % 2 == 0) { yellow_light.SetActive(false); } else { yellow_light.SetActive(true); }
                yield return new WaitForSeconds(ywait);
            }
            // Red Light
            redlight = true;
            yellow_light.SetActive(false);
            red_light.SetActive(true);
            yield return new WaitForSeconds(timer);
        }
    }

    IEnumerator ChangeLights_2(float timer)
    {
        float gtime = timer * 0.625f;
        float ytime = timer * 0.375f;
        float ywait = ytime * 0.083333333333f;

        yield return new WaitForSeconds(timer);

        while (true)
        {
            // Green Light
            red_light_2.SetActive(false);
            green_light_2.SetActive(true);
            yield return new WaitForSeconds(gtime);

            // Yellow Light
            green_light_2.SetActive(false);
            for (int i = 0; i < ytime; i++)
            {
                if (i % 2 == 0) { yellow_light_2.SetActive(false); } else { yellow_light_2.SetActive(true); }
                yield return new WaitForSeconds(ywait);
            }
            // Red Light
            yellow_light_2.SetActive(false);
            red_light_2.SetActive(true);
            yield return new WaitForSeconds(timer);
        }
    }
}
