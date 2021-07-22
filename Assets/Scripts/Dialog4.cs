using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog4 : MonoBehaviour
{

    private Animator _enemyAnimator;
    private bool _isDying;

    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {

        _enemyAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAniamtion()
	{
        _enemyAnimator.SetTrigger("EnemyIsDying");

    }

}
