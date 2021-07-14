using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    public GameObject uniWebViewGameObject;
    public string URI;
    bool _lead;
    private void Awake()
    {
        string leadStr = PlayerPrefs.GetString("lead");
        
        
        if (leadStr != String.Empty)
        {
            Boolean.TryParse(leadStr, out _lead);
            if (_lead == false)
            {
                uniWebViewGameObject.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(GetRequest(URI));
        }
    }

    private IEnumerator GetRequest(string uri)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);

        yield return webRequest.SendWebRequest();

        string[] pages = uri.Split('/');
        int page = pages.Length - 1;
        _lead = false;
        
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
                    PlayerPrefs.SetString("endpoint", webRequest.downloadHandler.text);
                    _lead = true;
                }
                break;
        }
        
        if (_lead == false)
        {
            uniWebViewGameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetString("lead",
            _lead ? Boolean.TrueString : Boolean.FalseString);
    }
}
