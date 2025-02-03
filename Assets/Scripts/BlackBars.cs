using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioUtility : MonoBehaviour
{
    private int lastWidth;
    private int lastHeight;

    void Start()
    {
        // Initialize the lastWidth and lastHeight with the current screen size
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        // Call Adjust on Start to ensure it's set initially
        Adjust();
    }

    void Update()
    {
        // Check if the screen size has changed since the last check
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            // Adjust and update the last known screen size
            Adjust();
            lastWidth = Screen.width;
            lastHeight = Screen.height;
        }
    }

    public void Adjust()
    {
        float targetaspect = 16.0f / 9.0f;  // Slightly increase the aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;

        Camera camera = GetComponent<Camera>();

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
            camera.rect = rect;
        }
        else
        {
            float scalewidth = 1.0f / scaleheight;
            Rect rect = camera.rect;
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }
    }


    //public void Adjust()
    //{
    //    float targetaspect = 16.0f / 9.0f;
    //    float windowaspect = (float)Screen.width / (float)Screen.height;
    //    float scaleheight = windowaspect / targetaspect;

    //    Camera camera = GetComponent<Camera>();

    //    if (scaleheight < 1.0f)
    //    {
    //        Rect rect = camera.rect;
    //        rect.width = 1.0f;
    //        rect.height = scaleheight;
    //        rect.x = 0;
    //        rect.y = (1.0f - scaleheight) / 2.0f;
    //        camera.rect = rect;
    //    }
    //    else
    //    {
    //        float scalewidth = 1.0f / scaleheight;
    //        Rect rect = camera.rect;
    //        rect.width = scalewidth;
    //        rect.height = 1.0f;
    //        rect.x = (1.0f - scalewidth) / 2.0f;
    //        rect.y = 0;
    //        camera.rect = rect;
    //    }
    //}
}
