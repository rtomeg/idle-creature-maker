using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField channelNameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField maxCreatureInput;
    [SerializeField] private Toggle allowMultipleCreatures;

    [SerializeField] private Button startButton;
    [SerializeField] private GameObject errorText;
    [SerializeField] private GameObject loading;

    private TwitchController twitchController;
    private float timeout = 4;
    private float timer;
    private bool tryingToConnect;

    private void Start()
    {
        twitchController = TwitchController.Instance;
        maxCreatureInput.text = PlayerPrefs.GetInt(GameController.MAX_CREATURES_KEY, 50).ToString();
        allowMultipleCreatures.isOn = PlayerPrefs.GetInt(GameController.ALLOW_MULTIPLE_CREATURES) != 0;
    }

    public void StartGame()
    {
        int i = int.TryParse(maxCreatureInput.text, out i) ? i : 50;
        i = i < 10 ? 10 : i;
        PlayerPrefs.SetInt(GameController.MAX_CREATURES_KEY, i);
        PlayerPrefs.SetInt(GameController.ALLOW_MULTIPLE_CREATURES, (allowMultipleCreatures.isOn ? 1 : 0));
        SceneManager.LoadScene("GameScene");
    }

    private void Update()
    {
        if (tryingToConnect)
        {
            if (timer <= timeout)
            {
                timer += Time.deltaTime;
            }
            else
            {
                OnCountdownEnd();
            }
        }

        if (twitchController.isConnected)
        {
            StartGame();
        }
    }

    public void TryLogin()
    {
        loading.SetActive(true);
        modifyInputStatus(false);
        tryingToConnect = true;
        startButton.interactable = false;
        twitchController.Login(usernameField.text, channelNameField.text, passwordField.text);
    }

    public void OnCountdownEnd()
    {
        loading.SetActive(false);
        tryingToConnect = false;
        errorText.SetActive(true);
        modifyInputStatus(true);
        timer = 0;
    }

    public void OnInputChanged()
    {
        errorText.SetActive(false);
        startButton.interactable = true;
    }

    private void modifyInputStatus(bool status)
    {
        usernameField.enabled = status;
        channelNameField.enabled = status;
        passwordField.enabled = status;
    }
}