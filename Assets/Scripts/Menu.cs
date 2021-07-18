using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private Button _startButton;

    private void Start()
    {
        _startButton = GetComponent<Button>();
        _startButton.onClick.AddListener(Play);
    }

    private static void Play()
    {
        SceneManager.LoadScene("Game");
    }
}
