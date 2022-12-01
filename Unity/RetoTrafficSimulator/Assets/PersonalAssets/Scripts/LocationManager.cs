/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
//libreria
using Newtonsoft.Json;
//using System.Text.Json;
//using System.Text.Json.Serialization;


public enum RequestType
{ //estructura de datos
    GET = 0,
    POST = 1,
    PUT = 2
}

public class Location
{
    public int x { get; set; }
    public int y { get; set; }
}

public class LocationManager : MonoBehaviour
{
    private string uri = "https://sistemasmultiagentes.azurewebsites.net/api/GetSingleCoordinate?code=ziosHdybH7_DJW32cxsufd4XmmLjpXaoSwUGUB2j8__bAzFujJClAA==";
    private string uriMultiple = "https://sistemasmultiagentes.azurewebsites.net/api/GetMultipleCoordinates?code=4QeIn32KSfMEViu0TGrmFM9s4VUqNkFe158kTNjcN8GNAzFuToK--Q==";
    //para analizar el caso base https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.Get.html

    private string simpleLocation = string.Empty;
    private string multipleLocation = string.Empty;

    ScoreManager scoreManager;

    float timer = 0;
    float waitTimer = 5;
    //private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = ScoreManager.Instance; //llamada
        StartCoroutine(llamarAPIsingle());
        StartCoroutine(llamarAPImultiple());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator llamarAPIsingle()
    {
        string direccion = uri + "&pickupId=24";
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
            Location location = JsonConvert.DeserializeObject<Location>(getText);
        }
    }

    IEnumerator llamarAPImultiple()
    {
        string direccion = uriMultiple + "&size=40";
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
            Location[] locations = JsonConvert.DeserializeObject<Location[]>(getText);
            if (scoreManager != null)
            {
                scoreManager.nuevasPosiciones(locations); //guarda a score manager
            }
        }

    }



    private UnityWebRequest CreateGetRequest(string path, RequestType type = RequestType.GET)
    {
        var request = new UnityWebRequest(path, type.ToString());
        //request.SendMessage
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        string getText = request.downloadHandler.text;
        return request;
    }
}*/