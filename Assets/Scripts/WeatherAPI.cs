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
public class WeatherAPI : MonoBehaviour
{
    public GameObject weatherTextObject;
    public GameObject humidityTextObject;
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
                var stringAPI = webRequest.downloadHandler.text;
                int first = stringAPI.IndexOf("temp");
                int second = stringAPI.IndexOf("feels_like");
                string trimTemp = stringAPI.Substring(first, second - first);
                string temp = trimTemp.Trim('t', 'e', 'm', 'p', ',', ':', '"', ':', ',');
                weatherTextObject.GetComponent<TextMeshPro>().text = temp + " F";

                int start = stringAPI.IndexOf("humidity");
                int end = stringAPI.IndexOf("visibility");
                string trimHumid = stringAPI.Substring(start, end - start);
                string humid = trimHumid.Trim('h', 'u', 'm', 'i', 'd', 'i', 't', 'y', '}', ':', '"', ':', ',');
                humidityTextObject.GetComponent<TextMeshPro>().text = humid + " %";
            }
        }
    }

}
