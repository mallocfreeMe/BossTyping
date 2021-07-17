using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")] public int movingSpeed = 3;
    public TextMeshPro nameField;

    private Vector3 _pos;
    private string[] _namesPool =
        {"Eric", "Pan", "Yang"};

    private void Start()
    {
        _pos = transform.position;
        nameField.text = _namesPool[Random.Range(0, _namesPool.Length)];
    }

    private void Update()
    {
        _pos += new Vector3(-movingSpeed, 0, 0) * Time.deltaTime;
        transform.position = _pos;
    }
}