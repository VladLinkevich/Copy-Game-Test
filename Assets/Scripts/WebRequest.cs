using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    bool _flag;
    private void Awake()
    {
        string flagStr = PlayerPrefs.GetString("FirstStart");
        
        
        if (flagStr != String.Empty)
        {
            Boolean.TryParse(flagStr, out _flag);
        }
        else
        {
            _flag = false;
        }

        if (_flag == false)
        {
            StartCoroutine(GetRequest("https://giveus.party/whitetest"));
        }
    }

    private IEnumerator GetRequest(string uri)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        string[] pages = uri.Split('/');
        int page = pages.Length - 1;

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                if (webRequest.downloadHandler.text != String.Empty)
                {
                    Debug.Log("Nice");
                    PlayerPrefs.SetString("Web text", webRequest.downloadHandler.text);
                    _flag = true;
                }
                break;
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetString("FirstStart",
            _flag ? Boolean.TrueString : Boolean.FalseString);
    }
}
