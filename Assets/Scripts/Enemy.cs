using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")] public int movingSpeed = 3;
    public TextMeshPro nameField;
    private Animator _enemyKnightAnimator;

    private Vector3 _pos;
    private string[] _namesPool =
        {"Saint", "George", "Galahad", "Siegfried", "Robert"};

    private void Start()
    {
        _pos = transform.position;
        nameField.text = _namesPool[Random.Range(0, _namesPool.Length)];
        _enemyKnightAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        _pos += new Vector3(-movingSpeed, 0, 0) * Time.deltaTime;
        transform.position = _pos;
        _enemyKnightAnimator.SetTrigger("EnemyIsWalking");
    }
}