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
using System;

public class WeatherPicture : MonoBehaviour
{
    public GameObject clearSky;
    public GameObject fewClouds;
    public GameObject scatteredClouds;
    public GameObject brokenClouds;
    public GameObject showerRain;
    public GameObject rain;
    public GameObject thunderstorm;
    public GameObject snow;
    public GameObject mist;

    public int count = -1;


    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=9d5ac532934a26586f44e592ab196165&units=imperial";


    void Start()
    {
        
        // wait a couple seconds to start and then refresh every 900 seconds
        InvokeRepeating("GetDataFromWeb", 2f, 900f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            count++;
            if (count == 1)
            {
                brokenClouds.SetActive(false);
                thunderstorm.SetActive(true);
            }
            if (count == 2)
            {
                thunderstorm.SetActive(false);
                showerRain.SetActive(true);
            }
            if (count == 3)
            {
                showerRain.SetActive(false);
                rain.SetActive(true);
            }
            if (count == 4)
            {
                rain.SetActive(false);
                snow.SetActive(true);
            }
            if (count == 5)
            {
                snow.SetActive(false);
                mist.SetActive(true);
            }
            if (count == 6)
            {
                mist.SetActive(false);
                clearSky.SetActive(true);
            }   
            if (count == 7)
            {
                clearSky.SetActive(false);
                fewClouds.SetActive(true);
            }
            if (count == 8)
            {
                fewClouds.SetActive(false);
                scatteredClouds.SetActive(true);
            }
            if (count == 9)
            {
                scatteredClouds.SetActive(false);
                brokenClouds.SetActive(true);
            }
            if (count == 10)
            {
                count = 0;
                brokenClouds.SetActive(false);
            }
                    
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            count--;
            if (count == 1)
            {
                brokenClouds.SetActive(true);
                thunderstorm.SetActive(false);
            }
            if (count == 2)
            {
                thunderstorm.SetActive(true);
                showerRain.SetActive(false);
            }
            if (count == 3)
            {
                showerRain.SetActive(true);
                rain.SetActive(false);
            }
            if (count == 4)
            {
                rain.SetActive(true);
                snow.SetActive(false);
            }
            if (count == 5)
            {
                snow.SetActive(true);
                mist.SetActive(false);
            }
            if (count == 6)
            {
                mist.SetActive(true);
                clearSky.SetActive(false);
            }
            if (count == 7)
            {
                clearSky.SetActive(true);
                fewClouds.SetActive(false);
            }
            if (count == 8)
            {
                fewClouds.SetActive(true);
                scatteredClouds.SetActive(false);
            }
            if (count == 9)
            {
                scatteredClouds.SetActive(true);
                brokenClouds.SetActive(false);
            }
            if (count == 0)
                count = 9;
        }



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
                int first = webRequest.downloadHandler.text.IndexOf("id");
                int second = webRequest.downloadHandler.text.IndexOf("main");
                string trim = webRequest.downloadHandler.text.Substring(first, second - first);
                string pic= trim.Trim('i','d', ':', '"', ':', ',');
                int id = Int32.Parse(pic);

                checkWeather(id);
                
 
                    

            }
        }
    }

    public void checkWeather(int id)
    {
        if (id >= 200 && id < 300)
        {
            count = 1;
            thunderstorm.SetActive(true);
        }
        if ((id >= 300 && id < 500) || id >= 520 && id < 600)
        {
            count = 2;
            showerRain.SetActive(true);
        }
        if (id >= 500 && id < 510)
        {
            count = 3;
            rain.SetActive(true);
        }
        if ((id >= 600 && id < 700) || id == 511)
        {
            count = 4;
            snow.SetActive(true);
        }
        if (id > 700 && id < 800)
        {
            count = 5;
            mist.SetActive(true);
        }
        if (id == 800)
        {
            count = 6;
            clearSky.SetActive(true);
        }
        if (id == 801)
        {
            count = 7;
            fewClouds.SetActive(true);
        }
        if (id == 802)
        {
            count = 8;
            scatteredClouds.SetActive(true);
        }
        if (id == 803 || id == 804)
        {
            count = 9;
            brokenClouds.SetActive(true);
        }
    }

}
