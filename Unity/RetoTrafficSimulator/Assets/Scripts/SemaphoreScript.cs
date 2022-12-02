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

        float apiTimer = api.variablesGuardar.semaphoreTimer;
        StartCoroutine(ChangeLights(apiTimer));
        StartCoroutine(ChangeLights_2(apiTimer));
        // Debug.Log(apiTimer);

    }

    IEnumerator ChangeLights(float timer)
    {
        while (true)
        {
            redlight = false;

            // Green Light
            red_light.SetActive(false);
            green_light.SetActive(true);
            yield return new WaitForSeconds(timer / 2);

            // Yellow Light
            yellow_light.SetActive(true);
            green_light.SetActive(false);
            yield return new WaitForSeconds(timer / 2);
            
            // Red Light
            redlight = true;
            yellow_light.SetActive(false);
            red_light.SetActive(true);
            yield return new WaitForSeconds(timer);
        }
    }

    IEnumerator ChangeLights_2(float timer)
    {
        yield return new WaitForSeconds(timer);

        while (true)
        {
            // Green Light
            red_light_2.SetActive(false);
            green_light_2.SetActive(true);
            yield return new WaitForSeconds(timer / 2);


            // Yellow Light
            yellow_light.SetActive(true);
            green_light.SetActive(false);
            yield return new WaitForSeconds(timer / 2);


            // Red Light
            yellow_light_2.SetActive(false);
            red_light_2.SetActive(true);
            yield return new WaitForSeconds(timer);
        }
    }
}
