using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform spawnPoint;
    public Enemy enemyPrefab;
    public TextMeshProUGUI pointUI;

    private List<Enemy> _enemies;
    private const int EnemyLength = 10;
    private int _enemiesIndex;
    private string _userInput;
    private int _pointer;
    private int _scores;
    private int _difficulties = 1;

    private void Start()
    {
        _enemies = new List<Enemy>();
        for (var i = 0; i < EnemyLength; i++)
        {
            var enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemy.gameObject.SetActive(false);
            _enemies.Add(enemy);
        }
    }

    private void Update()
    {
        _enemies[_enemiesIndex].gameObject.SetActive(true);

        if (_userInput == _enemies[_enemiesIndex].nameField.text)
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

    private void OnGUI()
    {
        var e = Event.current;
        var enemyName = _enemies[_enemiesIndex].nameField.text;
        
        if (e.isKey && e.keyCode != KeyCode.None)
        {
            if (enemyName.ToLower()[_pointer] == e.keyCode.ToString().ToLower()[0])
            {
                _userInput += enemyName[_pointer];
                _pointer++;
            }
        }
    }
}