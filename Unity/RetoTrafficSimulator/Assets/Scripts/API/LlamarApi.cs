using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
//libreria
using Newtonsoft.Json;
//using System.Text.Json;
//using System.Text.Json.Serialization;

/*
public enum RequestType
{ //estructura de datos
    GET = 0,
    POST = 1,
    PUT = 2
}
*/

public class Agent
{
    public int id { get; set; }
    // public Position position { get; set; }
    public int loc { get; set; }
}

/*public class Position
{
    public int x { get; set; }
    public int y { get; set; }
}*/

public class Root
{
    public List<Agent> agents { get; set; }
    public int semaphoreTimer { get; set; }
    public List<Semaphore> semaphores { get; set; }
    //public int loc { get; set; }

}

public class Semaphore
{
    public bool greenLeft { get; set; }
    public bool greenUp { get; set; }
    public int id { get; set; }
}

public class LlamarApi : Singleton<LlamarApi>
{
    private string uri = "http://chiron.pythonanywhere.com/";
    //para analizar el caso base https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.Get.html
    public Root variablesGuardar;

    public GameObject semaforos;
    public GameObject[] carros;

    public float timer;
    public float wait = 0.2f;
    public int tempora;

    //public int loc;
    //float timer = 0;
    //float waitTimer = 5;
    //private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(mesa("?reset=1"));
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > wait)
        {
            tempora += 1;
            StartCoroutine(mesa("?reset=0"));
            timer = 0f;
        }
        //List<Semaphore> lista = variablesGuardar.semaphores;
        //Debug.Log("El semaforo es" + lista[0].greenLeft);

        //Debug.Log("El semaforo es" + variablesGuardar.semaphores[0].greenLeft);
        //Debug.Log("La cantidad de agentes seran " + variablesGuardar.semaphoreTimer);
    }

    

    IEnumerator mesa(string reset)
    {
        string direccion = uri + reset;
        var getRequest = CreateGetRequest(direccion);
        yield return getRequest.SendWebRequest();

        if (getRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(getRequest.error);
        }
        else
        {
            string getText = getRequest.downloadHandler.text;
            Debug.Log(getText);
            variablesGuardar = JsonConvert.DeserializeObject<Root>(getText);
        }
    }

    


    private UnityWebRequest CreateGetRequest(string path , RequestType type = RequestType.GET)
    {
        var request = new UnityWebRequest(path, type.ToString());
        //request.SendMessage
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        string getText = request.downloadHandler.text;
        return request;
    }
}
