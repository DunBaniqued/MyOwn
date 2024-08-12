using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.Networking;

public class WebApi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string _baseURl = "https://omgvamp-hearthstone-v1.p.rapidapi.com/";
    string _baseTextureURL = "https://art.hearthstonejson.com/v1/orig/";
    private string _apiHost = "omgvamp-hearthstone-v1.p.rapidapi.com";
    [SerializeField] private string _apiKey;

    private IEnumerator RequestCard()
    {
        string url = this._baseURl + "";

        using (UnityWebRequest request = new UnityWebRequest(url, "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("X-RapidAPI-Key", this._apiKey);
            request.SetRequestHeader("X-RapidAPI-Host", this._apiHost);

            yield return request.SendWebRequest();
            Debug.Log(request.responseCode);

            if (string.IsNullOrEmpty(request.error))
            {
                Dictionary<string, string> response = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.downloadHandler.text);
            }
        }
    }
    private IEnumerator DownloadTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
        }
        else
        {
            DownloadHandlerTexture response = (DownloadHandlerTexture) request.downloadHandler;
            Texture texture = response.texture;
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.mainTexture = texture;
                renderer.material.SetTexture("_MainTex", texture);
            }
    }

        [JsonObject(MemberSerialization.OptIn)]
        public class MinionData {

        [JsonProperty("cardId")]
        public string CardId;
    }
}
