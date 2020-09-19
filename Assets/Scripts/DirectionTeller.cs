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
using System.Collections.Specialized;

public class DirectionTeller : MonoBehaviour
{
    public GameObject directionTextObject;
    public GameObject windSock;    
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

                

                if (webRequest.isNetworkError)
                {
                    Debug.Log(": Error: " + webRequest.error);
                }
                else
                {
                    Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                }

                string deg;
                var str = webRequest.downloadHandler.text;
                int first = str.IndexOf("deg");               
                int second = str.IndexOf("all");
                string trimDeg = str.Substring(first, second - first);
                if (trimDeg.Contains("gust"))
                {
                    second = str.IndexOf("gust");
                    string newTrimDeg = str.Substring(first, second - first);
                    deg = newTrimDeg.Trim('d', 'e', 'g', '}', ':', '"', ',');
                }
                else
                {
                    deg= trimDeg.Trim('d','e','g', '}', ':', '"', ',', 'c', 'l', 'o', 'u', 'd', 's', '{');
                }
                
                ////string deg = webRequest.downloadHandler.text.Substring(first);
                //directionTextObject.GetComponent<TextMeshPro>().text = deg;
                float direction = float.Parse(deg);

                int begin = webRequest.downloadHandler.text.IndexOf("speed");
                int end = webRequest.downloadHandler.text.IndexOf("deg");
                string trimWind = webRequest.downloadHandler.text.Substring(begin, end - begin);
                string wind = trimWind.Trim('s', 'p', 'e', 'e', 'd', '}', ':', '"', ':', ',');
                float windSpeed = float.Parse(wind);
                windTextObject.GetComponent<TextMeshPro>().text = wind + "mph";


                if (direction > 337.6 || direction < 22.5)
                    directionTextObject.GetComponent<TextMeshPro>().text = "N";
                if (direction > 22.6 && direction < 67.5)
                    directionTextObject.GetComponent<TextMeshPro>().text = "NE";
                if (direction > 67.6 && direction < 112.5)
                    directionTextObject.GetComponent<TextMeshPro>().text = "E";
                if (direction > 112.6 && direction < 157.5)
                    directionTextObject.GetComponent<TextMeshPro>().text = "SE";
                if (direction > 157.6 && direction < 202.5)
                    directionTextObject.GetComponent<TextMeshPro>().text = "S";
                if (direction > 202.6 && direction < 247.5)
                    directionTextObject.GetComponent<TextMeshPro>().text = "SW";
                if (direction > 247.6 && direction < 292.5)
                     directionTextObject.GetComponent<TextMeshPro>().text = "W";
                if (direction >292.6 && direction < 337.5)
                    directionTextObject.GetComponent<TextMeshPro>().text = "SW";


                windSock.transform.rotation = Quaternion.Euler(90, 0f, -direction);
                windSock.transform.localScale = new Vector3(1f, windSpeed * .2f, 4.058001f);




            }
        }
    }

}
