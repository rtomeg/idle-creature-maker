using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    [SerializeField] private TMP_InputField channelName;
    [SerializeField] private TMP_InputField maxCreatureInput;
    [SerializeField] private Toggle allowMultipleCreatures;

    [SerializeField] private Button playButton;
    [SerializeField] private Button connectToChannelButton;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private GameObject loading;

    private TwitchController twitchController;
    
    private bool testingConnection;

    
    private static string usernameKey = "username";

    private void Start()
    {
        twitchController = TwitchController.Instance;
        maxCreatureInput.text = PlayerPrefs.GetInt(GameController.MAX_CREATURES_KEY, 50).ToString();
        allowMultipleCreatures.isOn = PlayerPrefs.GetInt(GameController.ALLOW_MULTIPLE_CREATURES) != 0;
        channelName.text = PlayerPrefs.GetString(usernameKey);
        Application.targetFrameRate = 60;
    }

    public void StartGame()
    {
        int i = int.TryParse(maxCreatureInput.text, out i) ? i : 50;
        i = i < 10 ? 10 : i;
        PlayerPrefs.SetInt(GameController.MAX_CREATURES_KEY, i);
        PlayerPrefs.SetInt(GameController.ALLOW_MULTIPLE_CREATURES, (allowMultipleCreatures.isOn ? 1 : 0));
        SceneManager.LoadScene("GameScene");
    }

    public void OnTestConnectionButtonClick()
    {
        if (testingConnection) return;
        testingConnection = true;
        connectToChannelButton.interactable = false;
        
        if (!String.IsNullOrEmpty(channelName.text))
        {
            int startIndex = channelName.text.IndexOf(".tv/") == -1 ? 0 : channelName.text.IndexOf(".tv/") + 4;
            channelName.text = channelName.text.TrimEnd('/');
            string trimmedChannelName = channelName.text.Substring(startIndex, channelName.text.Length - startIndex);

            if (Regex.IsMatch(trimmedChannelName, "(^[a-zA-Z0-9][\\w]{2,24})$"))
            {
                StartCoroutine(TestConnection(trimmedChannelName));
                return;
            }
        }
        PrintErrorMessage("Channel Name not valid.", Color.red);
        connectToChannelButton.interactable = true;
        testingConnection = false;
    }
    
    private IEnumerator TestConnection(string cName)
    {
        twitchController.Login(cName);
        PrintErrorMessage("Connecting...", Color.cyan);
        yield return new WaitForSeconds(2);
        if (twitchController.isConnected)
        {
            Connected(true);
        }
        else
        {
            Connected(false);
        }
    }
    
    private void Connected(bool connected)
    {
        if (connected)
        {
            TwitchController.Instance.Initialize(channelName.text);
        }
        PrintErrorMessage(connected?"Connected!":"Connection Failed, please try again", connected?Color.white:Color.red);
        playButton.interactable = connected;
        channelName.interactable = !connected;
        connectToChannelButton.interactable = !connected;
        testingConnection = false;
        PlayerPrefs.SetString(usernameKey, channelName.text);
    }
    
    private void PrintErrorMessage(string message, Color c)
    {
        errorText.SetText(message);
        errorText.color = c;
    }
}