using System;
using UnityEngine;
using System.Collections;

public class MacAspectRatioController : MonoBehaviour
{
    // Aspect ratio width and height.
    [SerializeField] private float aspectRatioWidth = 16;
    [SerializeField] private float aspectRatioHeight = 9;

    // Width and height of active monitor. This is the monitor the window is currently on.
    private float pixelHeightOfCurrentScreen;
    private float pixelWidthOfCurrentScreen;

    // Currently locked aspect ratio.
    private float aspect;

// Use this for initialization
   void Start()
    {
        aspect = aspectRatioWidth / aspectRatioHeight;

        pixelHeightOfCurrentScreen = Screen.currentResolution.height;
        pixelWidthOfCurrentScreen = Screen.currentResolution.width;
        if (Math.Abs(pixelWidthOfCurrentScreen / pixelHeightOfCurrentScreen - aspect) > 0.1f)
        {
            // Switch to fullscreen detected. Set to max screen resolution while keeping the aspect ratio.
            int height;
            int width;

            // Check where black bars will have to be added. Depending on the current aspect ratio and the actual aspect
            // ratio of the monitor, there will be black bars to the left/right or top/bottom.
            // There could of course also be none at all if the aspect ratios match exactly.
            bool blackBarsLeftRight = aspect < (float) pixelWidthOfCurrentScreen / pixelHeightOfCurrentScreen;

            if (blackBarsLeftRight)
            {
                height =  Mathf.RoundToInt(pixelHeightOfCurrentScreen) - 200;
                width = Mathf.RoundToInt(height * aspect);
            }
            else
            {
                width =  Mathf.RoundToInt(pixelWidthOfCurrentScreen) - 200;
                height = Mathf.RoundToInt(width / aspect);
            }
            Screen.SetResolution(width, height, false);
        }
    }
}