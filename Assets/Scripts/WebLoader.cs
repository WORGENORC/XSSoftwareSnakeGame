using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebLoader : MonoBehaviour
{
    public string URLImage = "https://previews.123rf.com/images/lilu330/lilu3301603/lilu330160300039/53557709-cartoon-nature-landscape-vector-background-for-game-design-vertical-format-for-mobile-phone-screen.jpg";
    public Image image;

    void Start()
    {
        DownloadImage(URLImage);
    }

    public void DownloadImage(string url)
    {
        StartCoroutine(ImageRequest(url, (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(req);
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }));
    }

    IEnumerator ImageRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            yield return req.SendWebRequest();
            callback(req);
        }
    }
}
