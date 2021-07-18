using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform spawnPoint;
    public Enemy enemyPrefab;
    public TextMeshProUGUI pointUI;
    public Slider healthBar;
    public TextMeshProUGUI countDownUI;

    private float _countDownTimer = 3;
    private bool _gameIsStart;

    private List<Enemy> _enemies;
    private const int EnemyLength = 10;
    private int _enemiesIndex;
    private string _userInput;

    private int _pointer;
    private int _scores;
    private int _difficulties = 1;

    private const int MaxHealth = 3;
    private int _healthPoint = MaxHealth;

    private void Start()
    {
        _enemies = new List<Enemy>();
        for (var i = 0; i < EnemyLength; i++)
        {
            var enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemy.gameObject.SetActive(false);
            _enemies.Add(enemy);
        }

        healthBar.maxValue = MaxHealth;
        healthBar.value = MaxHealth;
    }

    private void Update()
    {
        GameCountDown();

        if (_gameIsStart)
        {
            _enemies[_enemiesIndex].gameObject.SetActive(true);

            if (_userInput == _enemies[_enemiesIndex].nameField.text)
            {
                CleanSteps();
                // add points
                _scores++;
                pointUI.text = "Points: " + _scores;
            }

            // add difficulties
            if (_scores > _difficulties * 5)
            {
                _difficulties++;
                foreach (var e in _enemies)
                {
                    e.nameField.text += "a";
                    e.movingSpeed += 1;
                }
            }
        }
    }

    // detect which keys the player presses 
    private void OnGUI()
    {
        if (_gameIsStart)
        {
            var e = Event.current;
            var enemyName = _enemies[_enemiesIndex].nameField.text;

            if (e.isKey && e.keyCode != KeyCode.None)
            {
                if (_pointer > enemyName.Length)
                {
                    _pointer = enemyName.Length;
                }

                if (enemyName.ToLower()[_pointer] == e.keyCode.ToString().ToLower()[0])
                {
                    _userInput += enemyName[_pointer];
                    _pointer++;
                }
            }
        }
    }

    // enemies collide with the player 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            CleanSteps();
            _healthPoint--;
            healthBar.value--;
            if (_healthPoint == 0)
            {
                PlayerPrefs.SetInt("Points", _scores);
                SceneManager.LoadScene("Game Over");
            }
        }
    }

    // Set Count Down Timer
    // Hide the UI Component
    private void GameCountDown()
    {
        if (!_gameIsStart)
        {
            if (countDownUI.text != "Go!")
            {
                countDownUI.text = "" + Math.Round(_countDownTimer);
            }
            _countDownTimer -= Time.deltaTime;
            
            if (_countDownTimer <= 1)
            {
                countDownUI.text = "Go!";
            }

            if (_countDownTimer <= 0)
            {
                countDownUI.gameObject.SetActive(false);
                _gameIsStart = true;
            }
        }
    }

    // hide enemies, adjust pos
    // clean the index
    private void CleanSteps()
    {
        _enemies[_enemiesIndex].transform.position = spawnPoint.position;
        _enemies[_enemiesIndex].gameObject.SetActive(false);
        _userInput = "";
        _pointer = 0;
        if (_enemiesIndex < EnemyLength - 1)
        {
            _enemiesIndex++;
        }
        else
        {
            _enemiesIndex = 0;
        }
    }
}