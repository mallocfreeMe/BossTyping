using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")] public int movingSpeed = 3;
    public TextMeshPro nameField;
    public TextMeshPro selectedName;
    public Animator animator;

    // private Vector3 _pos;

    private string[] _namesPool =
    {
        "Saint", "George", "Galahad", "Siegfried", "Robert", "Liam", "Noah", "Oliver", "Elijah", "Alexander", "Henry",
        "Lucas", "Benjamin"
    };

    private void Start()
    {
        nameField.text = _namesPool[Random.Range(0, _namesPool.Length)];
    }

    private void Update()
    {
        transform.position += new Vector3(-movingSpeed, 0, 0) * Time.deltaTime;
    }
}