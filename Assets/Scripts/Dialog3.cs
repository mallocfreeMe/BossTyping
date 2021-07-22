using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog3 : MonoBehaviour
{

    private Animator _playerAnimator;
    
    private bool _isDying;

    public AudioClip attack1, attack2, enemyDeath;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {

        _playerAnimator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAniamtion()
	{
        _playerAnimator.SetTrigger("IsAttacking");
  
        _audioSource.PlayOneShot(attack1, 1F);
        _audioSource.PlayOneShot(attack2, 1F);
        _audioSource.PlayOneShot(enemyDeath, 1F);

    }
    public void PlayAniamtion2()
    {
        _playerAnimator.SetTrigger("Fireup");

    }
}
