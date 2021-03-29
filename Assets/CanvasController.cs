using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject quitPanel;
     
    public void QuitApp()
    {
        Application.Quit();
    }

    public void OpenTwitch()
    {
        Application.OpenURL("https://www.twitch.tv/rothiotome");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            quitPanel.SetActive(true);
        }
    }
    
    
}
