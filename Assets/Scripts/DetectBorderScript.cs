using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBorderScript : MonoBehaviour
{
    public RectTransform player;

    void Start()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        if (gameObject.tag == "LoR")
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(20, Screen.height * 2);
        }
        else if(gameObject.tag == "UoD")
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 2, 20);
        }  
    }

    void Update()
    {
        if (gameObject.GetComponent<RectTransform>().Overlaps(player))
        {
            //player.GetComponent<PlayerController>().canMove = false;
        }

    }
}

public static class ExtensionMethod
{
    public static bool Overlaps(this RectTransform a, RectTransform b)
    {
        return a.WorldRect().Overlaps(b.WorldRect());
    }
    public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse)
    {
        return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
    }

    public static Rect WorldRect(this RectTransform rectTransform)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        Vector3 position = rectTransform.position;
        return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth, rectTransformHeight);
    }
}
