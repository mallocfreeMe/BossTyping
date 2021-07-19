using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private Button _startButton;

    private void Start()
    {
        if (GetComponent<Button>())
        {
            _startButton = GetComponent<Button>();
            _startButton.onClick.AddListener(Play);
        }

        if (GetComponent<TextMeshProUGUI>())
        {
            GetComponent<TextMeshProUGUI>().text = "Your Best Points: " + PlayerPrefs.GetInt("Best") + "\n" +
                                                   "Your Final Points: " + PlayerPrefs.GetInt("Points");
        }
    }

    private static void Play()
    {
        SceneManager.LoadScene("Game");
    }
}