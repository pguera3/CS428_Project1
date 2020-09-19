using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using Debug = UnityEngine.Debug;
public class WindTeller : MonoBehaviour
{
    public GameObject windTextObject;
    

    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=9d5ac532934a26586f44e592ab196165&units=imperial";


    void Start()
    {

        // wait a couple seconds to start and then refresh every 900 seconds

        InvokeRepeating("GetDataFromWeb", 2f, 900f);
    }

    void GetDataFromWeb()
    {

        StartCoroutine(GetRequest(url));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else
                {
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                }
                int first = webRequest.downloadHandler.text.IndexOf("speed");
                int second = webRequest.downloadHandler.text.IndexOf("deg");
                string trim = webRequest.downloadHandler.text.Substring(first, second - first);
                string wind = trim.Trim('s','p','e','e','d', '}', ':', '"', ':', ',');
                windTextObject.GetComponent<TextMeshPro>().text = wind + "mph";

            }
        }
    }

}
