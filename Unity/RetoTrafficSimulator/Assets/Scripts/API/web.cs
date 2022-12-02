using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class web : MonoBehaviour
{

  void Start()
  {
    StartCoroutine(GetText());
  }

  IEnumerator GetText()
  {
    UnityWebRequest www = UnityWebRequest.Get("http://chiron.pythonanywhere.com/");
    yield return www.Send();

    if (www.isNetworkError)
    {
      Debug.Log(www.error);
    }
    else
    {
      // Show results as text
      Debug.Log(www.downloadHandler.text);

      // Or retrieve results as binary data
      byte[] results = www.downloadHandler.data;
    }
  }




}