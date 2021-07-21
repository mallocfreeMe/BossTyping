using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    private string _userInput = "";

    private int _pointer;
    private int _scores;
    private int _difficulties = 1;

    private const int MaxHealth = 6;
    private int _healthPoint = MaxHealth;

    private bool _offScreen = true;

    private Animator _playerAnimator;
    private bool _isDying;
    private float _deathAnimationTimer = 2;
    private bool _isCollide;

    public AudioClip attack1, attack2, hurt, enemyDeath;
    private AudioSource _audioSource;


    // some pre works 
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

        _playerAnimator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();
    }

    // main game loop
    private void Update()
    {
        GameCountDown();
        GamePlay();
        EndGame();
    }

    // Set Count Down Timer
    // Hide the UI Component
    private void GameCountDown()
    {
        if (!_gameIsStart && !_isDying)
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

    // check whether the user input match with the actual enemy's name
    // count scores, adjust difficulties, reset enemies
    private void GamePlay()
    {
        if (_gameIsStart && !_isDying)
        {
            _enemies[_enemiesIndex].gameObject.SetActive(true);

            if (_userInput == _enemies[_enemiesIndex].nameField.text)
            {
                _playerAnimator.SetTrigger("IsAttacking");
                _audioSource.PlayOneShot(attack1, 1F);
                _audioSource.PlayOneShot(attack2, 1F);
                _audioSource.PlayOneShot(enemyDeath, 1F);
                
                CleanSteps();

                // add points
                _scores++;
                pointUI.text = "Points: " + _scores;
            }

            // add difficulties
            if (_scores > _difficulties * 10)
            {
                _difficulties++;
                foreach (var e in _enemies)
                {
                    var c = (char) ('a' + UnityEngine.Random.Range(0, 26));
                    e.nameField.text += c;
                    // e.movingSpeed += 1;
                }
            }
        }
    }

    private IEnumerator PlayDeathAnimation()
    {
        var temp = Instantiate(enemyPrefab,
            new Vector2(_enemies[_enemiesIndex].transform.position.x, spawnPoint.position.y),
            Quaternion.identity);
        temp.animator.SetTrigger("EnemyIsDying");
        temp.movingSpeed = 0;
        temp.nameField.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(temp.gameObject);
    }

    // hide enemies, adjust pos
    // clean the index
    private void CleanSteps()
    {
        if (!_isCollide)
        {
            StartCoroutine(PlayDeathAnimation());
        }

        _offScreen = true;
        _enemies[_enemiesIndex].selectedName.text = "";
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

    // when enemies are not offscreen 
    // detect which keys the player presses 
    private void OnGUI()
    {
        if (_gameIsStart)
        {
            var dist = Vector2.Distance(_enemies[_enemiesIndex].transform.position, transform.position);
            if (dist <= 16)
            {
                _offScreen = false;
            }
        }

        if (_gameIsStart && _offScreen == false)
        {
            var e = Event.current;
            var enemyName = _enemies[_enemiesIndex].nameField.text;

            if (e.isKey && e.keyCode != KeyCode.None)
            {
                if (_pointer > enemyName.Length - 1)
                {
                    _pointer = enemyName.Length - 1;
                }

                if (enemyName.ToLower()[_pointer] == e.keyCode.ToString().ToLower()[0] &&
                    _userInput.Length < enemyName.Length)
                {
                    _userInput += enemyName[_pointer];
                    _enemies[_enemiesIndex].selectedName.text = _userInput;
                    if (_pointer < enemyName.Length - 1)
                    {
                        _pointer++;
                    }
                }
            }
        }
    }

    // play the death animation before jump to the another scene
    private void EndGame()
    {
        if (!_gameIsStart && _isDying)
        {
            healthBar.gameObject.SetActive(false);
            _playerAnimator.SetTrigger("IsDying");
            _deathAnimationTimer -= Time.deltaTime;
            if (_deathAnimationTimer <= 0)
            {
                SceneManager.LoadScene("Game Over");
            }
        }
    }

    // enemies collide with the player 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _isCollide = true;
            CleanSteps();
            _playerAnimator.SetTrigger("IsHurted");
            _audioSource.PlayOneShot(hurt, 1F);
            _healthPoint--;
            healthBar.value--;
            _isCollide = false;
            if (_healthPoint == 0)
            {
                PlayerPrefs.SetInt("Points", _scores);
                _gameIsStart = false;
                _isDying = true;

                if (PlayerPrefs.GetInt("Best") != 0)
                {
                    if (PlayerPrefs.GetInt("Points") > PlayerPrefs.GetInt("Best"))
                    {
                        PlayerPrefs.SetInt("Best", _scores);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt("Best", _scores);
                }
            }
        }
    }
}